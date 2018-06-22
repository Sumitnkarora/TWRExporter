using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeExporter.StateTracking.Models
{
    public class AppExecutionDetails
    {
        public int NumberOfItemsUploadedToDatabase { get; set; }
        public int NumberOfItemsErroredOut { get; set; }
        public AppExecutionDetails()
        {
            NumberOfItemsErroredOut = 0;
            NumberOfItemsUploadedToDatabase = 0;
        }
    }
}
