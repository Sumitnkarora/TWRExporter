using System;
using Indigo.TWRTradeDataExporter.PricingEngine.Service;
using Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract;
using Indigo.TWRTradeDataExporter.DataExtractor.Service;
using Indigo.TWRTradeDataExporter.DataExtractor.Models;
using System.Collections.Generic;

namespace Indigo.TWRTradeExporter.Tests
{
    [TestClass]
    public class TradeDataTests
    {
        static ITradeDataProvider _trade;
        static IPricingDataProvider _pricing;
        static IDataExtracter _extracter;
        static IPricingEngine _engine;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _trade = new FakeTradeData();
            _pricing = new FakePricingData();
            _engine = new TWRPricingEngine(_pricing);
            _extracter = new TradeDataExtracter(_trade, _engine);
        }

        [TestMethod]
        public void NormalDataConversionTest()
        {
            var output = _extracter.GetTradeDataForExport().ToArray();

            Assert.AreEqual(FakeTradeData.Items.Length, output.Length);

            var firstInput = FakeTradeData.Items.Where(x => x.StyleNo == "9780029013007").Single();
            var firstOutput = output.Where(x => x.StyleNo == "9780029013007").Single();

            this.CompareItems(firstInput, firstOutput);
        }

        [TestMethod]
        public void EmptyDataConversionTest()
        {
            var output = _extracter.GetTradeDataForExport().ToArray();

            Assert.AreEqual(FakeTradeData.Items.Length, output.Length);

            var firstInput = FakeTradeData.Items.Where(x => x.StyleNo == "StyleNo").Single();
            var firstOutput = output.Where(x => x.StyleNo == "StyleNo").Single();

            this.CompareItems(firstInput, firstOutput);
        }

