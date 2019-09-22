using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Runtime.Serialization.Formatters.Binary;
using TallComponents.PDF.Rasterizer;
using System.Drawing.Drawing2D;

namespace PrintQueueManager
{
    using Data;
    using Print;
    using iTextSharp.text.pdf;

    public partial class MainForm : Form
    {
        public Print.PrintManager printManger { get; set; } = new Print.PrintManager();
        public LogForm log { get; set; }
        private bool exitProgram;
        public ConfigData config { get; set; }
        public dataContext db { get; set; }
        public List<ProfileController> ProfileControllers { get; set; } = new List<ProfileController>();
        public MainForm()
        {
            InitializeComponent();
            db = new dataContext();

            LoadConfigData();
            LoadProfileTabs();

            Application.Idle += Application_Idle;

#if !DEBUG
            testToolStripMenuItem.Visible = false;
#endif


        }

        private void Application_Idle(object sender, EventArgs e)
        {
            createProsetToolStripMenuItem.Enabled = !IsStarted();
            newProfileToolStripMenuItem.Enabled = !IsStarted();
            fTPLocationSettingToolStripMenuItem.Enabled = !IsStarted();
            refreshRepoFolderToolStripMenuItem.Enabled = fTPLocationSettingToolStripMenuItem.Enabled;


            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled =
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).lblStatus.Text != "Started";
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnUp.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).maskedTextBox1.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).maskedTextBox2.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).checkBox1.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).cmbPrinter.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).cmbPresets.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            //(tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnMove.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).CanMove;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnChangePrintStatus.Enabled =
                        (tabControl1.SelectedTab.Controls[0] as ProfileFrame).CanChangeStatus; // btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).button2.Enabled = (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnDown.Enabled;
            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).button3.Enabled = !(  ((tabControl1.SelectedTab.Controls[0] as ProfileFrame).button2.Enabled)
                                                                                     || ((tabControl1.SelectedTab.Controls[0] as ProfileFrame).Controller.Stopping) );

            (tabControl1.SelectedTab.Controls[0] as ProfileFrame).btnKill.Enabled = !(((tabControl1.SelectedTab.Controls[0] as ProfileFrame).button2.Enabled)
                                                                                     || ((tabControl1.SelectedTab.Controls[0] as ProfileFrame).Controller.Stopping));
            

        }


        private void LoadProfileTabs()
        {
            var profiles = db.PrintProfiles.Include(d=>d.ProfileFilters).Include(d => d.ProfileOrders).ThenInclude(d => d.Order);

            foreach (var item in profiles)
            {
                var ctrl = new ProfileController(this, db, item);
                ProfileControllers.Add(ctrl);
                ctrl.Show();
            }
        }

        public void CalibratePrinter(string docFilePath,PrinterSettings printerSetting,int noOfCopies,int? presetId,double time /* minutes */)
        {
            PrinterSpeedData data = null;
            if(presetId == null)
                data = config.PrinterSpeedData.FirstOrDefault(d => (d.PrinterName == printerSetting.PrinterName)); // && (d.PresetID == presetId));
            else
                data = config.PrinterSpeedData.FirstOrDefault(d => (d.PrinterName == printerSetting.PrinterName) && (d.PresetID == (int)presetId)  );
            if( data != null)
            {
                var pagePerMinute = 0.00;
                data.Divider = data.Divider + 1;
                data.PagesPerMinute = pagePerMinute / data.Divider;                
            }
        }

        public void DeleteProfile(string ProfileName)
        {
            if (MessageBox.Show("Please confirm deleting profile " + ProfileName, "Delete Profile", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                foreach (var tab in tabControl1.TabPages.OfType<TabPage>())
                {
                    if((tab.Controls[0] as ProfileFrame).txtProfileName.Text == ProfileName)
                    {
                        try
                        {
                            var controller = (tab.Controls[0] as ProfileFrame).Controller;
                            tabControl1.TabPages.Remove(tab);
                            db.Backup();
                            controller.DeleteFromDatabase();
                            this.ProfileControllers.Remove(controller);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("error deleting Profile " + ex.Message);
                        }
                        
                        break;
                    }
                }
                
            }
        }


        private void LoadConfigData()
        {
            if (!File.Exists("config.json"))
            {
                config = new ConfigData();
                config.CheckInterval = 20; // (20*60)*1000;
                //config.PrinterSpeedData = "";
                SaveConfig();
            }

            var s = File.ReadAllText("config.json");
            config = JsonConvert.DeserializeObject<ConfigData>(s);
            if(config.PrinterSpeedData == null)
            {
                config.PrinterSpeedData = new List<PrinterSpeedData>();
            }
        }

        public void SaveConfig()
        {
            var s = JsonConvert.SerializeObject(config);
            File.WriteAllText("config.json", s);
        }

        private ProfileController CreateProfileController()
        {
            var profileData = new PrintProfile();
            profileData.ProfileName = GetNewTabName();
            var filterList = db.PrintProfiles.Select(d => d.FilterType).ToList();
            var lst = Enum.GetValues(typeof(FilterType)).Cast<FilterType>();
            lst = lst.Where(d => !filterList.Any(a => a == d));
            if (!lst.Any())
            {
                MessageBox.Show("No filters available.");
                return null;
            }

            profileData.FilterType = lst.First();

            db.Add(profileData);
            db.SaveChanges();

            var pro = new ProfileController(this, db, profileData);
            return pro;
        }

        public void AddNewTab(ProfileFrame frame)
        {
            var tab = new TabPage();
            tab.Text = frame.txtProfileName.Text;
            frame.Parent = tab;
            frame.Dock = DockStyle.Fill;
            tabControl1.TabPages.Add(tab);
        }

        public string GetNewTabName()
        {
            return "New Profile";
        }

        private void newProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
            var ctrl = CreateProfileController();
            if (ctrl != null)
                ctrl.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoImport();
        }

        private void DoImport(Stream stream)
        {
            var i = new OrdersImporter();
            //var importedList = i.Import("orders.csv");  
#if DEBUG
            var importedList = i.Import(@"c:\temp\PrintRun3.csv"); // C:\projects2\PrintQueueManager\PrintRun(1).csv");
#else
            var importedList = i.Import(stream);
#endif

            ExtractImportedData(importedList);
            foreach (var item in ProfileControllers)
                item.frame.PopulatePrintRun();
        }
        
        private void ExtractImportedData(List<Order> importedList)
        {
            if (!importedList.Any())
                return;

            // filters already imporeted items out. we use OrderId + Sku as unique key.
            // 
#if DEBUG
            var newOrderList = importedList.ToList();
#else

            var newOrderList = importedList.Where(d => !db.Orders.Any(a => (a.OrderId.ToString() + a.Sku.ToString()) == (d.OrderId.ToString() + d.Sku.ToString()) ) ).ToList();
#endif

            // sort by order time
            //
            newOrderList = newOrderList.OrderByDescending(d => d.TimeOfOrder).ToList();

            var order = new OrderController(db);
            // check if PDFs linked
            //
            foreach (var item in newOrderList)
            {
                item.Linked = File.Exists(config.PDFFolder + item.Sku + ".pdf");
            }

            order.SaveOrders(newOrderList);

            log.AddMessage("no. of records to import " + newOrderList.Count());

            var addedList = new List<Order>();

             // 
            foreach (var profile in db.PrintProfiles)
            {
                IEnumerable<Order> addList = null;
                addList = ProfileController.GetOrdersInProfile(profile, newOrderList);

                /*
                IEnumerable<Order> addList = null;
                var ci = new System.Globalization.CultureInfo("en-US");
                if (profile.FilterType == FilterType.ContainsA3)
                {
                    addList = newOrderList.Where(d => d.Sku.IndexOf("A3", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else if (profile.FilterType == FilterType.ContainsA5)
                {
                    addList = newOrderList.Where(d => d.Sku.IndexOf("A5", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else if (profile.FilterType == FilterType.ContainsA4)
                {
                    addList = newOrderList.Where(d => d.Sku.IndexOf("A4", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else if (profile.FilterType == FilterType.ContainsA2)
                {
                    addList = newOrderList.Where(d => d.Sku.IndexOf("A2", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else if (profile.FilterType == FilterType.ContainsA1)
                {
                    addList = newOrderList.Where(d => d.Sku.IndexOf("A1", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }
                else if (profile.FilterType == FilterType.StartsD)
                {                    
                    addList = newOrderList.Where(d => d.Sku.StartsWith("D",true, ci)).ToList();
                }
                else if (profile.FilterType == FilterType.StartsNH)
                {
                    addList = newOrderList.Where(d => d.Sku.StartsWith("N",true, ci) || d.Sku.StartsWith("H",true,ci)).ToList();
                }
                else if (profile.FilterType == FilterType.StartsC)
                {
                    addList = newOrderList.Where(d => d.Sku.StartsWith("C", true, ci) && (!d.Sku.StartsWith("CUS")) ).ToList();
                }
                else
                {
                    ;
                }
                */
                // addList = importedList.Where(d => d.Sku.Contains("A4")); //TODO: warning !, this should not be.

                newOrderList.RemoveAll(d => addList.Contains(d)); // addList.Any(v => v == d));
                if (addList != null && addList.Any())
                    addedList.AddRange(addList);
                else
                    continue;


                var pendingList = db.ProfileOrders.Where(d => d.ProfileId == profile.Id
                                                                && (d.PrintingStatus == Data.PrintingStatus.Pending
                                                                 || d.PrintingStatus == Data.PrintingStatus.Printing));
                var lastPrintingOrder = 100;  // higher number print last, lower number print first.
                if (pendingList.Any())
                    lastPrintingOrder = pendingList.Max(d => d.PrintingOrder);

                foreach (var item in addList.Reverse())
                {
                    var profileId = profile.Id;
                    var profiledOrder = new ProfileOrder();
                    profiledOrder.OrderId = item.Id;
                    profiledOrder.ProfileId = profileId;
                    
                    if ((item.OrderStatus == OrderStatus.Paused)  )
                        profiledOrder.PrintingStatus = Data.PrintingStatus.Paused;
                    else
                        profiledOrder.PrintingStatus = Data.PrintingStatus.Pending;

                    //if (profiledOrder.PrintingStatus == Data.PrintingStatus.Pending)
                    profiledOrder.PrintingOrder = ++lastPrintingOrder;

                    db.Add(profiledOrder);
                    db.SaveChanges();
                }
            }

            log.AddMessage("no. of records imported " + addedList.Count()); 

            //var unAddedList = newOrderList.Where(d => !addedList.Any(x => x == d));
            log.AddMessage("no. of records not imported " + newOrderList.Count.ToString());// (newOrderList.Count - addedList.Count).ToString() ); // unAddedList.Count());

            RefreshProfiles();
        }

        private void DoImport()
        {
            var i = new OrdersImporter();
#if DEBUG
            //var importedList = i.Import("orders221.csv"); // cc.csv");  
            //var importedList = i.Import(@"C:\projects2\PrintQueueManager\orders.csv");
            var importedList = i.Import(@"c:\temp\order.csv");
#else
            var importedList = i.Import("orders.csv");
#endif
            ExtractImportedData(importedList);

            //if (!importedList.Any())
            //    return;

            //// filters already imporeted items out. we use OrderId + Sku as unique key.
            //// 
            //var newOrderList = importedList.Where(d => !db.Orders.Any(a => a.OrderId == d.OrderId && a.Sku == d.Sku)).ToList();

            //// sort by order time
            ////
            //newOrderList = newOrderList.OrderByDescending(d => d.TimeOfOrder).ToList();

            //var order = new OrderController(db);
            //// check if PDFs linked
            ////
            //foreach (var item in newOrderList)
            //{
            //    item.Linked = File.Exists(config.PDFFolder + item.Sku + ".pdf");
            //}

            //order.SaveOrders(newOrderList);

            //log.AddMessage("no. of records to import " + newOrderList.Count());

            //var addedList = new List<Order>();

            //// 
            //foreach (var profile in db.PrintProfiles)
            //{
            //    IEnumerable<Order> addList = null;
            //    if (profile.FilterType == FilterType.ContainsA3)
            //    {
            //        addList = newOrderList.Where(d => d.Sku.Contains("A3"));
            //    }
            //    else if (profile.FilterType == FilterType.ContainsA5)
            //    {
            //        addList = newOrderList.Where(d => d.Sku.Contains("A5"));
            //    }
            //    else if (profile.FilterType == FilterType.ContainsA4)
            //    {
            //        addList = newOrderList.Where(d => d.Sku.Contains("A4"));
            //    }
            //    else if (profile.FilterType == FilterType.StartsD)
            //    {
            //        addList = newOrderList.Where(d => d.Sku.StartsWith("D"));
            //    }
            //    else if (profile.FilterType == FilterType.StartsNH)
            //    {
            //        addList = newOrderList.Where(d => d.Sku.StartsWith("N") || d.Sku.StartsWith("H"));
            //    }
            //    else if (profile.FilterType == FilterType.StartsC)
            //    {
            //        addList = newOrderList.Where(d => d.Sku.StartsWith("C"));
            //    }
            //    else
            //        ; // addList = importedList.Where(d => d.Sku.Contains("A4")); //TODO: warning !, this should not be.

            //    if (addList != null)
            //        addedList.AddRange(addList);

            //    var pendingList = db.ProfileOrders.Where(d => d.ProfileId == profile.Id
            //                                                    && (d.PrintingStatus == Data.PrintingStatus.Pending
            //                                                     || d.PrintingStatus == Data.PrintingStatus.Printing));
            //    var lastPrintingOrder = 100;  // higher number print last, lower number print first.
            //    if (pendingList.Any())
            //        lastPrintingOrder = pendingList.Max(d => d.PrintingOrder);

            //    foreach (var item in addList.Reverse())
            //    {
            //        var profileId = profile.Id;
            //        var profiledOrder = new ProfileOrder();
            //        profiledOrder.OrderId = item.Id;
            //        profiledOrder.ProfileId = profileId;
            //        if (item.OrderStatus == OrderStatus.Paused)
            //            profiledOrder.PrintingStatus = Data.PrintingStatus.Paused;
            //        else
            //            profiledOrder.PrintingStatus = Data.PrintingStatus.Pending;

            //        //if (profiledOrder.PrintingStatus == Data.PrintingStatus.Pending)
            //        profiledOrder.PrintingOrder = ++lastPrintingOrder;

            //        db.Add(profiledOrder);
            //        db.SaveChanges();
            //    }
            //}

            //log.AddMessage("no. of records imported " + addedList.Count());

            //var unAddedList = newOrderList.Where(d => !addedList.Any(x => x == d));
            //log.AddMessage("no. of records not imported " + unAddedList.Count());

            //RefreshProfiles();

        }

        private void RefreshProfiles()
        {
            foreach (var item in ProfileControllers)
                item.LoadProfileOrders();
        }

        private void fTPLocationSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var setting = new ConfigForm();
            if (setting.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var item in ProfileControllers)
                {
                    item.RefreshLinkedFiles();
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
#if DEBUG
            return;
#endif

            //e.Cancel = !exitProgram;
            //if (e.Cancel)
            //{
            //    this.Hide();
            //    notifyIcon1.Visible = true;
            //}




        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exitProgram = true;
            Close();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private async void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //log.AddMessage("asdfasdfasd");
            //DownloadCSV();

            //var ctrl = printManger.CreatePrintController();
            //ctrl.PrinterName = "Canon Inkjet 0278 Class Driver";
            //ctrl.Send(@"C:\projects2\PrintQueueManager\Printer Managment.pdf");
            //await ctrl.Send(@"d:\TestManage.zip");
            //ctrl.Start();


            //ProfileControllers.First().Start();


            var file = File.ReadAllBytes(@"C:\projects2\PrintQueueManager\TestPDF\test.pdf");
            foreach (var item in db.ProfileOrders.Include(d => d.Order))
            {
                File.WriteAllBytes(@"C:\projects2\PrintQueueManager\TestPDF\" + item.Order.Sku + ".pdf", file);
            }


            //var i = 0;
            //foreach(var item in db.ProfileOrders)
            //{
            //    item.PrintingOrder = ++i;
            //    db.Update(item);
            //    db.SaveChanges();
            //}



        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            log = new LogForm();
            log.Show();

            timCheckFTP.Interval = ((config.CheckInterval * 60) * 1000);
            timCheckFTP.Start();

        }
        private bool IsStarted()
        {

            return ProfileControllers.Any(d => d.Status == ProfileController.ProfileStatus.Started);

        }

        private async void timCheckFTP_Tick(object sender, EventArgs e)
        {
            timCheckFTP.Enabled = false;
            try
            {
                //if (ProfileControllers.Any(d => d.Status == ProfileController.ProfileStatus.Started))
                //{
                //    log.AddMessage("A profile is running, downloading CSV aborted.");
                //    return;
                //}
                int retry = 10;
                while (retry > 0)
                {
                    try
                    {
                        var stream = new MemoryStream();
                        try
                        {

#if DEBUG
                            //DoImport();
                            //return;

                            //if (await DownloadCSV(config.FTPServer, config.FTPUsername, config.FTPPassword))
                            ////if (await DownloadCSVToStream(config.FTPServer, config.FTPUsername, config.FTPPassword, stream))
                            //{
                            //    //DoImport(stream);
                            //    DoImport();
                            //}

                            //if (await DownloadCSVToStream(config.FTPServer, config.FTPUsername, config.FTPPassword, stream))
                            //{
                                DoImport(stream);                                
                            //}
#else
                            if (await DownloadCSVToStream(config.FTPServer, config.FTPUsername, config.FTPPassword, stream))
                            {
                                DoImport(stream);                                
                            }

#endif

                        }
                        finally
                        {
                            stream.Dispose();
                        }
                        // DONE.
                        break;
                    }
                    catch (Exception ex)
                    {
                        log.AddMessage(ex.Message);
                        log.AddMessage("retrying..." + retry.ToString());
                        await Task.Delay(30000); // wait for 30 secs. and try again.
                        --retry;
                        continue;
                    }
                   
                }

                if (retry == 0)
                {
                    log.AddMessage("Download CSV failed.");
                    //MessageBox.Show("Download CSV failed. please see the log.");
                }
            }
            finally
            {
                timCheckFTP.Enabled = true;
            }
        }

        private async Task<bool> DownloadCSV(string address, string user, string password)
        {
            try
            {
                log.AddMessage("Downloading CSV...");
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(user, password); // "training", "Revell1775");
                var uri = new Uri(address); // "ftp://shared4.arvixe.com/exportMain.csv");
                await client.DownloadFileTaskAsync(uri, @"orders.csv");
                log.AddMessage("Downloading CSV...done");

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error, unable to download files from FTP." + ex.Message);
                log.AddMessage("unable to download files from FTP." + ex.Message);
                return false;
            }
        }

        private async Task<bool> DownloadCSVToStream(string address, string user, string password,Stream outStream)
        {
            try
            {
                log.AddMessage("Downloading CSV...");
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(user, password); // "training", "Revell1775");
                var uri = new Uri(address); // "ftp://shared4.arvixe.com/exportMain.csv");
                var data = await client.DownloadDataTaskAsync(uri);
                outStream.Write(data,0,data.Length);
                log.AddMessage("Downloading CSV...done");

                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error, unable to download files from FTP." + ex.Message);
                log.AddMessage("unable to download files from FTP." + ex.Message);

                return false;
            }
        }

        public PrinterSettings LoadPrinterSettings(string prefix,string id)
        {
            var pfile = prefix + id + ".dat";
            if (File.Exists(pfile))
            {
                var f = File.ReadAllText(pfile);

                return SettingFromString(f);
            }
            else
                return null;

        }

        public void RefreshPresetDropdowns()
        {
            foreach (var tab in tabControl1.TabPages.OfType<TabPage>())
            {
                (tab.Controls[0] as ProfileFrame).PopulatePresets();
            }

        }

        public int GetPDFNoOfpages(string filePath)
        {
          
            PdfReader pdfReader = new PdfReader(filePath);
            int numberOfPages = pdfReader.NumberOfPages;

            return numberOfPages;
        }

        public void AddConfiguredPrinter(PrinterSettings settings,int presetId)
        {
            var p = new PresetPrinter();            
            p.PrinterName = settings.PrinterName;
            p.PresetId = presetId;
            db.Add(p);
            db.SaveChanges();

            SavePrinterSettings("cfgprinter", settings, p.Id.ToString());
        
        }
        public PrinterSettings GetConfiguredPrinter(int presetId,string printerName)
        {
            var p = db.PresetPrinters.SingleOrDefault(d => d.PresetId == presetId && d.PrinterName == printerName);
            if (p != null)
                return LoadPrinterSettings("cfgprinter", p.Id.ToString());
            return null;

        }

        public void SaveConfiguredPrinter(PrinterSettings settings,int presetPrinterId)
        {
            SavePrinterSettings("cfgprinter", settings, presetPrinterId.ToString());
        }

        public void SavePrinterSettings(string prefix,PrinterSettings data, string id)
        {
            var pfile = prefix + id + ".dat";
            var str = SettingToString(data);
            File.WriteAllText(pfile, str);
        }

        private string SettingToString(PrinterSettings settings)
        {
            if (settings == null)
                return null;

            var bf = new BinaryFormatter();
            using (var ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, settings);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private PrinterSettings SettingFromString(string base64)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (var ms = new System.IO.MemoryStream(Convert.FromBase64String(base64)))
                {
                    return (PrinterSettings)bf.Deserialize(ms);
                }
            }
            catch (Exception)
            {
                return new PrinterSettings();
            }
        }

        private void createProsetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form = new PresetForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshPresetDropdowns();
            }
        }

        public int GetPrinterSpeed(string printerName)
        {

            //var lst = config.PrinterSpeedData.Split('\n'); // Environment.NewLine);
            //foreach (var item in lst)
            //{
            //    var speedData = item.Split(',');
            //    if (speedData.SingleOrDefault(d => d == printerName) != null)
            //        return Int32.Parse(speedData[1]);
            //}

            return 0;

        }

        private void refreshRepoFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in ProfileControllers)
            {
                item.RefreshLinkedFiles();
            }
        }

        private void test1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = GetPrinterSpeed("printer2");


        }

        public class ConfigData
        {
            public string FTPServer { get; set; }
            public string FTPUsername { get; set; }
            public string FTPPassword { get; set; }
            public int CheckInterval { get; set; }
            public string PDFFolder { get; set; }
            public DateTime? StartTime { get; set; }
            public DateTime? StopTime { get; set; }          
            public List<PrinterSpeedData> PrinterSpeedData { get; set; }
            public string PrintRun { get; set; }

        }

        int currentPage = 0;
        Document document;
        PrintDocument printDocument;

    
        private void printTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var file = new FileStream(@"C:\projects2\PrintQueueManager\C0280.pdf", FileMode.Open);
            document = new Document(file);
            //settings.DefaultPageSettings.Landscape = true;

            printDocument = new PrintDocument();

            printDocument.DocumentName = document.DocumentInfo.Title;
            printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument.DocumentName = "dfdf";
            printDocument.PrintPage += new PrintPageEventHandler(printPage);
            printDocument.PrintController = new StandardPrintController();
            //printDocument.DefaultPageSettings.Margins.Left = 0;
            //printDocument.DefaultPageSettings.Margins.Right = 0;
            //printDocument.DefaultPageSettings.Margins.Bottom = 0;
            //printDocument.DefaultPageSettings.Margins.Top = 0;
            
            printDocument.DefaultPageSettings.Landscape = true;
            //printDocument.DefaultPageSettings.PaperSize = new PaperSize( ze => size.Kind == PaperKind.A4);
            //printDocument.OriginAtMargins = true;

            //printDocument.OriginAtMargins = true;
            //printDocument.PrinterSettings.Copies = noOfCopies;
            //printDocument.PrinterSettings.DefaultPageSettings.Landscape = true;
            //if (settings != null)
            //printDocument.DefaultPageSettings = settings.DefaultPageSettings;
            //printDocument.DefaultPageSettings.Landscape = true;            
            printDocument.Print();

        }

        private Matrix GetPrintTransformation(Page page, RectangleF printableArea)
        {            
            Matrix transformation = new Matrix();

            float pdfWidth = (float)page.Width;
            float pdfHeight = (float)page.Height;
            float pdfRatio = pdfWidth / pdfHeight;

            PaperSize paperSize = printDocument.PrinterSettings.PaperSizes[0];
            float paperWidth = ((float)paperSize.Width * 72f) / 100f;
            float paperHeight = ((float)paperSize.Height * 72f) / 100f;

            // It is possible that a driver returns bogus data for the printable area. If that is
            // obviously the case, we are going to ignore the usePrintableArea settings (otherwise
            // we get invalid transforms).
            
            paperWidth = printableArea.Width;
            paperHeight = printableArea.Height;
            transformation.Translate(printableArea.Left, printableArea.Top);

            float paperRatio = paperWidth / paperHeight;

            float scaledPdfWidth = pdfWidth;
            float scaledPdfHeight = pdfHeight;

            bool rotate = false;
            if ((pdfRatio > 1 && paperRatio < 1) || (pdfRatio < 1 && paperRatio > 1))
            {
                // need to rotate
                rotate = true;
                float temp = scaledPdfWidth;
                scaledPdfWidth = scaledPdfHeight;
                scaledPdfHeight = temp;
                pdfRatio = 1 / pdfRatio;
            }

            float scale = 1;
            if (pdfRatio > paperRatio)
                scale = paperWidth / scaledPdfWidth;
            else
                scale = paperHeight / scaledPdfHeight;

            scaledPdfWidth *= scale;
            scaledPdfHeight *= scale;

            // auto-rotate
            //if (rotate)
            //{
            //    transformation.Rotate(-90);
            //    transformation.Translate(-scaledPdfHeight, 0);
            //}

            // scale
            transformation.Scale(scale, scale);

            return transformation;
        }

        void printFitPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Point;

            Page page = document.Pages[currentPage++];

            autoPosition(
               e.Graphics,
               (e.PageSettings.PaperSize.Width * 72) / 100,
               (e.PageSettings.PaperSize.Height * 72) / 100,
               // As of .NET 2.0, you can use the printable area 
               // instead of the papersize.
               // ( e.PageSettings.PrintableArea.Width * 72 ) / 100, 
               //( e.PageSettings.PrintableArea.Height * 72 ) / 100,
               page.Width, page.Height);

            page.Draw(e.Graphics);
            e.HasMorePages = currentPage < document.Pages.Count;
        }

        void autoPosition(Graphics graphics, double printerWidth,
           double printerHeight, double pdfWidth, double pdfHeight)
        {
            double scaleX = printerWidth / pdfWidth;
            double scaleY = printerHeight / pdfHeight;
            double scale = Math.Min(scaleX, scaleY);

            graphics.ScaleTransform((float)scale, (float)scale);
            graphics.TranslateTransform(
               (float)(printerWidth - scale * pdfWidth) / 2,
               (float)(printerHeight - scale * pdfHeight) / 2);
        }

        void printPage(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.PageUnit = GraphicsUnit.Point;
            //Page page = document.Pages[pageIndex++];
            //page.Draw(e.Graphics);
            //e.HasMorePages = pageIndex < document.Pages.Count;
            
            e.HasMorePages = false;
            if (currentPage <= document.Pages.Count)
            {
                e.Graphics.PageUnit = GraphicsUnit.Point;
                Page page = document.Pages[currentPage];

                e.Graphics.MultiplyTransform(GetPrintTransformation(page, e.Graphics.VisibleClipBounds));
                page.Draw(e.Graphics);
                if (currentPage < document.Pages.Count - 1)
                {
                    currentPage++;
                    e.HasMorePages = true;
                }
            }

        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // This event is called once for each tab button in your tab control

            // First paint the background with a color based on the current tab

            // e.Index is the index of the tab in the TabPages collection.
            if( (tabControl1.TabPages[e.Index].Controls[0] as ProfileFrame).lblStatus.Text == "Started")
            {
                // Then draw the current tab button text 
                e.Graphics.FillRectangle(new SolidBrush(Color.Green), e.Bounds);
                System.Drawing.Rectangle paddedBounds = e.Bounds;
                paddedBounds.Inflate(-2, -2);
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, this.Font, SystemBrushes.ControlLight, paddedBounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                System.Drawing.Rectangle paddedBounds = e.Bounds;
                paddedBounds.Inflate(-2, -2);
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, this.Font, SystemBrushes.ActiveCaptionText, paddedBounds);
            }
            

            //switch (e.Index)
            //{
            //    case 0:
            //        e.Graphics.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
            //        break;
            //    case 1:
            //        e.Graphics.FillRectangle(new SolidBrush(Color.Blue), e.Bounds);
            //        break;
            //    default:
            //        break;
            //}


        }

        private void moretestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var x = "AMZ_00001.PDF";
            MessageBox.Show(x.Remove(0, 4));
        }
    }

    public static class MyExtension
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }



}
