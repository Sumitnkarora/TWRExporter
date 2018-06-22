using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Models
{
    public class TWRPricingResult
    {
        public int PricingId { get; set; }
        public int ConditionId { get; set; }
        public string TWRLevel { get; set; }
        public bool PriceWasCalculated { get; set; }
        public decimal Price { get; set; }
    }
}
