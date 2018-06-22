using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Models
{
    public partial class Upc
    {
        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("IsDefault")]
        public bool IsDefault { get; set; }
    }
}
