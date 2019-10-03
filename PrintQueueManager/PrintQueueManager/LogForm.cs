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
                    var line = DateTime.Now.ToString() + "@" + msg;
                    if (textBox1.Lines.Count() == 0)
                    {
                        textBox1.AppendText(Environment.NewLine + line);
                    }
                    else
                    {
                        var lastLine = textBox1.Lines[textBox1.Lines.Count() - 1];
                        if (lastLine.Contains("@"))
                        {
                            var prevMsg = textBox1.Lines[textBox1.Lines.Count() - 1].Split('@')[1];
                            if (prevMsg != msg)
                                textBox1.AppendText(line + Environment.NewLine);
                        }
                        else
                            textBox1.AppendText(Environment.NewLine + line);
                    }

                    if ((textBox1.Lines.Count() % 10) == 0)
                        System.IO.File.WriteAllText("log.txt", textBox1.Text);
                }));                
            }
            else
            {
                var line = DateTime.Now.ToString() + "@" + msg;
                if (textBox1.Lines.Count() == 0)
                {
                    textBox1.AppendText(Environment.NewLine + line);
                }
                else
                {
                    if (textBox1.Lines[textBox1.Lines.Count() - 1].Contains("@"))
                    {
                        var prevMsg = textBox1.Lines[textBox1.Lines.Count() - 1].Split('@')[1];
                        if (prevMsg != msg)
                            textBox1.AppendText(Environment.NewLine + line);
                    }
                    else
                        textBox1.AppendText(Environment.NewLine + line);
                }

                if ((textBox1.Lines.Count() % 10) == 0)
                    System.IO.File.WriteAllText("log.txt", textBox1.Text);
            }
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
