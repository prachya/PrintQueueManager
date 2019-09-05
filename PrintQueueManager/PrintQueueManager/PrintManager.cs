using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintQueueManager.Print
{
    public class PrintManager
    {
        public List<PrintController> PrinterControllers { get; set; } = new List<PrintController>();

        public PrintController CreatePrintController()
        {

            var controller = new PrintController();
            PrinterControllers.Add(controller);

            return controller;
        }
    }
}
