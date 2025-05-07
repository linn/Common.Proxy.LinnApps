namespace Linn.Common.Proxy.LinnApps.Services
{
    using System.Data;
    using System.Threading.Tasks;

    using Oracle.ManagedDataAccess.Client;

    public interface IDatabaseService
    {
        OracleConnection GetConnection();

        int GetIdSequence(string sequenceName);

        Task<int> GetIdSequenceAsync(string sequenceName);

        int GetNextVal(string sequenceName);

        Task<int> GetNextValAsync(string sequenceName);

        DataSet ExecuteQuery(string sql);
    }
}
