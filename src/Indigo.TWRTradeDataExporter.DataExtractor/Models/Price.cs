using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Models
{
    public partial class Price
    {
        [JsonProperty("PriceLevel")]
        public string PriceLevel { get; set; }

        [JsonProperty("Price")]
        public decimal PricePrice { get; set; }
    }
}
