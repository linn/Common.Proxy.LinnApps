namespace Linn.Common.Proxy.LinnApps.Services
{
    using System.Data;
    using System.Threading.Tasks;

    using Linn.Common.Domain.LinnApps.Services;

    using global::Oracle.ManagedDataAccess.Client;

    public class BartenderLabelPack : ILabelPrinter
    {
        public async Task<(bool Success, string Message)> PrintLabelsAsync(
            string fileName, string printer, int qty, string template, string data)
        {
            var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());
            var cmd = new OracleCommand("BARTENDER.PRINT_LABELS_WRAPPER", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };

            var result = new OracleParameter(null, OracleDbType.Int32)
                             {
                                 Direction = ParameterDirection.ReturnValue,
                                 Size = 2000
                             };
            cmd.Parameters.Add(result);

            cmd.Parameters.Add(
                new OracleParameter("p_filename", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.Input,
                        Size = 50,
                        Value = fileName
                    });
            cmd.Parameters.Add(
                new OracleParameter("p_printer", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.Input,
                        Size = 100,
                        Value = printer
                    });
            cmd.Parameters.Add(
                new OracleParameter("p_qty", OracleDbType.Int32)
                    {
                        Direction = ParameterDirection.Input, Value = qty
                    });
            cmd.Parameters.Add(
                new OracleParameter("p_template", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.Input,
                        Size = 100,
                        Value = template
                    });
            cmd.Parameters.Add(
                new OracleParameter("p_data", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.Input,
                        Size = 4000,
                        Value = data
                    });
            var message = string.Empty;

            cmd.Parameters.Add(
                new OracleParameter("p_message", OracleDbType.Varchar2)
                    {
                        Direction = ParameterDirection.InputOutput,
                        Size = 500,
                        Value = message
                    });

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            var success = result.Value.ToString() == "1";

            return (success, message);
        }
    }
}
