using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Models
{
    public class TWRPricing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TWRLevel { get; set; }
        public bool IsEnabled { get; set; }

        public List<TWRDiscount> Discounts { get; set; }

        public TWRPricingResult CalculatePrices(string upc, string mcat, decimal price)
        {
            TWRPricingResult output = new TWRPricingResult()
            {
                PricingId = this.Id,
                PriceWasCalculated = false,
                TWRLevel = this.TWRLevel,
                Price = price
            };

            if (Discounts == null || Discounts.Count == 0)
            {
                return output;
            }

            foreach (TWRDiscount discount in Discounts)
            {
                decimal discountOutput = discount.CalculatePrice(upc, mcat, price);

                if (discount.ConditionMet)
                {
                    if (discountOutput <= output.Price)
                    {
                        if (!output.PriceWasCalculated || discountOutput < output.Price)
                        {
                            output.Price = discountOutput;
                            output.ConditionId = discount.Id;
                            output.PriceWasCalculated = true;
                        }
                    }
                }
            }

            return output;
        }
    }
}
