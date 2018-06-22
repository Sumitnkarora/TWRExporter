using Indigo.TWRTradeDataExporter.PricingEngine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract
{
    public interface IPricingDataProvider
    {
        IEnumerable<TWRDiscount> GetDiscountEntries();

        IEnumerable<TWRDiscountEligibility> GetDiscountEligibilityEntries();

        IEnumerable<TWRPricing> GetPricingEntries();
    }
}
