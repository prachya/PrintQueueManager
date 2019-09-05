using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace PrintQueueManager
{
    public partial class SelectProfileForm : Form
    {
        public SelectProfileForm()
        {
            InitializeComponent();
        }

        public string ExcludeName { get; set; }

        private void SelectProfileForm_Load(object sender, EventArgs e)
        {
            var lst = Program.mainForm.db.PrintProfiles.AsNoTracking().Where(d=>d.ProfileName != ExcludeName).Select(d => d.ProfileName).ToList();
            cmbProfiles.Items.AddRange(lst.ToArray());
            cmbProfiles.SelectedIndex = 0;


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
