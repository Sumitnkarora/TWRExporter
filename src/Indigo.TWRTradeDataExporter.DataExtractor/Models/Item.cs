using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Models
{
    public partial class Item
    {
        public Item()
        {
            Prices = new List<Price>();
            UPCs = new List<Upc>();
        }
        [JsonProperty("WeeksOfSupply")]
        public int WeeksOfSupply { get; set; }

        [JsonProperty("EID")]
        public string Eid { get; set; }

        //[JsonProperty("PLU")]
        //public string Plu { get; set; }

        [JsonProperty("CLU")]
        public string Clu { get; set; }

        [JsonProperty("UPCs")]
        public List<Upc> UPCs { get; set; }

        [JsonProperty("Attribute1")]
        public string Attribute1 { get; set; }

        [JsonProperty("Attribute2")]
        public string Attribute2 { get; set; }

        [JsonProperty("Height")]
        public decimal Height { get; set; }

        [JsonProperty("Length")]
        public decimal Length { get; set; }

        [JsonProperty("Weight")]
        public decimal Weight { get; set; }

        [JsonProperty("Width")]
        public decimal Width { get; set; }

        [JsonProperty("OrderCost")]
        public decimal OrderCost { get; set; }

        [JsonProperty("BasePrice")]
        public decimal BasePrice { get; set; }

        [JsonProperty("Prices")]
        public List<Price> Prices { get; set; }

        [JsonProperty("ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("NotTrackOH")]
        public bool NotTrackOh { get; set; }

        [JsonProperty("TradeDiscount")]
        public bool TradeDiscount { get; set; }

        [JsonProperty("MemberDiscount")]
        public bool MemberDiscount { get; set; }

        [JsonProperty("Inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("RequireDiscountAuthorizationCode")]
        public bool RequireDiscountAuthorizationCode { get; set; }

        [JsonProperty("AcceptToken")]
        public bool AcceptToken { get; set; }

        [JsonProperty("AutoPromptToPayWithTokens")]
        public bool AutoPromptToPayWithTokens { get; set; }

        [JsonProperty("EligibleForLoyaltyRewards2")]
        public bool EligibleForLoyaltyRewards2 { get; set; }

        [JsonProperty("LoyaltyRewards2Ratio")]
        public int LoyaltyRewards2Ratio { get; set; }

        //[JsonProperty("Name")]
        //public string Name { get; set; }

        //[JsonProperty("LookupName")]
        //public string LookupName { get; set; }

        [JsonProperty("CustomDate1")]
        public DateTime? CustomDate1 { get; set; }

        [JsonProperty("CustomFlag3")]
        public bool CustomFlag3 { get; set; }

        [JsonProperty("CustomFlag4")]
        public bool CustomFlag4 { get; set; }

        [JsonProperty("CustomFlag5")]
        public bool CustomFlag5 { get; set; }

        [JsonProperty("CustomFlag6")]
        public bool CustomFlag6 { get; set; }

        [JsonProperty("CustomFlag7")]
        public bool CustomFlag7 { get; set; }

        [JsonProperty("CustomNumber3")]
        public int CustomNumber3 { get; set; }

        [JsonProperty("CustomNumber4")]
        public int CustomNumber4 { get; set; }

        [JsonProperty("CustomNumber5")]
        public int CustomNumber5 { get; set; }

        [JsonProperty("CustomNumber6")]
        public int CustomNumber6 { get; set; }

        [JsonProperty("CustomNumber7")]
        public int CustomNumber7 { get; set; }

        [JsonProperty("CustomNumber8")]
        public int CustomNumber8 { get; set; }

        [JsonProperty("CustomNumber9")]
        public int CustomNumber9 { get; set; }

        [JsonProperty("CustomText1")]
        public string CustomText1 { get; set; }

        [JsonProperty("CustomText2")]
        public string CustomText2 { get; set; }

        [JsonProperty("CustomText3")]
        public string CustomText3 { get; set; }

        [JsonProperty("CustomText4")]
        public string CustomText4 { get; set; }

        [JsonProperty("CustomText5")]
        public string CustomText5 { get; set; }

        [JsonProperty("CustomText6")]
        public string CustomText6 { get; set; }

        [JsonProperty("CustomText7")]
        public string CustomText7 { get; set; }

        [JsonProperty("CustomText8")]
        public string CustomText8 { get; set; }

        [JsonProperty("CustomText9")]
        public string CustomText9 { get; set; }

        [JsonProperty("CustomText10")]
        public string CustomText10 { get; set; }

        [JsonProperty("CustomText11")]
        public string CustomText11 { get; set; }

        [JsonProperty("CustomText12")]
        public string CustomText12 { get; set; }

        [JsonProperty("Class")]
        public string Class { get; set; }

        [JsonProperty("SubClass1")]
        public string SubClass1 { get; set; }

        [JsonProperty("Subclass2")]
        public string Subclass2 { get; set; }
    }
}
