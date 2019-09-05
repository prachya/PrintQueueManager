using System;
using System.Windows.Forms;

namespace PrintQueueManager
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {


                Program.mainForm.config.FTPServer = txtServer.Text;
                Program.mainForm.config.FTPUsername = txtUser.Text;
                Program.mainForm.config.FTPPassword = txtPassword.Text;
                var val = 0;
                if (!int.TryParse(txtCheckInterval.Text, out val))
                    throw new Exception("invalid interval value.");

                if (val < 3)
                    throw new Exception("invalid interval value. the value must be great than 1");

                Program.mainForm.config.CheckInterval = val;
                if (Program.mainForm.config.CheckInterval <= 0)
                    throw new Exception("invalid interval value.");

                if (!txtPDFFolder.Text.EndsWith(@"\"))
                {
                    txtPDFFolder.Text = txtPDFFolder.Text + @"\";
                }
                Program.mainForm.config.PDFFolder = txtPDFFolder.Text;
                //Program.mainForm.config.PrinterSpeedData = txtSpeedData.Text;


                Program.mainForm.SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            txtServer.Text =  Program.mainForm.config.FTPServer;
            txtUser.Text = Program.mainForm.config.FTPUsername;
            txtPassword.Text = Program.mainForm.config.FTPPassword;
            txtCheckInterval.Text = Program.mainForm.config.CheckInterval.ToString();
            txtPDFFolder.Text = Program.mainForm.config.PDFFolder;
            //txtSpeedData.Text = Program.mainForm.config.PrinterSpeedData;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPDFFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
