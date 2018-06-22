using Autofac;
using Indigo.TWRTradeDataExporter.DataExtractor.Service;
using Indigo.TWRTradeDataExporter.DataExtractor.Service.Contract;
using Indigo.TWRTradeDataExporter.EnqueueToServiceBus.Service;
using Indigo.TWRTradeDataExporter.EnqueueToServiceBus.Service.Contract;
using Indigo.TWRTradeDataExporter.PricingEngine.Service;
using Indigo.TWRTradeDataExporter.PricingEngine.Service.Contract;
using Indigo.TWRTradeExporter.StateTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeExporter.CLI
{
    public class Program
    {
        private const int ERROR_RESULT = 255;
        private const int SUCCESS_RESULT = 0;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        static int Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("Starting Application");

#if DEBUG
            log.Warn("This is a DEBUG BUILD");
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
#else
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif


            int result = SUCCESS_RESULT;
            try
            {
                var exportServiceBuilder = new ContainerBuilder();
                exportServiceBuilder.RegisterType<SQLPricingData>().As<IPricingDataProvider>();
                exportServiceBuilder.RegisterType<SQLTradeData>().As<ITradeDataProvider>();
                exportServiceBuilder.RegisterType<TWRPricingEngine>().As<IPricingEngine>();
                exportServiceBuilder.RegisterType<TradeDataExtracter>().As<IDataExtracter>();
                exportServiceBuilder.RegisterType<ExportService>().As<IExportService>();
                var exportServiceContainer = exportServiceBuilder.Build();
                IExportService exportService = exportServiceContainer.Resolve<IExportService>();
                exportService.ExecuteExport();
            }
            catch(Exception ex)
            {
                StateTracker.ReportException(ex);
            }
            finally
            {
                log.Info("Exiting Application");
            }
            return result;
        }

        /// <summary>
        /// Unhandled Exception Handler
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Fatal("Unhandled Exception --", (Exception)e.ExceptionObject);
            Environment.Exit(ERROR_RESULT);
        }

        /// <summary>
        /// First Chance Exception Handler - Debug Mode Only
        /// </summary>
        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            log.Error("First Chance Exception Handler -- ", e.Exception);
        }
    }
}
