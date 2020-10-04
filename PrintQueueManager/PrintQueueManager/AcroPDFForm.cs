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
    public partial class AcroPDFForm : Form
    {
        static public AcroPDFForm f = null;
        public AcroPDFForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will print the PDF to the default printer.");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                axAcroPDF1.src = openFileDialog1.FileName;
                //System.Threading.Thread.Sleep(1000);
                //MessageBox.Show("loaded");                
                
                //axAcroPDF1.printAll();
                axAcroPDF1.Print();

                MessageBox.Show("printed");
            }
        }

        static public AcroPDFForm GetForm()
        {
            if(f == null)
            {
                f = new AcroPDFForm();
                f.Show();
            }
            return f;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        public void PrintFile(string filePath)
        {
            axAcroPDF1.src = filePath;
            axAcroPDF1.printAll();
            
        }
    }
}
