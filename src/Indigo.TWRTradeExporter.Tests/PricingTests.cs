using System;
using Indigo.TWRTradeDataExporter.PricingEngine.Service;
using Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

// Note - The tests in this class are heavily dependent on the
// values in the FakePricingData.cs class. If you change data
// in that class, you may have to update tests here.

namespace Indigo.TWRTradeExporter.Tests
{
    [TestClass]
    public class PricingTests
    {
        static IPricingDataProvider _provider;
        static IPricingEngine _engine;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            _provider = new FakePricingData();
            _engine = new TWRPricingEngine(_provider);
        }

        [TestMethod]
        public void BasicTest()
        {
            string upc = "9781234567890";
            string mcat = "P10101";
            decimal price = 1.00m;

            var results = _engine.CalculatePrices(upc, mcat, price).ToArray();

            Assert.IsTrue(results.Length == 6);

            var emp000 = results.Where(x => x.TWRLevel == "EMP").First().Price;
            var corp05 = results.Where(x => x.TWRLevel == "CORPORATE5").First().Price;
            var corp10 = results.Where(x => x.TWRLevel == "CORPORATE10").First().Price;
            var corp15 = results.Where(x => x.TWRLevel == "CORPORATE15").First().Price;
            var corp20 = results.Where(x => x.TWRLevel == "CORPORATE20").First().Price;
            var retail = results.Where(x => x.TWRLevel == "RETAIL").First().Price;

            Assert.IsTrue(retail == price);
            Assert.IsTrue(corp05 == price * 0.95m);
            Assert.IsTrue(corp10 == price * 0.9m);
            Assert.IsTrue(corp15 == price * 0.85m);
            Assert.IsTrue(corp20 == price * 0.80m);
            Assert.IsTrue(emp000 == price * 0.7m);
        }

        [TestMethod]
        public void EmployeeTier1Included()
        {
            string upc = "9781234567890";
            string mcat = "P10101";
            decimal price = 1.00m;

            var results = _engine.CalculatePrices(upc, mcat, price).ToArray();

            var emp000 = results.Where(x => x.TWRLevel == "EMP").FirstOrDefault()?.Price;

            Assert.IsNotNull(emp000);
            Assert.IsTrue(emp000 == price * 0.7m);
        }

        [TestMethod]
        public void EmployeeTier2Included()
        {
            string upc = "9781234567890";
            string mcat = "M10101";
            decimal price = 1.00m;

            var results = _engine.CalculatePrices(upc, mcat, price).ToArray();

            var emp000 = results.Where(x => x.TWRLevel == "EMP").FirstOrDefault()?.Price;

            Assert.IsNotNull(emp000);
            Assert.IsTrue(emp000 == price * 0.9m);
        }

        [TestMethod]
        public void EmployeeTier3Included()
        {
            string upc = "9781234567890";
            string mcat = "P10301";
            decimal price = 1.00m;

            var results = _engine.CalculatePrices(upc, mcat, price).ToArray();

            var emp000 = results.Where(x => x.TWRLevel == "EMP").FirstOrDefault()?.Price;

            Assert.IsNotNull(emp000);
            Assert.IsTrue(emp000 == price * 0.95m);
        }

        [TestMethod]
        public void EmployeeTier1Excluded()
        {
            string upc = "28399150151";
            string mcat = "P10101";
            decimal price = 1.00m;

            var results = _engine.CalculatePrices(upc, mcat, price).ToArray();

            var emp000 = results.Where(x => x.TWRLevel == "EMP").FirstOrDefault()?.Price;

            Assert.IsNotNull(emp000);
            Assert.IsTrue(emp000.Value == price);
        }

        [TestMethod]
        public void EmployeeMixTier()
        {
            string upc = "28399150151";
            string mcat = "P10301";
            decimal price = 1.00m;

            var results = _engine.CalculatePrices(upc, mcat, price).ToArray();

            var emp000 = results.Where(x => x.TWRLevel == "EMP").FirstOrDefault()?.Price;

            Assert.IsNotNull(emp000);
            Assert.IsTrue(emp000 == price * 0.95m);
        }

        [TestMethod]
        public void SpeedTest()
        {
            // About 7k products/second on my laptop virtual machine.

            string upc = "28399150151";
            string mcat = "P10301";
            decimal price = 1.00m;

            for (int i = 0; i <= 15000; i++)
            {
                var results = _engine.CalculatePrices(upc, mcat, price).ToArray();
            }
        }
    }
}
