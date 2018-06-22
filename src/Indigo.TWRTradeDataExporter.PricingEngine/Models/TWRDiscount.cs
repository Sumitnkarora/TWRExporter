using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Models
{
    public class TWRDiscount
    {
        public int PricingId { get; set; }
        public int Id { get; set; }
        public string ResultType { get; set; }
        public decimal ResultValue { get; set; }
        public bool IsEnabled { get; set; }

        public bool ConditionMet { get; set; }

        public List<TWRDiscountEligibility> InclusionParameters { get; set; }

        public List<TWRDiscountEligibility> ExclusionParameters { get; set; }

        public decimal CalculatePrice(string upc, string mcat, decimal price)
        {
            this.ConditionMet = false;

            decimal output = price;

            if (CheckEligibility(upc, mcat))
            {
                this.ConditionMet = true;

                switch (this.ResultType.ToUpper())
                {
                    case "D":
                        output = output - ResultValue;
                        if (output < 0) return 0;
                        break;
                    case "F":
                        output = ResultValue;
                        break;
                    case "P":
                        output = output * (1.00m - ResultValue);
                        if (output < 0) return 0;
                        break;
                }
            }

            return output;
        }

        private bool CheckEligibility(string upc, string mcat)
        {
            // If there are no includes OR excludes, returns true;
            bool defaultOutput = true;

            if (ExclusionParameters != null && ExclusionParameters.Count > 0)
            {
                // This is here in case the default would otherwise be false.
                defaultOutput = true;

                foreach (TWRDiscountEligibility param in ExclusionParameters)
                {
                    if (param.IsMatch(upc, mcat))
                    {
                        return false;
                    }
                }
            }

            if (InclusionParameters != null && InclusionParameters.Count > 0)
            {
                defaultOutput = false;

                foreach (TWRDiscountEligibility param in InclusionParameters)
                {
                    if (param.IsMatch(upc, mcat))
                    {
                        return true;
                    }
                }
            }

            return defaultOutput;
        }
    }
}
