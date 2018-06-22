using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract;
using Indigo.TWRTradeDataExporter.DataExtractor.Models;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Service
{
    public class SQLTradeData : ITradeDataProvider
    {
        private readonly string _connectionString;
        private readonly int _commandTimeout;

        public SQLTradeData() : this(null)
        {
        }

        public SQLTradeData(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = ConfigurationHelper.GetConnectionString("TradeDbUs");
            }

            _connectionString = connectionString;
            var dct = ConfigurationHelper.GetAppConfig("DataCommandTimeout", false, "60");
            if (!int.TryParse(dct, out _commandTimeout))
            {
                _commandTimeout = 60;
            }
        }

        public void ExecuteTradeAutoApproval(string sapVendorId = null)
        {
            if (string.IsNullOrWhiteSpace(sapVendorId))
            {
                sapVendorId = ConfigurationHelper.GetAppConfig("BTSAPVendorId", true, string.Empty);
            }

            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                cn.Execute("TradeAutoApproval", new { SAPVendorID = sapVendorId }, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }

        public IEnumerable<TradeExportData> RetrieveExportData()
        {
            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                return cn.Query<TradeExportData>("spTradeDataExport", commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }

        public void UpdateExportedItemStatus(string isbn13)
        {
            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                cn.Execute("spUpdateExportedItemStatus", new { ISBN13 = isbn13 }, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }
    }
}
