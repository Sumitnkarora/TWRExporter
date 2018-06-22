using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indigo.TWRTradeDataExporter.EnqueueToServiceBus.Service.Contract
{
    public interface IExportService
    {
        void ExecuteExport();
    }
}