        private void CompareItems(TradeExportData input, TradeData output)
        {
            Assert.AreEqual(input.StyleNo, output.StyleNo);
            Assert.AreEqual(input.Description1 ?? string.Empty, output.Description1);
            Assert.AreEqual(input.Description2 ?? string.Empty, output.Description2);
            Assert.AreEqual(input.Description4 ?? string.Empty, output.Description4);
            Assert.AreEqual(input.PrimaryVendor ?? string.Empty, output.PrimaryVendor);
            Assert.AreEqual(input.SeasonName ?? string.Empty, output.SeasonName);
            Assert.AreEqual(input.TaxGroupCode ?? string.Empty, output.TaxGroupCode);
            Assert.AreEqual(input.TaxCategory ?? string.Empty, output.TaxCategory);
            Assert.AreEqual(input.BrandName ?? string.Empty, output.BrandName);
            Assert.AreEqual(true, output.Replenishment);
            Assert.AreEqual(input.CountryOfOrigin ?? string.Empty, output.CountryOfOrigin);
            Assert.AreEqual(input.CustomLongText1 ?? string.Empty, output.CustomLongText1);
            Assert.AreEqual(input.CustomLongText2 ?? string.Empty, output.CustomLongText2);
            Assert.AreEqual(input.CustomLongText3 ?? string.Empty, output.CustomLongText3);
            Assert.AreEqual(input.CustomLongText4 ?? string.Empty, output.CustomLongText4);
            Assert.AreEqual(input.CustomLongText5 ?? string.Empty, output.CustomLongText5);
            Assert.AreEqual(input.CustomLongText6 ?? string.Empty, output.CustomLongText6);
            Assert.AreEqual(string.Empty, output.CustomLongText7);
            Assert.AreEqual(string.Empty, output.CustomLongText8);
            Assert.AreEqual(string.Empty, output.CustomLongText9);
            Assert.AreEqual(string.Empty, output.CustomLongText10);
            Assert.AreEqual(string.Empty, output.CustomLongText11);
            Assert.AreEqual(string.Empty, output.CustomLongText12);
            Assert.AreEqual(string.Empty, output.Manufacturer);
            Assert.AreEqual(input.SubClass1.Substring(0, 1), output.Department);
            Assert.AreEqual(input.OrderCost, output.OrderCost);

            var item = output.Items.Single();

            Assert.AreEqual(6, item.WeeksOfSupply);
            Assert.AreEqual(input.EID, item.Eid);
            Assert.AreEqual(input.CLU, item.Clu);

            var upc = item.UPCs.Single();

            Assert.AreEqual(input.UPCValue, upc.Value);
            Assert.AreEqual(true, upc.IsDefault);

            Assert.AreEqual(input.Attribute1 ?? string.Empty, item.Attribute1);
            Assert.AreEqual(input.Attribute2 ?? string.Empty, item.Attribute2);

            Assert.AreEqual(input.Height ?? default(decimal), item.Height);
            Assert.AreEqual(input.Weight ?? default(decimal), item.Weight);
            Assert.AreEqual(input.Width ?? default(decimal), item.Width);
            Assert.AreEqual(input.Length ?? default(decimal), item.Length);

            Assert.AreEqual(input.ReleaseDate ?? null, item.ReleaseDate);

            Assert.AreEqual(false, item.NotTrackOh);
            Assert.AreEqual(true, item.TradeDiscount);
            Assert.AreEqual(true, item.MemberDiscount);
            Assert.AreEqual(false, item.Inactive);
            Assert.AreEqual(true, item.RequireDiscountAuthorizationCode);
            Assert.AreEqual(true, item.AcceptToken);
            Assert.AreEqual(true, item.AutoPromptToPayWithTokens);
            Assert.AreEqual(true, item.EligibleForLoyaltyRewards2);
            Assert.AreEqual(1, item.LoyaltyRewards2Ratio);

            Assert.AreEqual(input.ItemCost ?? default(decimal), item.OrderCost);
            Assert.AreEqual(input.CustomDate1 ?? null, item.CustomDate1);

            Assert.AreEqual(false, item.CustomFlag3);
            Assert.AreEqual(false, item.CustomFlag4);
            Assert.AreEqual(false, item.CustomFlag5);
            Assert.AreEqual(false, item.CustomFlag6);
            Assert.AreEqual(false, item.CustomFlag7);

            Assert.AreEqual(0, item.CustomNumber3);
            Assert.AreEqual(0, item.CustomNumber4);
            Assert.AreEqual(0, item.CustomNumber5);
            Assert.AreEqual(0, item.CustomNumber6);
            Assert.AreEqual(0, item.CustomNumber7);
            Assert.AreEqual(0, item.CustomNumber8);
            Assert.AreEqual(0, item.CustomNumber9);

            Assert.AreEqual(input.CustomText1 ?? string.Empty, item.CustomText1);
            Assert.AreEqual(input.CustomText2 ?? string.Empty, item.CustomText2);
            Assert.AreEqual(string.Empty, item.CustomText3);
            Assert.AreEqual(string.Empty, item.CustomText4);
            Assert.AreEqual(input.CustomText5 ?? string.Empty, item.CustomText5);
            Assert.AreEqual(string.Empty, item.CustomText6);
            Assert.AreEqual(string.Empty, item.CustomText7);
            Assert.AreEqual(string.Empty, item.CustomText8);
            Assert.AreEqual(input.CustomText9 ?? string.Empty, item.CustomText9);
            Assert.AreEqual(string.Empty, item.CustomText10);
            Assert.AreEqual(string.Empty, item.CustomText11);
            Assert.AreEqual(string.Empty, item.CustomText12);

            Assert.AreEqual(input.SubClass1 ?? string.Empty, item.SubClass1);
            Assert.AreEqual(input.SubClass2 ?? string.Empty, item.Subclass2);

            if (!string.IsNullOrWhiteSpace(item.SubClass1))
            {
                if (item.SubClass1.Length == 6)
                {
                    string elementClass = item.SubClass1.Substring(3, 3);

                    switch (item.SubClass1.Substring(0, 3).ToLowerInvariant())
                    {
                        case "p10":
                            elementClass = "1" + elementClass;
                            break;
                        case "m10":
                            elementClass = "2" + elementClass;
                            break;
                        case "g10":
                            elementClass = "3" + elementClass;
                            break;
                        case "f10":
                            elementClass = "6" + elementClass;
                            break;
                    }

                    Assert.AreEqual(elementClass, item.Class);
                }
            }

            Assert.AreEqual(input.BasePrice, item.BasePrice);
            this.ComparePricing(item.Prices, upc.Value, item.SubClass1, input.BasePrice);
        }

        private void ComparePricing(List<Price> prices, string upc, string mcat, decimal price)
        {
            var priceResults = _engine.CalculatePrices(upc, mcat, price);

            foreach (var pricing in priceResults)
            {
                var tp = prices.Where(x => x.PriceLevel == pricing.TWRLevel).Single();
                Assert.AreEqual(pricing.Price, tp.PricePrice);
            }
        }
    }
}
