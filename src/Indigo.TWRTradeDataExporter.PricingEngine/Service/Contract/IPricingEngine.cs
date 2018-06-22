using Indigo.TWRTradeDataExporter.PricingEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract
{
    public interface IPricingEngine
    {
        IEnumerable<TWRPricingResult> CalculatePrices(string upc, string mcat, decimal price);
    }
}
