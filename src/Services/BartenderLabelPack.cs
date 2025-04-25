namespace Linn.Common.Proxy.LinnApps.Services
{
    using System.Data;
    using System.Threading.Tasks;

    using Linn.Common.Domain.LinnApps.Services;

    using global::Oracle.ManagedDataAccess.Client;

    public class BartenderLabelPack : ILabelPrinter
    {
        public Task<(bool Success, string Message)> PrintLabelsAsync(string fileName, string printer, int qty, string template, string data)
        {
            throw new System.NotImplementedException();
        }
    }
}
