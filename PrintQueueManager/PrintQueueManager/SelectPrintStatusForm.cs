using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintQueueManager
{
    public partial class SelectPrintStatusForm : Form
    {
        public SelectPrintStatusForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
    }
}
