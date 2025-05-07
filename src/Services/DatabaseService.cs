namespace Linn.Common.Proxy.LinnApps.Services
{
    using System.Data;
    using System.Threading.Tasks;

    using Linn.Common.Proxy.LinnApps;
    using global::Oracle.ManagedDataAccess.Client;

    public class DatabaseService : IDatabaseService
    {
        public OracleConnection GetConnection()
        {
            OracleConfiguration.TraceLevel = 0;
            return new OracleConnection(ConnectionStrings.ManagedConnectionString());
        }

        public int GetNextVal(string sequenceName)
        {
            using var connection = this.GetConnection();

            connection.Open();
            var (cmd, result) = GetNextValCmd(sequenceName, connection);

            cmd.ExecuteNonQuery();
            connection.Close();
            var res = result.Value.ToString();
            return int.Parse(res);
        }

        public async Task<int> GetNextValAsync(string sequenceName)
        {
            await using var connection = this.GetConnection();

            await connection.OpenAsync();
            var (cmd, result) = GetNextValCmd(sequenceName, connection);

            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            var res = result.Value.ToString();
            return int.Parse(res);
        }

        public DataSet ExecuteQuery(string sql)
        {
            using (var connection = this.GetConnection())
            {
                var dataAdapter = new OracleDataAdapter(
                    new OracleCommand(sql, connection) { CommandType = CommandType.Text });
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        public int GetIdSequence(string sequenceName)
        {
            using var connection = this.GetConnection();
            var (cmd, result) = CreateGetSequenceCommand(connection, sequenceName);

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

            return int.Parse(result.Value.ToString());
        }

        public async Task<int> GetIdSequenceAsync(string sequenceName)
        {
            await using var connection = this.GetConnection();
            var (cmd, result) = CreateGetSequenceCommand(connection, sequenceName);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return int.Parse(result.Value.ToString());
        }

        private static (OracleCommand cmd, OracleParameter result) GetNextValCmd(string sequenceName, OracleConnection connection)
        {
            var cmd = new OracleCommand("cg_code_controls_next_val", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };

            var result = new OracleParameter(null, OracleDbType.Int32)
                             {
                                 Direction = ParameterDirection.ReturnValue,
                                 Size = 50
                             };
            cmd.Parameters.Add(result);

            var parameter = new OracleParameter("p_cc_domain", OracleDbType.Varchar2)
                                {
                                    Direction = ParameterDirection.Input,
                                    Value = sequenceName
                                };
            cmd.Parameters.Add(parameter);

            var num = new OracleParameter("p_increment", OracleDbType.Int32)
                          {
                              Direction = ParameterDirection.Input,
                              Value = 1
                          };
            cmd.Parameters.Add(num);
            return (cmd, result);
        }

        private static (OracleCommand cmd, OracleParameter result) CreateGetSequenceCommand(OracleConnection connection, string sequenceName)
        {
            var cmd = new OracleCommand("get_next_sequence_value", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };

            var result = new OracleParameter(string.Empty, OracleDbType.Int32)
                             {
                                 Direction = ParameterDirection.ReturnValue
                             };
            cmd.Parameters.Add(result);

            var sequenceParameter = new OracleParameter("p_sequence", OracleDbType.Varchar2)
                                        {
                                            Direction = ParameterDirection.Input,
                                            Size = 50,
                                            Value = sequenceName
                                        };
            cmd.Parameters.Add(sequenceParameter);

            return (cmd, result);
        }
    }
}
