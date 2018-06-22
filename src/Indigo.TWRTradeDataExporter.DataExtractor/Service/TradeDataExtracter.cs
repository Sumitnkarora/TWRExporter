using Indigo.TWRTradeDataExporter.DataExtractor.Models;
using Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract;
using Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.DataExtractor.Service
{
    public class TradeDataExtracter : IDataExtracter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(TradeDataExtracter));

        private readonly ITradeDataProvider _dao;
        private readonly IPricingEngine _pricingEngine;

        public TradeDataExtracter(ITradeDataProvider dao, IPricingEngine pricingEngine)
        {
            this._dao = dao ?? throw new ArgumentNullException("dao");
            this._pricingEngine = pricingEngine ?? throw new ArgumentNullException("pricingEngine");
        }

        public IEnumerable<TradeData> GetTradeDataForExport()
        {
            if (ConfigurationHelper.GetAppConfig("EnableAutoApproval", false, "true").Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                log.Info("Running Autoapproval...");
                _dao.ExecuteTradeAutoApproval();
            }

            int recordsRetrieved = 0;
            int errorRecords = 0;

            try
            {
                log.Info("Retrieving trade data from database...");

                foreach (var data in _dao.RetrieveExportData())
                {
                    recordsRetrieved++;

                    TradeData formattedData = null;

                    try
                    {
                        string logMessage = $"Processing record [{recordsRetrieved}]: {formattedData?.StyleNo}";
                        log.DebugFormat(logMessage);
                        formattedData = this.CreateTradeDataElement(data);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Cannot process record [{recordsRetrieved}]: {formattedData?.StyleNo}";
                        log.Error(errorMessage, ex);
                        errorRecords++;
                    }

                    if (formattedData != null)
                    {
                        yield return this.CreateTradeDataElement(data);
                    }
                }
            }
            finally
            {
                log.Info($"Retrieved Records : {recordsRetrieved}, records with errors : {errorRecords}");
                NewRelic.Api.Agent.NewRelic.AddCustomParameter("NumberOfItemsRetrievedFromDB", recordsRetrieved);
                NewRelic.Api.Agent.NewRelic.AddCustomParameter("NumberOfItemsErroredOut", errorRecords);
                log.Info("Done retrieving trade data from database...");
            }

            yield break;
        }

        public string SerializeTradeData(TradeData data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public void UpdateExportedItemStatus(string isbn13)
        {
            this._dao.UpdateExportedItemStatus(isbn13);
        }

        private TradeData CreateTradeDataElement(TradeExportData data)
        {
            if (string.IsNullOrWhiteSpace(data.StyleNo))
            {
                throw new Exception("data has empty StyleNo");
            }

            TradeData output = new TradeData();
            Item item = new Item();
            Upc upc = new Upc();

            var pricing = _pricingEngine.CalculatePrices(data.StyleNo, data.SubClass1 ?? string.Empty, data.BasePrice);

            output.StyleNo = data.StyleNo;
            output.Description1 = data.Description1 ?? string.Empty;
            output.Description2 = data.Description2 ?? string.Empty;
            output.Description4 = data.Description4 ?? string.Empty;
            output.PrimaryVendor = data.PrimaryVendor ?? string.Empty;
            output.SeasonName = data.SeasonName ?? string.Empty;
            output.TaxGroupCode = data.TaxGroupCode ?? string.Empty;
            output.TaxCategory = data.TaxCategory ?? string.Empty;
            output.BrandName = data.BrandName ?? string.Empty;
            output.Replenishment = true;
            output.CountryOfOrigin = data.CountryOfOrigin ?? string.Empty;
            output.CustomLongText1 = data.CustomLongText1 ?? string.Empty;
            output.CustomLongText2 = data.CustomLongText2 ?? string.Empty;
            output.CustomLongText3 = data.CustomLongText3 ?? string.Empty;
            output.CustomLongText4 = data.CustomLongText4 ?? string.Empty;
            output.CustomLongText5 = data.CustomLongText5 ?? string.Empty;
            output.CustomLongText6 = data.CustomLongText6 ?? string.Empty;
            output.CustomLongText7 = string.Empty;
            output.CustomLongText8 = string.Empty;
            output.CustomLongText9 = string.Empty;
            output.CustomLongText10 = string.Empty;
            output.CustomLongText11 = string.Empty;
            output.CustomLongText12 = string.Empty;
            output.Manufacturer = string.Empty;
            output.Department = string.IsNullOrEmpty(data.SubClass1) ? string.Empty : data.SubClass1.Substring(0, 1);
            output.OrderCost = data.OrderCost;

            item.WeeksOfSupply = 6;
            item.Eid = data.EID;
            item.Clu = data.CLU;

            upc.Value = data.UPCValue;
            upc.IsDefault = true;

            item.UPCs.Add(upc);

            item.Attribute1 = data.Attribute1 ?? string.Empty;
            item.Attribute2 = data.Attribute2 ?? string.Empty;

            if (data.Height.HasValue)
            {
                item.Height = data.Height.Value;
            }

            if (data.Weight.HasValue)
            {
                item.Weight = data.Weight.Value;
            }

            if (data.Width.HasValue)
            {
                item.Width = data.Width.Value;
            }

            if (data.Length.HasValue)
            {
                item.Length = data.Length.Value;
            }

            item.BasePrice = data.BasePrice;

            foreach (var outputPrice in _pricingEngine.CalculatePrices(data.StyleNo, data.SubClass1, data.BasePrice))
            {
                Price price = new Price
                {
                    PriceLevel = outputPrice.TWRLevel,
                    PricePrice = outputPrice.Price
                };

                item.Prices.Add(price);
            }

            if (data.ReleaseDate.HasValue)
            {
                item.ReleaseDate = data.ReleaseDate.Value;
            }

            item.NotTrackOh = false;
            item.TradeDiscount = true;
            item.MemberDiscount = true;
            item.Inactive = false;
            item.RequireDiscountAuthorizationCode = true;
            item.AcceptToken = true;
            item.AutoPromptToPayWithTokens = true;
            item.EligibleForLoyaltyRewards2 = true;
            item.LoyaltyRewards2Ratio = 1;

            if (data.ItemCost.HasValue)
            {
                item.OrderCost = data.ItemCost.Value;
            }

            if (data.CustomDate1.HasValue)
            {
                item.CustomDate1 = data.CustomDate1.Value;
            }

            item.CustomFlag3 = false;
            item.CustomFlag4 = false;
            item.CustomFlag5 = false;
            item.CustomFlag6 = false;
            item.CustomFlag7 = false;
            item.CustomNumber3 = 0;
            item.CustomNumber4 = 0;
            item.CustomNumber5 = 0;
            item.CustomNumber6 = 0;
            item.CustomNumber7 = 0;
            item.CustomNumber8 = 0;
            item.CustomNumber9 = 0;

            item.CustomText1 = data.CustomText1 ?? string.Empty;
            item.CustomText2 = data.CustomText2 ?? string.Empty;
            item.CustomText3 = string.Empty;
            item.CustomText4 = string.Empty;
            item.CustomText5 = data.CustomText5 ?? string.Empty;
            item.CustomText6 = string.Empty;
            item.CustomText7 = string.Empty;
            item.CustomText8 = string.Empty;
            item.CustomText9 = data.CustomText9 ?? string.Empty;
            item.CustomText10 = string.Empty;
            item.CustomText11 = string.Empty;
            item.CustomText12 = string.Empty;

            item.SubClass1 = data.SubClass1 ?? string.Empty;
            item.Subclass2 = data.SubClass2 ?? string.Empty;

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

                    item.Class = elementClass;
                }
            }

            output.Items.Add(item);

            return output;
        }
    }
}
