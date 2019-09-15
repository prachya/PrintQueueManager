using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Printing;
namespace PrintQueueManager.Data
{
    public class PrinterSpeedData
    {
        public string PrinterName { get; set; }
        public string PrinterID { get; set; }
        public int? PresetID { get; set; }
        public double PagesPerMinute { get; set; }
        public PrinterSettings PrinterInfo { get; set; }
        public int Divider { get; set; }
    }
}
