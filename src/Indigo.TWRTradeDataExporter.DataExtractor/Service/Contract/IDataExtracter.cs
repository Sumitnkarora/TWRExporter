using Indigo.TWRTradeDataExporter.DataExtractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract
{
    public interface IDataExtracter
    {
        IEnumerable<TradeData> GetTradeDataForExport();

        void UpdateExportedItemStatus(string isbn13);

        string SerializeTradeData(TradeData data);
    }
}
