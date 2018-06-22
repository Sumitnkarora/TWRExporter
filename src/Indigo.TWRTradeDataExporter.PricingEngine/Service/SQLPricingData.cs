using Indigo.TWRTradeDataExporter.PricingEngine.Models;
using Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Service
{
    public class SQLPricingData : IPricingDataProvider
    {
        private readonly string _connectionString;
        private readonly int _commandTimeout;

        public SQLPricingData() : this(null)
        {
        }

        public SQLPricingData(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = ConfigurationHelper.GetConnectionString("PricingEngineDB");
            }

            _connectionString = connectionString;
            var dct = ConfigurationHelper.GetAppConfig("DataCommandTimeout", false, "60");
            if (!int.TryParse(dct, out _commandTimeout))
            {
                _commandTimeout = 60;
            }
        }

        public IEnumerable<TWRDiscountEligibility> GetDiscountEligibilityEntries()
        {
            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                return cn.Query<TWRDiscountEligibility>("spTWRDiscountEligibility", commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }

        public IEnumerable<TWRDiscount> GetDiscountEntries()
        {
            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                return cn.Query<TWRDiscount>("spTWRDiscounts", commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }

        public IEnumerable<TWRPricing> GetPricingEntries()
        {
            using (SqlConnection cn = new SqlConnection(this._connectionString))
            {
                return cn.Query<TWRPricing>("spTWRPricing", commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }
    }
}
