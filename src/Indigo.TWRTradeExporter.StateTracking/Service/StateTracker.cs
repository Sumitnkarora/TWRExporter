using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indigo.TWRTradeExporter.StateTracking.Models;
using Newtonsoft.Json;
using StackExchange.Exceptional;

namespace Indigo.TWRTradeExporter.StateTracking
{
    public class StateTracker
    {
        /// <summary>
        /// Log4net - Logger
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(StateTracker));
        public static AppState State { get; set; }
        static StateTracker()
        {
            State = new AppState();
        }
        public static void ReportException(Exception x)
        {
            State.Status = "Error";
            State.ErrorDetails.Add(new AppError { ErrorType = x.GetType().Name, Message = x.Message, StackTrace = x.StackTrace });
            State.HasErrorsSinceLastGeneration = true;
            log.Error(x);
            ErrorStore.LogExceptionWithoutContext(x);
            NewRelic.Api.Agent.NewRelic.NoticeError(x);
        }
        public static void OutputAppState(string heartbeatFilePath, string heartbeatFilenamePattern, int maxNumHeartbeatFiles)
        {
            log.Info("Writing heartbeat file...");
            State.GenerationTime = DateTime.Now;
            string outputJson = JsonConvert.SerializeObject(State, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            string heartbeatFileName = string.Format(heartbeatFilenamePattern, State.GenerationTime.ToString("yyyyMMddHHmm"));
            if (heartbeatFilePath == "." || string.IsNullOrEmpty(heartbeatFilePath))
            {
                heartbeatFilePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            CleanupHeartbeatFiles(heartbeatFilePath, heartbeatFilenamePattern, maxNumHeartbeatFiles);
            heartbeatFilePath = Path.Combine(heartbeatFilePath, heartbeatFileName);
            File.WriteAllText(heartbeatFilePath, outputJson);
            State.HasErrorsSinceLastGeneration = false;
            log.Info("Done writing heartbeat file.");
        }

        private static void CleanupHeartbeatFiles(string heartbeatFilePath, string heartbeatFilenamePattern, int maxNumHeartbeatFiles)
        {
            FileInfo[] heartbeatFiles = new DirectoryInfo(heartbeatFilePath).GetFiles(string.Format(heartbeatFilenamePattern, "*"));
            int count = heartbeatFiles.Length;
            foreach (FileInfo fi in heartbeatFiles.OrderBy(fi => fi.CreationTime))
            {
                if (count >= maxNumHeartbeatFiles)
                {
                    File.Delete(fi.FullName);
                    count--;
                }
            }
        }
    }
}
