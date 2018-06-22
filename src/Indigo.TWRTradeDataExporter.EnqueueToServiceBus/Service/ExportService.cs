using Indigo.TWRTradeDataExporter.DataExtractor.Service;
using Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract;
using Indigo.TWRTradeDataExporter.EnqueueToServiceBus.Service.Contract;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.EnqueueToServiceBus.Service
{
    public class ExportService : IExportService
    {
        public IDataExtracter _dataExtractor { get; set; }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExportService));
        private static IQueueClient _queueClient;
        private string ServiceBusConnectionString = ConfigurationManager.AppSettings["TWR.ServiceBus"];
        private string QueueName = ConfigurationManager.AppSettings["QueueName"];

        public ExportService(IDataExtracter dataExtracter)
        {
            _dataExtractor = dataExtracter;
        }

        public void ExecuteExport()
        {
            int messageCount = 0;
            try
            {
                _queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
                if (_dataExtractor != null)
                {
                    string jsonMessage = string.Empty;
                    dynamic jsonTradeData = null;
                    log.Info("Sending trade data to queue...");
                    foreach (var message in _dataExtractor.GetTradeDataForExport())
                    {
                        jsonMessage=_dataExtractor.SerializeTradeData(message);
                        messageCount++;
                        SendMessageAsync(jsonMessage).GetAwaiter().GetResult();
                        jsonTradeData = JObject.Parse(jsonMessage);
                        string isbn13 = jsonTradeData.StyleNo.ToString();
                        _dataExtractor.UpdateExportedItemStatus(isbn13);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An exception occured while pushing messages to service bus.";
                log.Error(errorMessage, ex);
            }
            finally
            {
                log.Info($"Sent {messageCount} messages to queue.");
                log.Info("Done sending trade data to queue...");
            }
        }

        protected async System.Threading.Tasks.Task SendMessageAsync(string message)
        {
            var jsonMessage = new Message(Encoding.UTF8.GetBytes(message));
            await _queueClient.SendAsync(jsonMessage);
        }
    }
}
