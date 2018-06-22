using Indigo.TWRTradeDataExporter.DataExtractor.Models;
using Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeExporter.Tests
{
    public class FakeTradeData : ITradeDataProvider
    {
        public static TradeExportData[] Items = new TradeExportData[]
        {
            new TradeExportData()
            {
                StyleNo = "9780029013007",
                Description1 = "Symlog",
                Description2 = "Symlog",
                Description4 = "Symlog",
                AttributeSet1 = null,
                PrimaryVendor = "723125",
                OrderCost = 40.0000000000000m,
                SeasonName = "",
                TaxGroupCode = "",
                TaxCategory = "",
                BrandName = "Free Pr",
                CountryOfOrigin = "",
                CustomLongText1 = null,
                CustomLongText2 = "Symlog",
                CustomLongText3 = null,
                CustomLongText4 = "",
                CustomLongText5 = "",
                CustomLongText6 = "~Author^Bales, Robert Freed",
                EID = "9780029013007",
                CLU = "9780029013007",
                PLU = "9780029013007",
                UPCValue = "9780029013007",
                Attribute1 = "",
                Attribute2 = "",
                Height = 1.50000m,
                Length = 9.75000m,
                Weight = 2.10000m,
                Width = 6.50000m,
                ItemCost = 40.0000000000000m,
                BasePrice = 50.00m,
                ReleaseDate = DateTime.Parse("1979-01-01 00:00:00.000"),
                Name = "Symlog",
                LookupName = "Symlog",
                CustomDate1 = null,
                CustomText1 = "Bales, Robert Freed",
                CustomText2 = "A01",
                CustomText5 = @"[
  ""eng""
]",
                CustomText9 = null,
                SubClass1 = "P10145",
                SubClass2 = "NFAUD01A"
            },
            new TradeExportData()
            {
                StyleNo = "StyleNo",
                Description1 = null,
                Description2 = null,
                Description4 = null,
                AttributeSet1 = null,
                PrimaryVendor = null,
                OrderCost = 0,
                SeasonName = null,
                TaxGroupCode = null,
                TaxCategory = null,
                BrandName = null,
                CountryOfOrigin = null,
                CustomLongText1 = null,
                CustomLongText2 = null,
                CustomLongText3 = null,
                CustomLongText4 = null,
                CustomLongText5 = null,
                CustomLongText6 = null,
                EID = "EID",
                CLU = "CLU",
                PLU = "PLU",
                UPCValue = "UPCValue",
                Attribute1 = null,
                Attribute2 = null,
                Height = null,
                Length = null,
                Weight = null,
                Width = null,
                ItemCost = 0,
                BasePrice = 0,
                ReleaseDate = null,
                Name = null,
                LookupName = null,
                CustomDate1 = null,
                CustomText1 = null,
                CustomText2 = null,
                CustomText5 = null,
                CustomText9 = null,
                SubClass1 = "P10101",
                SubClass2 = null
            }
        };


        public void ExecuteTradeAutoApproval()
        {
            // don't do anything.
        }

        public void ExecuteTradeAutoApproval(string sapVendorId)
        {
            // don't do anything
        }

        public IEnumerable<TradeExportData> RetrieveExportData()
        {
            return FakeTradeData.Items;
        }

        public void UpdateExportedItemStatus(string isbn13)
        {
            // don't do anything
        }
    }
}
