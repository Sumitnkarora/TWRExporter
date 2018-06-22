using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeExporter.StateTracking.Models
{
    public class AppState
    {
        public DateTime GenerationTime { get; set; }
        public bool HasErrorsSinceLastGeneration { get; set; }
        public string Status { get; set; }
        public AppExecutionDetails ExecutionDetails { get; set; }
        public List<AppError> ErrorDetails { get; set; }
        public AppState()
        {
            Status = "Started";
            ErrorDetails = new List<AppError>();
            ExecutionDetails = new AppExecutionDetails();
        }
    }
}
