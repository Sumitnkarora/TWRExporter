using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.PricingEngine.Models
{
    public class TWRDiscountEligibility
    {
        private bool validated = false;

        private Func<string, string, bool> matchMethod = null;

        private const string MERCH_CAT_P10 = "P10";
        private const string MERCH_CAT_M10 = "M10";
        private const string MERCH_CAT_G10 = "G10";
        private const string MERCH_CAT_F10 = "F10";

        public TWRDiscountEligibility()
        {

        }

        public TWRDiscountEligibility(string ParameterType, string ParameterGroup, string ParameterValue)
        {
            this.ParameterType = ParameterType;
            this.ParameterGroup = ParameterGroup;
            this.ParameterValue = ParameterValue;
        }

        public int DiscountId { get; set; }
        public int Id { get; set; }
        public string ParameterType { get; set; }
        public string ParameterGroup { get; set; }
        public string ParameterValue { get; set; }

        public bool IsMatch(string upc, string mcat)
        {
            this.Validate();

            return this.matchMethod(upc, mcat);
        }

        private void Validate()
        {
            // This function is used to optimize the pricing conditions
            // The first check of any condition is a little slow, subsequent ones
            // are quick.
            if (validated)
            {
                return;
            }

            ParameterValue = ParameterValue.Trim();

            if (ParameterGroup == "U")
            {
                ParameterValue = ParameterValue.Trim(new[] { '0' });
            }
            else
            {
                ParameterValue = ParameterValue.ToUpper();
            }

            switch (this.ParameterGroup.ToUpper())
            {
                case "M":
                    if (this.ParameterValue.Length == 6)
                    {
                        matchMethod = McatMatch;
                    }
                    break;
                case "D":
                    if (this.ParameterValue.Length == 4)
                    {
                        matchMethod = DepartmentMatch;
                    }
                    break;
                case "U":
                    if (this.ParameterValue.Length > 0)
                    {
                        matchMethod = UpcMatch;
                    }
                    break;
                case "A":
                    matchMethod = ((a, b) => { return true; });
                    break;
            }

            if (matchMethod == null)
            {
                matchMethod = ((a, b) => { return false; });
            }

            validated = true;
        }

        private bool McatMatch(string upc, string mcat)
        {
            string m = mcat.Trim().ToUpper();

            if (this.ParameterValue.Equals(m, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private bool DepartmentMatch(string upc, string mcat)
        {
            string m = mcat.Trim().ToUpper();

            if (m.Length != 6)
            {
                return false;
            }

            string d = string.Empty;

            switch (m.Substring(0, 3))
            {
                case MERCH_CAT_P10:
                    d = "1";
                    break;
                case MERCH_CAT_M10:
                    d = "2";
                    break;
                case MERCH_CAT_G10:
                    d = "3";
                    break;
                case MERCH_CAT_F10:
                    d = "6";
                    break;
                default:
                    return false;
            }

            d = d + m.Substring(3, 3);

            if (d == this.ParameterValue)
            {
                return true;
            }

            return false;
        }

        private bool UpcMatch(string upc, string mcat)
        {
            if (this.ParameterValue.Equals(upc, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
