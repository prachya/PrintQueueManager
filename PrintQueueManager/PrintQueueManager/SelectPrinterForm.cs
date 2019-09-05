using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintQueueManager
{
    public partial class SelectPrinterForm : Form
    {
        public SelectPrinterForm()
        {
            InitializeComponent();

            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            cmbPrinter.Items.AddRange(printers.OfType<string>().ToArray());
            cmbPrinter.SelectedIndex = 0;

        }
    }
}
