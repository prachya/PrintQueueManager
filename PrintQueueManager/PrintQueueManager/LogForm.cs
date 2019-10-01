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
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        
        public void AddMessage(string msg)
        {
            //textBox1.AppendText(DateTime.Now.ToString() + " " + msg + Environment.NewLine);
            //if ((textBox1.Lines.Count() % 10) == 0)
            //{
            //    System.IO.File.WriteAllText("log.txt", textBox1.Text);
            //}
            //textBox1.Lines.AsLin .ToList().Add(msg + Environment.NewLine);

            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke((MethodInvoker)(() =>
                {
                    textBox1.AppendText(DateTime.Now.ToString() + " " + msg + Environment.NewLine);
                    if ((textBox1.Lines.Count() % 10) == 0)
                    {
                        System.IO.File.WriteAllText("log.txt", textBox1.Text);
                    }
                }));
                
            }
            else
            {
                textBox1.AppendText(DateTime.Now.ToString() + " " + msg + Environment.NewLine);
                if ((textBox1.Lines.Count() % 10) == 0)
                {
                    System.IO.File.WriteAllText("log.txt", textBox1.Text);
                }
            }

        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
