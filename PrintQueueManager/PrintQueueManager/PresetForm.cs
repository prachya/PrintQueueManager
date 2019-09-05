using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Printing;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace PrintQueueManager
{
    using Data;
    public partial class PresetForm : Form
    {
        public string PresetName { get; set; }

        public static PaperSourceKind? GetSourceFromName(string name)
        {
            var src = Enum.GetValues(typeof(PaperSourceKind)).Cast<PaperSourceKind>().SingleOrDefault(d => d.ToString() == name); //.Select(d => d.ToString());
            return src;
        }

        public static PaperKind? GetSizeFromName(string name)
        {
            var result = Enum.GetValues(typeof(PaperKind)).Cast<PaperKind>()
                               .SingleOrDefault(d => d.ToString() == name); //.Select(d => d.ToString());
            return result;

        }

        public static PrinterResolutionKind? GetQualityFromName(string name)
        {
            var result = Enum.GetValues(typeof(PrinterResolutionKind)).Cast<PrinterResolutionKind>().SingleOrDefault(d => d.ToString() == name);
            return result;
        }

        public static CustomprinterSettings.MediaType GetMediaType(string name)
        {
            if(name == "Standard")
            {
                return (CustomprinterSettings.MediaType)1;
            }
            else if(name == "Glossy")
            {
                return (CustomprinterSettings.MediaType)2; // 16; // 2;
            }
            else if(name=="Transparency")
            {
                return (CustomprinterSettings.MediaType)3;
            }
            return (CustomprinterSettings.MediaType)1;

        }

        public PresetForm()
        {
            InitializeComponent();

            var lst = Enum.GetValues(typeof(PaperKind)).Cast<PaperKind>()
                                .Where(d => d == PaperKind.A2
                                                 || d == PaperKind.A3
                                                 || d == PaperKind.A4
                                                 || d == PaperKind.A5
                                                 || d == PaperKind.Letter
                                                 || d == PaperKind.Legal
                                                 || d == PaperKind.A6)

                                .Select(d => d.ToString());

            cmbPapersize.Items.AddRange(lst.ToArray());

            var src = Enum.GetValues(typeof(PaperSourceKind)).Cast<PaperSourceKind>().Select(d => d.ToString());

            cmbPaperSources.Items.AddRange(src.ToArray());

            var src1 = Enum.GetValues(typeof(PrinterResolutionKind)).Cast<PrinterResolutionKind>().Select(d => d.ToString());


            

            cmbQualities.Items.AddRange(src1.ToArray());

            var src5 = Enum.GetValues(typeof(PageMediaType)).Cast<PageMediaType>().Select(d => d.ToString());
            //var src2 = new List<string>();
            //src2.Add("Standard");
            //src2.Add("Glossy");
            //src2.Add("Transparency");
            cmbMediaType.Items.AddRange(src5.ToArray());

        }

        public Preset preset { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            
            //Preset preset = null;
            if (preset == null)
                preset = new Preset();
            //else
             //   preset = Program.mainForm.db.Presets.AsNoTracking().SingleOrDefault(d => d.Id == Int32.Parse(txtId.Text));
            preset.PresetName = textBox1.Text;
            preset.PaperSize = cmbPaperSources.Text;
            preset.PaperSource = cmbPaperSources.Text;
            preset.PrintQuality = cmbQualities.Text;
            preset.MediaType = cmbMediaType.Text;
            preset.PaperSize = cmbPapersize.Text;
            if (PresetName == null)
            {
                Program.mainForm.db.Add(preset);
            }
            else
            {
                //preset.Id = Int32.Parse(txtId.Text);
                Program.mainForm.db.Attach(preset).State = EntityState.Modified;
                Program.mainForm.db.Presets.Update(preset);
            }
            Program.mainForm.db.SaveChanges();
            
            //preset = null;

            //if(settings != null)
            //    Program.mainForm.SavePrinterSettings("psetting",settings, preset.Id.ToString());
        }

        private void PresetForm_Shown(object sender, EventArgs e)
        {
            if(PresetName != null )   // this means editing.
            {
                button3.Show();

                preset = Program.mainForm.db.Presets.SingleOrDefault(d => d.PresetName == PresetName);
                if (preset != null) {
                    txtId.Text = preset.Id.ToString();
                    textBox1.Text = preset.PresetName;                    
                    cmbPaperSources.Text = preset.PaperSource;
                    cmbQualities.Text = preset.PrintQuality;
                    cmbMediaType.Text = preset.MediaType;
                    cmbPapersize.Text = preset.PaperSize;

                    PopulateConfiguredPrinters(preset.Id);
                }

                panel1.Show();

                
            }      
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (PresetName != null)
            {
                if (MessageBox.Show("Please confirm deleting this Preset", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                                                                MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    var preset = Program.mainForm.db.Presets.SingleOrDefault(d => d.PresetName == PresetName);

                    Program.mainForm.db.Remove(preset);
                    Program.mainForm.db.SaveChanges();
                }
                
            }
        }

        private void PopulateConfiguredPrinters(int presetId)
        {
            cmbPrinters.Items.Clear();

            var lst = Program.mainForm.db.PresetPrinters.AsNoTracking().Where(d => d.PresetId == presetId).Select(d => d.PrinterName);
            if(lst.Count() > 0)
                cmbPrinters.Items.AddRange(lst.ToArray());
        }

        PrinterSettings settings { get; set; }
        private void button4_Click(object sender, EventArgs e)
        {

            //var f = new SelectPrinterForm();
            //if(f.ShowDialog() == DialogResult.OK)
            //{
            var printer = new PrintDialog();
            if (printer.ShowDialog() == DialogResult.OK) {
                var presetId = Int32.Parse(txtId.Text);
                Program.mainForm.AddConfiguredPrinter(printer.PrinterSettings,presetId);

                PopulateConfiguredPrinters(presetId);
            }
            //}

            return;

            //if (PresetName != null)
            //{
            //    settings = Program.mainForm.LoadPrinterSettings(txtId.Text);
            //}

            //var printer = new PrintDialog();
            //if (settings != null)
            //    printer.PrinterSettings = settings;

            //if (printer.ShowDialog() == DialogResult.OK)
            //{
            //    //printer.PrinterSettings = settings;

            //    //var mode = CustomprinterSettings.DEVMODE;
            //    //Marshal.PtrToStructure(settings.GetHdevmode(), mode); // RetrieveVirtualItemEventArgs dm, yDevModeData, true);

               

            //    var mode = (CustomprinterSettings.DEVMODE)Marshal.PtrToStructure(printer.PrinterSettings.GetHdevmode(), typeof(CustomprinterSettings.DEVMODE));


            //    settings = printer.PrinterSettings;
            //}
        }

        private void PresetForm_Load(object sender, EventArgs e)
        {

        }

        private void btnEditPrinter_Click(object sender, EventArgs e)
        {
            if (cmbPrinters.Text == "")
                return;


            var printer = new PrintDialog();
            var presetId = Int32.Parse(txtId.Text);
            var p = Program.mainForm.db.PresetPrinters.SingleOrDefault(d => d.PresetId == presetId && d.PrinterName == cmbPrinters.Text);
            if (p == null)
                return;

            var settings = Program.mainForm.LoadPrinterSettings("cfgprinter", p.Id.ToString());
            if (settings != null)
                printer.PrinterSettings = settings;
            
            if (printer.ShowDialog() == DialogResult.OK)
            {
               
                    Program.mainForm.SaveConfiguredPrinter(printer.PrinterSettings, p.Id);
            }

        }

        private void btnDeletePrinter_Click(object sender, EventArgs e)
        {
            if (cmbPrinters.Text == "")
                return;


            var printer = new PrintDialog();
            var presetId = Int32.Parse(txtId.Text);
            var p = Program.mainForm.db.PresetPrinters.SingleOrDefault(d => d.PresetId == presetId && d.PrinterName == cmbPrinters.Text);
            if (p == null)
                return;

            Program.mainForm.db.Remove(p);
            Program.mainForm.db.SaveChanges();
            PopulateConfiguredPrinters(presetId);

        }
    }
}
