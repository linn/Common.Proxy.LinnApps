namespace Linn.Common.Proxy.LinnApps.Services
{
    using System.Data;

    using Oracle.ManagedDataAccess.Client;

    public interface IDatabaseService
    {
        // todo - async overloads
        OracleConnection GetConnection();

        int GetIdSequence(string sequenceName);

        int GetNextVal(string sequenceName);

        DataSet ExecuteQuery(string sql);
    }
}
