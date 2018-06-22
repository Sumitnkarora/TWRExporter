using Indigo.TWRTradeDataExporter.DataExtractor.Models;
using System.Collections.Generic;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract
{
    public interface ITradeDataProvider
    {
        void ExecuteTradeAutoApproval(string sapVendorId = null);

        IEnumerable<TradeExportData> RetrieveExportData();

        void UpdateExportedItemStatus(string isbn13);
    }
}
