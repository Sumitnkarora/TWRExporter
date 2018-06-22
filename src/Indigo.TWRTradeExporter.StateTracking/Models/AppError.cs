using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeExporter.StateTracking.Models
{
    public class AppError
    {
        public string ErrorType { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
    }
}
