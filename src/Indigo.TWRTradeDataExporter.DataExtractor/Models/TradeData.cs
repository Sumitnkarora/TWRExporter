using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Models
{
    public partial class TradeData
    {
        public TradeData()
        {
            Items = new List<Item>();
        }
        [JsonProperty("StyleNo")]
        public string StyleNo { get; set; }

        [JsonProperty("Description1")]
        public string Description1 { get; set; }

        [JsonProperty("Description2")]
        public string Description2 { get; set; }

        [JsonProperty("Description4")]
        public string Description4 { get; set; }

        [JsonProperty("AttributeSet1")]
        public string AttributeSet1 { get; set; }

        [JsonProperty("PrimaryVendor")]
        public string PrimaryVendor { get; set; }

        [JsonProperty("OrderCost")]
        public decimal OrderCost { get; set; }

        [JsonProperty("SeasonName")]
        public string SeasonName { get; set; }

        [JsonProperty("TaxGroupCode")]
        public string TaxGroupCode { get; set; }

        [JsonProperty("TaxCategory")]
        public string TaxCategory { get; set; }

        [JsonProperty("BrandName")]
        public string BrandName { get; set; }

        [JsonProperty("Replenishment")]
        public bool Replenishment { get; set; }

        [JsonProperty("CountryOfOrigin")]
        public string CountryOfOrigin { get; set; }

        [JsonProperty("CustomLongText1")]
        public string CustomLongText1 { get; set; }

        [JsonProperty("CustomLongText2")]
        public string CustomLongText2 { get; set; }

        [JsonProperty("CustomLongText3")]
        public string CustomLongText3 { get; set; }

        [JsonProperty("CustomLongText4")]
        public string CustomLongText4 { get; set; }

        [JsonProperty("CustomLongText5")]
        public string CustomLongText5 { get; set; }

        [JsonProperty("CustomLongText6")]
        public string CustomLongText6 { get; set; }

        [JsonProperty("CustomLongText7")]
        public string CustomLongText7 { get; set; }

        [JsonProperty("CustomLongText8")]
        public string CustomLongText8 { get; set; }

        [JsonProperty("CustomLongText9")]
        public string CustomLongText9 { get; set; }

        [JsonProperty("CustomLongText10")]
        public string CustomLongText10 { get; set; }

        [JsonProperty("CustomLongText11")]
        public string CustomLongText11 { get; set; }

        [JsonProperty("CustomLongText12")]
        public string CustomLongText12 { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [JsonProperty("Items")]
        public List<Item> Items { get; set; }
    }
}
