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
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter
{
    public partial class ExporterService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExporterService));

        private bool dataImportInProgress = false;
        private System.Timers.Timer loopTimer;
        private System.Timers.Timer heartbeatTimer;
        private const int MaxWaitForImport = 60; //seconds
        public ExporterService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            AppDomain.CurrentDomain.UnhandledException += ExceptionalHandler;
            loopTimer = new System.Timers.Timer();
            loopTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["LoopTimerInterval"]) * 1000;
            loopTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnPollQueue);
            loopTimer.Start();
            heartbeatTimer = new System.Timers.Timer();
            heartbeatTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatTimerInterval"]) * 1000;
            heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnHeartbeat);
            heartbeatTimer.Start();
            log.Info("Trade Data Exporter service started.");
        }

        protected override void OnStop()
        {
        }

        private void OnPollQueue(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.Info("Poll queue timer tick.");
            if (!dataImportInProgress)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DoDataExport));
            }
        }

        private void DoDataExport(object state)
        {
            try
            {
                dataImportInProgress = true;
                // Send Messages
                var exportServiceBuilder = new ContainerBuilder();
                exportServiceBuilder.RegisterType<SQLPricingData>().As<IPricingDataProvider>();
                exportServiceBuilder.RegisterType<SQLTradeData>().As<ITradeDataProvider>();
                exportServiceBuilder.RegisterType<TWRPricingEngine>().As<IPricingEngine>();
                exportServiceBuilder.RegisterType<TradeDataExtracter>().As<IDataExtracter>();
                exportServiceBuilder.RegisterType<ExportService>().As<IExportService>();
                var exportServiceContainer = exportServiceBuilder.Build();
                IExportService exportService = exportServiceContainer.Resolve<IExportService>();
                exportService.ExecuteExport();

                dataImportInProgress = false;
            }
            catch(Exception ex)
            {
                StateTracker.ReportException(ex);
            }
        }

        private void OnHeartbeat(object sender, System.Timers.ElapsedEventArgs e)
        {
            log.Info("Heartbeat timer tick.");
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoHeartbeat));
        }

        private void DoHeartbeat(object state)
        {
            try
            {
                StateTracker.OutputAppState(ConfigurationManager.AppSettings["HeartbeatPath"],
                ConfigurationManager.AppSettings["HeartbeatFilenamePattern"],
                Convert.ToInt32(ConfigurationManager.AppSettings["MaxNumHeartbeatFiles"]));
            }
            catch (Exception ex)
            {
                StateTracker.ReportException(ex);
            }
        }

        private void ExceptionalHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                StateTracker.ReportException(exception);
            }
        }
    }
}
