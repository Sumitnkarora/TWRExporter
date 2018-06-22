using Indigo.TWRTradeDataExporter.PricingEngine.Models;
using Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Service
{
    public class TWRPricingEngine : IPricingEngine
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(TWRPricingEngine));
        private readonly IPricingDataProvider _dao;
        private List<TWRPricing> pricingRules = null;

        public TWRPricingEngine(IPricingDataProvider dao)
        {
            _dao = dao ?? throw new ArgumentNullException("dao");

            this.LoadData();
        }

        public IEnumerable<TWRPricingResult> CalculatePrices(string upc, string mcat, decimal price)
        {
            foreach (var pricing in pricingRules)
            {
                var result = pricing.CalculatePrices(upc, mcat, price);

                if (result.PriceWasCalculated)
                {
                    yield return result;
                }
            }
        }

        private void LoadData()
        {
            IEnumerable<TWRPricing> pricing;
            IEnumerable<TWRDiscount> discounts;
            IEnumerable<TWRDiscountEligibility> criteria;

            try
            {
                pricing = _dao.GetPricingEntries();
                discounts = _dao.GetDiscountEntries();
                criteria = _dao.GetDiscountEligibilityEntries();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Failed to retrieve pricing data for pricing engine from database.";
                log.Error(errorMessage, ex);
                throw;
            }

            pricingRules = pricing.Where(x => x.IsEnabled).Select(
                    x =>
                    {
                        x.Discounts = discounts.Where(y => y.IsEnabled && y.PricingId == x.Id).Select(
                            y =>
                            {
                                y.InclusionParameters = criteria.Where(z => z.DiscountId == y.Id && z.ParameterType == "I").ToList();
                                y.ExclusionParameters = criteria.Where(z => z.DiscountId == y.Id && z.ParameterType == "E").ToList();

                                return y;
                            }
                        ).ToList();

                        return x;
                    }
            ).ToList();
        }
    }
}
