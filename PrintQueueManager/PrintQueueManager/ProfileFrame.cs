using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Printing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace PrintQueueManager
{
    using Data;

    

    public partial class ProfileFrame : UserControl
    {

        private ProfileController _controller;
        public ProfileController Controller { get { return _controller; }
            set
            {
                _controller = value;
                _controller.printController.OnPrintingStatusChanged += PrintController_OnPrintingStatusChanged;
                _controller.OnPrintingFinished += PrintController_OnPrintingFinished1;
                _controller.OnPrintItemStatusChanged += _controller_OnPrintItemStatusChanged;

                PopulatePresets();



                LoadFormData(_controller.profile);

                PopulatePrintRun();
                UpdateInformationStatuses();

                SetPrintTimer();
                    
            }
        }

        public void AddPrintingStatus(string txt)
        {

            if (txtPrintingStatus.InvokeRequired)
            {
                txtPrintingStatus.Invoke((MethodInvoker)(() =>
                {
                    txtPrintingStatus.Text = txt;                    
                }));

            }
            else
            {
                txtPrintingStatus.Text = txt;
            }
        }

        public bool CanChangeStatus { get
            {
                if (orderGridView.CurrentRow == null)
                    return false;

                var profileOrder = (orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow).ProfileOrder;
                return profileOrder.PrintingStatus != PrintingStatus.Printing;
            }
        }

        private void LoadFormData(PrintProfile profile)
        {
            cmbFilterTypes.SelectedIndex = (int)profile.FilterType;
            txtProfileName.Text = profile.ProfileName;


            var preset = Controller.db.Presets.AsNoTracking().SingleOrDefault(d => d.Id == profile.PresetId);
            if (preset != null)
                cmbPresets.Text = preset.PresetName;

            if (profile.StartTime != null)
            {
                var txt = ((DateTime)profile.StartTime).ToString("HH.mm");
                maskedTextBox1.Text = txt;
            }

            if (profile.EndTime != null)
                maskedTextBox2.Text =  ((DateTime)profile.EndTime).ToString("HH.mm");

            checkBox1.Checked = profile.AutoStartStop;


        }

        private void _controller_OnPrintItemStatusChanged(object sender, ProfileOrder profileOrder, PrintingStatus status)
        {
            //throw new NotImplementedException();
            //foreach (DataGridViewRow item in orderGridView.Rows)
            //{
            //    //var val = item.Cells["Id"].Value;
            //    if (profileOrder.Id == (item.DataBoundItem as PrintItemDataRow).Id)
            //    {
            //        //item.Cells["PrintStatus"].Value = "Complete";
            //        (item.DataBoundItem as PrintItemDataRow).PrintingStatus = "Complete";
            //        break;
            //    }
            //}

            var item = orderGridView.Rows.OfType<DataGridViewRow>().Where(d => (d.DataBoundItem as PrintItemDataRow).Id == profileOrder.OrderId).FirstOrDefault();
            if(item != null)
            {
                if(status == PrintingStatus.Printing)
                {
                    (item.DataBoundItem as PrintItemDataRow).ProfileOrder.PrintingStatus = PrintingStatus.Printing; 
                    (item.DataBoundItem as PrintItemDataRow).PrintingStatus = "Printing";
                    orderGridView.Refresh();

                    UpdateInformationStatuses();
                }
                else if(status == PrintingStatus.Complete)
                {
                    (item.DataBoundItem as PrintItemDataRow).ProfileOrder.PrintingStatus = PrintingStatus.Complete;
                    (item.DataBoundItem as PrintItemDataRow).PrintingStatus = "Complete";
                    orderGridView.Refresh();

                    AddPrintingStatus("");

                    UpdateInformationStatuses();
                }
            }

        }

        private void PrintController_OnPrintingFinished1(object sender, ProfileOrder profileOrder)
        {

            ////throw new NotImplementedException();
            //foreach(DataGridViewRow item in orderGridView.Rows)
            //{
            //    var val = item.Cells["Id"].Value;
            //    if (profileOrder.Id == (int)val)
            //    {
            //        //item.Cells["PrintStatus"].Value = "Complete";
            //        (item.DataBoundItem as PrintItemDataRow).PrintingStatus = "Complete";
            //    }
            //}
            
        }

        private void PrintController_OnPrintingStatusChanged(object sender, Print.PrintingStatus status)
        {
            //throw new NotImplementedException();
        }

        public ProfileFrame()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            
            orderGridView.DoubleBuffered(true);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        
        public void LoadProfileOrders(IEnumerable<PrintItemDataRow> lst)
        {
            //var lst = Controller.db.ProfileOrders.Select(d => new
            //{
            //    d.Order.Quantity,
            //    SKU = d.Order.Sku,
            //    Linked = d.Order.Linked ? "Yes" : "--",
            //    d.PrintingStatus,
            //    d.Order.TimeOfOrder,
            //    d.Order.CustomerName,
            //    d.Order.OrderNotes,
            //    d.Order.CustomerEmail,
            //    d.PrintingOrder,
            //    d.Id

            //}).OrderByDescending(d => d.PrintingOrder).ThenByDescending(d => d.TimeOfOrder).ToList();



            //Reprint.DisplayIndex = 5;
            
            orderGridView.DataSource = lst;
            orderGridView.Refresh();

            //var i = 0;
            //foreach (DataGridViewRow item in orderGridView.Rows)
            //{

            //dynamic x = item.DataBoundItem;
            //x.PrintingStatus = "Complete";
            // item.Cells["Qty"].Value = "999";
            //orderGridView.UpdateCellValue(2, i);
            //i++;

            //}

            PopulatePageSelector();

        }

        private void cmbFilterTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            // save to database
            
        }

        private void PopulateOnlinePrinters()
        {
            var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
            cmbPrinter.Items.AddRange(printers.OfType<string>().ToArray());
            cmbPrinter.SelectedIndex = 0;

            if (Controller.profile.SelectedPrinter != null)
            {
                var idx = cmbPrinter.Items.IndexOf(Controller.profile.SelectedPrinter);
                if (idx >= 0)
                    cmbPrinter.SelectedIndex = idx;
            }

            Controller.printController.PrinterName = cmbPrinter.Text;                
        }

        private void ProfileFrame_Load(object sender, EventArgs e)
        {
            PopulateOnlinePrinters();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Controller.printController.PrinterName = comboBox2.Text;



            Controller.SetPrinter(cmbPrinter.Text);

        }

        private void orderGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            //var col = new DataGridViewLinkCell();
            //if (orderGridView.RowCount > 0)
            //    orderGridView[e.ColumnIndex, 0] = col;

            

            if (e.ColumnIndex == orderGridView.Columns["PrintStatus"].Index)
            {
                var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex); 
                if (r != null)
                {
                    var printItem = (r.DataBoundItem as PrintItemDataRow);
                    if(printItem.ProfileOrder.PrintingStatus == PrintingStatus.Pending)
                    {
                        e.Value = "Pending";
                    }
                    else if (printItem.ProfileOrder.PrintingStatus == PrintingStatus.Complete)
                    {
                        e.Value = "Complete";
                        this.PopulatePrintRun();
                    }
                    else if (printItem.ProfileOrder.PrintingStatus == PrintingStatus.Printing)
                    {
                        e.Value = "Printing";
                    }
                    else if (printItem.ProfileOrder.PrintingStatus == PrintingStatus.Paused)
                    {
                        e.Value = "Paused";
                    }

                }


                if (e.Value.ToString() == "Complete")
                {
                    //orderGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    e.CellStyle.ForeColor = Color.Green;

                }
                else if (e.Value.ToString() == "Printing")
                {
                    //orderGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
                    e.CellStyle.ForeColor = Color.Blue;
                }
                else if (e.Value.ToString() == "Paused")
                    e.CellStyle.ForeColor = Color.DarkOrange;
                else
                {
                    //orderGridView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    e.CellStyle.ForeColor = Color.Black;
                }

                               
            }

            if(e.ColumnIndex == 2)
            {
                if (e.Value.ToString() == "Yes")
                {
                    e.CellStyle.ForeColor = Color.Green;
                }
            }


            if ((orderGridView.Rows[e.RowIndex].DataBoundItem as PrintItemDataRow).PrintingStatus == "Printing")
            {
                e.CellStyle.BackColor = Color.LightBlue;
            }
            else
                e.CellStyle.BackColor = Color.White;

        }

        private void cmbFilterTypes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Controller.profile.FilterType = (FilterType)cmbFilterTypes.SelectedIndex;
            Controller.db.Update(Controller.profile);
            Controller.db.SaveChanges();
        }

        private void txtProfileName_Leave(object sender, EventArgs e)
        {
            Controller.profile.ProfileName = txtProfileName.Text;
            Controller.db.SaveChanges();
            (this.Parent as TabPage).Text = txtProfileName.Text;
        }

        private void SelectRow(int profileOrderId)
        {
            var row = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => (d.DataBoundItem as PrintItemDataRow).Id == profileOrderId);
            if (row != null)
            {
                orderGridView.CurrentCell = row.Cells[2];
                row.Selected = true;
            }
        }

        private int GetPrintRunCount(string printRun)
        {
            return Controller.profile.ProfileOrders.Count(d => (d.Order.PrintRun == printRun)
                                                    && (d.Order.OrderStatus == OrderStatus.Pending)
                                                    );
        }

        public void PopulatePrintRun()
        {
            var printRunlist = Controller.profile.ProfileOrders.Where(d => (d.Order.PrintRun != null) 
                                                                            && (d.Order.PrintRun != "")).OrderBy(d => d.Order.PrintRun)
                                                                                                        .Select(d => d.Order.PrintRun).Distinct();
            cmbPrintRun.DisplayMember = "Text";
            cmbPrintRun.ValueMember = "Value";
            cmbPrintRun.Items.Clear();
            foreach(var item in printRunlist)
            {
                var c = GetPrintRunCount(item);
                if(c > 0)
                    cmbPrintRun.Items.Add(new { Text = item + " (" + c.ToString() + ")", Value = item } );
            }

            cmbPrintRun.Items.Insert(0,new { Text = "<Run All>", Value = "Run All" });
            if (Controller.profile.PrintRun != "")
            {
                var obj = cmbPrintRun.Items.OfType<dynamic>().FirstOrDefault(d => d.Value == Controller.profile.PrintRun);
                if (obj != null)
                    cmbPrintRun.SelectedIndex = cmbPrintRun.FindStringExact(obj.Text);
                else
                    cmbPrintRun.SelectedIndex = 0;
            }
            else
                cmbPrintRun.SelectedIndex = 0;
        }

        private PrintItemDataRow GetSelectedProfileOrder()
        {
            if (orderGridView.CurrentRow != null)
            {
                return (orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow);
            }
            else
                return null;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            //var item = orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow; //.DefaultCellStyle.ForeColor = Color.Green;
            var item = GetSelectedProfileOrder();
            if (Controller.Status == ProfileController.ProfileStatus.Stopped 
                && item != null && item.PrintingStatus != "Complete")
            {
                Controller.OrderUP(item.ProfileOrder);
                SelectRow(item.Id);
            }

        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            //var item = orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow; //.DefaultCellStyle.ForeColor = Color.Green;
            var item = GetSelectedProfileOrder();
            if (Controller.Status == ProfileController.ProfileStatus.Stopped
                && item != null && item.PrintingStatus != "Complete")
            {
                Controller.OrderDown(item.ProfileOrder);
                SelectRow(item.Id);
            }
        }

        private List<PrintItemDataRow> CurrentSearchResult;
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                CurrentSearchResult = Controller.Search(txtSearch.Text);
                if (CurrentSearchResult != null && CurrentSearchResult.Any())
                {
                    //SelectRow((int)profileOrderId);
                    var item = CurrentSearchResult.First();
                    SelectRow(item.Id);
                    CurrentSearchResult.Remove(item);
                }
                else
                    MessageBox.Show("Not found");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Controller.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Controller.Stop();
        }

        private void txtProfileName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void orderGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            //if(e.ColumnIndex==6)
            //{
            //    Cursor.Current = Cursors.Hand;
            //}
        }

        private void orderGridView_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && Controller.Status == ProfileController.ProfileStatus.Stopped)
            {
                if (e.ColumnIndex == 2)
                {
                    var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex); // (d.DataBoundItem as PrintItemDataRow).Id == profileOrderId);
                    if (r != null)
                    {
                        Cursor.Current = Cursors.Hand;
                    }

                    //if (orderGridView[e.ColumnIndex, e.RowIndex].Value != "Yes")
                    //    Cursor.Current = Cursors.Hand;               
                }
                else if (e.ColumnIndex == orderGridView.Columns["PrintStatus"].Index)
                {
                    var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex);
                    if (r != null && (r.DataBoundItem as PrintItemDataRow).PrintingStatus == "Paused")
                    {
                        Cursor.Current = Cursors.Hand;
                    }

                }
            }
        }

        private void orderGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && Controller.Status == ProfileController.ProfileStatus.Stopped)
            {
                var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex);
                if ((r.DataBoundItem as PrintItemDataRow).PrintingStatus == "Complete")
                    return;

                if (e.ColumnIndex == 2)
                {

                    //var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex);
                    if (r != null)
                    {
                        var printItem = (r.DataBoundItem as PrintItemDataRow);
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            printItem.ProfileOrder.Order.Linked = true;
                            printItem.ProfileOrder.Order.LinkedFilename = openFileDialog1.FileName;
                            printItem.Linked = "Yes";
                            printItem.LinkedFileName = printItem.ProfileOrder.Order.LinkedFilename;

                            Controller.db.Update(printItem.ProfileOrder.Order);
                            Controller.db.SaveChanges();

                            orderGridView.Refresh();
                        }

                    }

                }                
                else if (e.ColumnIndex == orderGridView.Columns["PrintStatus"].Index)
                {
                    //var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex);
                    if (r != null && (r.DataBoundItem as PrintItemDataRow).PrintingStatus == "Paused")
                    {
                        if(MessageBox.Show("Please confirm change the selected item to 'Pending'.","Confirm", 
                                                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information, 
                                                                MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            var printItem = (r.DataBoundItem as PrintItemDataRow);
                            if (printItem != null)
                            {
                                //printItem.ProfileOrder.Order.OrderStatus = OrderStatus.Pending;
                                //printItem.ProfileOrder.PrintingStatus = PrintingStatus.Pending;
                                //printItem.PrintingStatus = "Pending";
                                //Controller.db.Update(printItem.ProfileOrder.Order);
                                //Controller.db.Update(printItem.ProfileOrder);
                                //Controller.db.SaveChanges();


                                //printItem.profileOrder.PrintingStatus = "Pending";
                                var lst = new List<ProfileOrder>();
                                lst.Add(printItem.ProfileOrder);
                                Controller.ChangeToPending(lst); // printItem.ProfileOrder);
                                orderGridView.Refresh();
                            }
                        }
                    
                        
                    }
                    else if(r != null && (r.DataBoundItem as PrintItemDataRow).PrintingStatus == "Pending")
                    {
                        if (MessageBox.Show("Please confirm change the selected item to 'Paused'.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                                                               MessageBoxDefaultButton.Button1) == DialogResult.OK)
                        {
                            var printItem = (r.DataBoundItem as PrintItemDataRow);
                            var lst = new List<ProfileOrder>();
                            lst.Add(printItem.ProfileOrder);
                            Controller.ChangeToPaused(lst); // printItem.ProfileOrder);
                            orderGridView.Refresh();
                        }

                    }
                }

                else if (e.ColumnIndex == orderGridView.Columns["LinkedFilename"].Index)
                {
                   
                    var printItem = (r.DataBoundItem as PrintItemDataRow);
                    if (String.IsNullOrEmpty(printItem.LinkedFileName) || (r.DataBoundItem as PrintItemDataRow).PrintingStatus == "Complete")
                        return;

                    if (MessageBox.Show("Please confirm removing this link.", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,
                                                            MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        
                       
                        //printItem.ProfileOrder.Order.Linked = true;
                        printItem.ProfileOrder.Order.LinkedFilename = "";
                        //printItem.Linked = "Yes";                       
                        printItem.LinkedFileName = "";

                        Controller.db.Update(printItem.ProfileOrder.Order);
                        Controller.db.SaveChanges();

                        orderGridView.Refresh();

                    }
                }
            }
        

        }

        private void orderGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0 || Controller.Status == ProfileController.ProfileStatus.Started)
            {
                return;
            }


            if (e.ColumnIndex == orderGridView.Columns["Reprint"].Index)
            {
                var r = orderGridView.Rows.OfType<DataGridViewRow>().SingleOrDefault(d => d.Index == e.RowIndex);
                if ((r.DataBoundItem as PrintItemDataRow).PrintingStatus != "Complete")
                    return;

                var printItem = (r.DataBoundItem as PrintItemDataRow);

                //if (printItem.ProfileOrder.Order.ReprintedQuantity != null)
                //{
                //    MessageBox.Show("This item was already reprinted");
                //    return;
                //}

                var reprint = new ReprintForm();
                reprint.txtQty.Text = printItem.Quantity.ToString();
                if (reprint.ShowDialog() == DialogResult.OK)
                {
                    Controller.SetReprint(printItem.ProfileOrder, Int32.Parse(reprint.txtQty.Text));
                }
            }
        }

        public void UpdateInformationStatuses()
        {
            var totalComplets = Controller.profile.ProfileOrders.Count(d => d.Order.OrderStatus == OrderStatus.Complete);
            var pendings = Controller.profile.ProfileOrders.Count(d => d.Order.OrderStatus == OrderStatus.Pending);
            var pauses = Controller.profile.ProfileOrders.Count(d => d.Order.OrderStatus == OrderStatus.Paused);           
            lblComlete.Text = totalComplets.ToString(); 
            lblPaused.Text = pauses.ToString();
            lblPending.Text = pendings.ToString(); 
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        public void PopulatePresets()
        {
            //var idx = cmbPresets.SelectedIndex;
            var selected = cmbPresets.Text;

            cmbPresets.Items.Clear();
            var items = _controller.db.Presets.Select(d => d.PresetName).AsNoTracking().ToList();
            cmbPresets.Items.AddRange(items.ToArray());
            if (selected != "")
                cmbPresets.Text = selected;

            
            //PopulatePresets();
            //try
            //{
            //    cmbPresets.SelectedIndex = idx;
            //}
            //catch
            //{
            //    ;
            //}

        }

        public void PopulatePageSelector()
        {
            var lastPage = Controller.GetNoOfPages(); // Math.Round(c, 0);
            cmbPageSelector.Items.Clear();
            for(var i=1;i <= lastPage;i++)
                cmbPageSelector.Items.Add(i);
            
            if(cmbPageSelector.Items.Count > 0)
            {
                var idx = cmbPageSelector.Items.IndexOf(Controller.CurrPage);
                if (idx >= 0)
                    cmbPageSelector.SelectedIndex = idx;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cmbPresets.SelectedIndex >= 0)
            {
                var preset = new PresetForm();
                preset.PresetName = cmbPresets.Text;
                var result = preset.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //var idx = cmbPresets.SelectedIndex;
                    PopulatePresets();
                   
                }
                else if (result == DialogResult.Yes)   // delete, then refresh all Preset dropdowns in all profiles.
                {
                    Program.mainForm.RefreshPresetDropdowns();
                }
                preset.Dispose();
            }
        }

        // ---------------------- GetPrintTicketFromPrinter -----------------------
        /// <summary>
        ///   Returns a PrintTicket based on the current default printer.</summary>
        /// <returns>
        ///   A PrintTicket for the current local default printer.</returns>
        private PrintTicket GetPrintTicketFromPrinter(string printName)
        {
            PrintQueue printQueue = null;

            LocalPrintServer localPrintServer = new LocalPrintServer();

            //// Retrieving collection of local printer on user machine
            //PrintQueueCollection localPrinterCollection =
            //    localPrintServer.GetPrintQueues();

            //System.Collections.IEnumerator localPrinterEnumerator =
            //    localPrinterCollection.GetEnumerator();

            //if (localPrinterEnumerator.MoveNext())
            //{
            //    // Get PrintQueue from first available printer
            //    printQueue = (PrintQueue)localPrinterEnumerator.Current;
            //}
            //else
            //{
            //    // No printer exist, return null PrintTicket
            //    return null;
            //}

            printQueue = localPrintServer.GetPrintQueue(printName);

            // Get default PrintTicket from printer
            PrintTicket printTicket = printQueue.DefaultPrintTicket;

            PrintCapabilities printCapabilites = printQueue.GetPrintCapabilities();

            // Modify PrintTicket
            if (printCapabilites.CollationCapability.Contains(Collation.Collated))
            {
                printTicket.Collation = Collation.Collated;
            }

            if (printCapabilites.DuplexingCapability.Contains(
                    Duplexing.TwoSidedLongEdge))
            {
                printTicket.Duplexing = Duplexing.TwoSidedLongEdge;
            }

            if (printCapabilites.StaplingCapability.Contains(Stapling.StapleDualLeft))
            {
                printTicket.Stapling = Stapling.StapleDualLeft;
            }

            return printTicket;
        }// end:GetPrintTicketFromPrinter(

        private void cmbPresets_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //PrintDocument pdoc = new PrintDocument();

            //pdoc.PrinterSettings.PrinterName = "HP Color LaserJet 2500 PCL6 Class Driver";
            ////var s = pdoc.DefaultPageSettings;
            ////pdoc.PrinterSettings.DefaultPageSettings; //.Landscape = true;
            ////pdoc.PrinterSettings.DefaultPageSettings.PaperSize.Height = 140;
            ////pdoc.PrinterSettings.DefaultPageSettings.PaperSize.Width = 104;

            //var p = new PrinterSettings();
            //p.PrinterName = "HP Color LaserJet 2500 PCL6 Class Driver";
            //var st = p.DefaultPageSettings;

            //var ticket = GetPrintTicketFromPrinter(cmbPrinter.Text);
            //ticket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);


            
            ////return;

            //var dlg = new PrintDialog();


            //var settings = new PrinterSettings();

            //settings.PrinterName = cmbPrinter.Text;
            ////settings.DefaultPageSettings.PaperSize.RawKind = 1; // (int)PaperKind.Legal;
            ////settings.DefaultPageSettings.Landscape = false;
            //dlg.PrinterSettings = settings;


            ////ShowPrinterProperties(dlg.PrinterSettings);
            ////var set = new PrinterSettings();
            ////set.PrinterName = cmbPrinter.Text;
            ////set.DefaultPageSettings.PaperSize.RawKind = (int)PaperKind.A5;
            ////dlg.PrinterSettings = set; // .PrinterSettings.PaperSize.RawKind = (int)PaperKind.A5; // settings;

            //dlg.ShowDialog();
            ////var s = dlg.PrinterSettings;
            //var fm = SettingToString(dlg.PrinterSettings);


            //var s = SettingFromString(fm);

            //var d1 = new PrintDialog();
            //s.PrinterName = "Canon Inkjet MX340 series";
            //d1.PrinterSettings = s;
            //d1.ShowDialog();
            ////var m = d1.PrinterSettings.GetHdevmode();

            //return;



            var preset = Controller.db.Presets.AsNoTracking().SingleOrDefault(d => d.PresetName == cmbPresets.Text);
            if (preset != null)
            {

                var printer =  cmbPrinter.Text;

                //var dlg = new PrintDialog();
                //var settings = new PrinterSettings();
                ////settings.PageSe
                //dlg.PrinterSettings.PrinterName = printer;
                //var set = new PrinterSettings();
                //set.PrinterName = printer;
                //set.DefaultPageSettings.PaperSize.RawKind = (int)PaperKind.A5;
                //dlg.PrinterSettings = set; // .PrinterSettings.PaperSize.RawKind = (int)PaperKind.A5; // settings;
                //dlg.ShowDialog();

                
                // var printerServer = new LocalPrintServer(); // new PrintServer(PrinterName);


                ////var q = printerServer.GetPrintQueue(PrinterName); // new[] { EnumeratedPrintQueueTypes.Local }); //  PrinterName); // Microsoft XPS Document Writer");
                //var q = printerServer.GetPrintQueue(printer); // new[] { EnumeratedPrintQueueTypes.Local }); //  PrinterName); // Microsoft XPS Document Writer");
                //var mds = new PageMediaSize(PageMediaSizeName.ISOA5);
                //q.DefaultPrintTicket.PageMediaSize = mds;



                //var setting = new CustomprinterSettings.PrinterSettings();
                //var data = new CustomprinterSettings.PrinterData();
                //data.Size = (CustomprinterSettings.PaperSize)PresetForm.GetSizeFromName(preset.PaperSize); // CustomprinterSettings.PaperSize.DMPAPER_A5;
                //data.Duplex =  CustomprinterSettings.PageDuplex.DMDUP_SIMPLEX;
                //data.Orientation = CustomprinterSettings.PageOrientation.DMORIENT_PORTRAIT;
                //data.Media = PresetForm.GetMediaType(preset.MediaType);
                //data.source =  (CustomprinterSettings.PaperSource)PresetForm.GetSourceFromName(preset.PaperSource); // CustomprinterSettings.PaperSource.DMBIN_AUTO;
                //data.Quality = (CustomprinterSettings.PrintQuality)PresetForm.GetQualityFromName(preset.PrintQuality);

                //setting.ChangePrinterSetting(printer, data);

               

               // return;

                //var d = new PrintDialog();
                //d.PrinterSettings.PrinterName = cmbPrinter.Text;
                //d.ShowDialog();
                //var hd = d.PrinterSettings.GetHdevmode(d.PrinterSettings.DefaultPageSettings);
                
                //setting.ChangePrinterSetting2(printer, hd); //.GetHdevmode(d.PrinterSettings.DefaultPageSettings));
                ////var d1 = new PrintDialog();
                ////d1.PrinterSettings.PrinterName = cmbPrinter.Text;
                ////d1.PrinterSettings.SetHdevmode(hd);
                ////d1.ShowDialog();

                // save.
                Controller.profile.PresetId = preset.Id;
                Controller.db.Update(Controller.profile);
                Controller.db.SaveChanges();
            }
        }

        
        private void maskedTextBox1_Leave(object sender, EventArgs e)
        {

            if (maskedTextBox1.Text != "")
            {
                DateTime p;
                DateTime startDateTime;
                if (DateTime.TryParseExact(maskedTextBox1.Text, "HH.mm", null, System.Globalization.DateTimeStyles.None,out startDateTime))
                {
                    Controller.profile.StartTime = startDateTime;
                    Controller.db.Update(Controller.profile);
                    Controller.db.SaveChanges();

                    SetPrintTimer();
                }
            }
        }

        //private DateTime? StartPrintTime { get; set; } = null;

        private void SetPrintTimer()
        {
            if (!checkBox1.Checked)
                return;

            if (Controller.profile.StartTime != null)
            {
                startPrintTimer.Enabled = false;
                try
                {
                    var startDateTime = DateTime.ParseExact(maskedTextBox1.Text, "HH.mm", null, System.Globalization.DateTimeStyles.None);
                    var now = DateTime.Now;
                    var StartPrintTime = new DateTime(now.Year, now.Month, now.Day, startDateTime.Hour, startDateTime.Minute, 0);
                    var nextStartPrintTime = StartPrintTime;
                    var nextPrint = nextStartPrintTime - DateTime.Now;
                    if (nextPrint.TotalMilliseconds <= 0)
                    {
                        nextStartPrintTime = StartPrintTime.AddDays(1);
                    }
                    nextPrint = nextStartPrintTime - DateTime.Now;
                    startPrintTimer.Interval = (int)nextPrint.TotalMilliseconds;
                }
                finally
                {
                    startPrintTimer.Enabled = true;
                }
            }

            if(Controller.profile.EndTime != null)
            {
                var endDateTime = DateTime.ParseExact(maskedTextBox2.Text, "HH.mm", null, System.Globalization.DateTimeStyles.None);
                var now = DateTime.Now;
                var endPrintTime = new DateTime(now.Year, now.Month, now.Day, endDateTime.Hour, endDateTime.Minute, 0);
                var nextEndPrintTime = endPrintTime;
                var nextEnd = nextEndPrintTime - DateTime.Now;
                if (nextEnd.TotalMilliseconds <= 0)
                {
                    nextEndPrintTime = endPrintTime.AddDays(1);
                }
                nextEnd = nextEndPrintTime - DateTime.Now;
                timer2.Interval = (int)nextEnd.TotalMilliseconds;
                timer2.Start();
            }

        }

        private void maskedTextBox2_Leave(object sender, EventArgs e)
        {
            if (maskedTextBox2.Text != "")
            {

                DateTime endDateTime;
                if (DateTime.TryParseExact(maskedTextBox2.Text, "HH.mm", null, System.Globalization.DateTimeStyles.None, out endDateTime))
                {
                    Controller.profile.EndTime = endDateTime;
                    Controller.db.Update(Controller.profile);
                    Controller.db.SaveChanges();

                    SetPrintTimer();
                }
            }          
        }

        private DateTime? lastPrinted;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {

                startPrintTimer.Enabled = false;
                //var secs = 
                //var s = secs.TotalMilliseconds;
                //var secs = (DateTime.Now - (DateTime)StartPrintTime).TotalMilliseconds;
                //if (StartPrintTime != null && (secs < timer1.Interval + 10000)) //(lastPrinted == null || (lastPrinted != null && lastPrinted != DateTime.Today)) )
                //{
                    //var now = DateTime.Now;
                    //if (now.Hour >= StartPrintTime?.Hour && now.Minute >= StartPrintTime?.Minute)
                    //{
                Controller.Start();

                var startDateTime = DateTime.ParseExact(maskedTextBox1.Text, "HH.mm", null, System.Globalization.DateTimeStyles.None);
                var now = DateTime.Now;
                var StartPrintTime = new DateTime(now.Year, now.Month, now.Day, startDateTime.Hour, startDateTime.Minute, 0);
       
                var nextStartPrintTime = StartPrintTime.AddDays(1);
                var nextPrint = nextStartPrintTime - DateTime.Now;

                startPrintTimer.Interval = (int)nextPrint.TotalMilliseconds;                 
            }
            finally
            {
                startPrintTimer.Enabled = true;
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {

                timer2.Enabled = false;
                Controller.Stop();

                var endDateTime = DateTime.ParseExact(maskedTextBox2.Text, "HH.mm", null, System.Globalization.DateTimeStyles.None);
                var now = DateTime.Now;
                var endPrintTime = new DateTime(now.Year, now.Month, now.Day, endDateTime.Hour, endDateTime.Minute, 0);

                var nextEndPrintTime = endPrintTime.AddDays(1);
                var nextEnd = nextEndPrintTime - DateTime.Now;
                timer2.Interval = (int)nextEnd.TotalMilliseconds;
            }
            finally
            {
                timer2.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CurrentSearchResult != null && CurrentSearchResult.Any())
            {
                //SelectRow((int)profileOrderId);
                var item = CurrentSearchResult.First();
                SelectRow(item.Id);
                CurrentSearchResult.Remove(item);
            }
        }


            [DllImport("kernel32.dll")] static extern IntPtr GlobalLock(IntPtr hMem); 
	        [DllImport("kernel32.dll")] [return: MarshalAs(UnmanagedType.Bool)] static extern bool GlobalUnlock(IntPtr hMem); 
	        [DllImport("kernel32.dll")] static extern IntPtr GlobalFree(IntPtr hMem); 
	        [DllImport("winspool.Drv", EntryPoint = "DocumentPropertiesW", SetLastError = true,
                                      ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]  
	                                   static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter,
	                                   [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
	                                   IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode); 
	 
	        private const int DM_IN_BUFFER = 8; 
	        private const int DM_OUT_BUFFER = 2; 
	        private const int DM_IN_PROMPT = 4; 
 
	 
	        private void ShowPrinterProperties(PrinterSettings settings)
	        { 
	            IntPtr hDevMode = settings.GetHdevmode(settings.DefaultPageSettings); 
	 
	            IntPtr pDevMode = GlobalLock(hDevMode); 
	 
	            DocumentProperties(this.Handle, IntPtr.Zero, settings.PrinterName, pDevMode, pDevMode, DM_IN_PROMPT); 
	 
	            GlobalUnlock(hDevMode); 
	 
	            settings.SetHdevmode(hDevMode); 
	 
	            settings.DefaultPageSettings.SetHdevmode(hDevMode); 
	 
	            GlobalFree(hDevMode); 
	        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Controller.profile.AutoStartStop = checkBox1.Checked;
            Controller.db.Update(Controller.profile);
            Controller.db.SaveChanges();

            if (checkBox1.Checked)
                SetPrintTimer();

        }

        private void button5_Click(object sender, EventArgs e)
        {

            //GetPrintTicketFromPrinter(cmbPrinter.Text);

            var printer = new PrintDialog();
            if (Controller.profile.PresetId != null)
            {
                //var settings = Program.mainForm.LoadPrinterSettings("psetting",Controller.profile.PresetId.ToString());
                var settings = Program.mainForm.GetConfiguredPrinter((int)Controller.profile.PresetId, cmbPrinter.Text);
                if (settings != null)
                {
                    //settings.PrinterName = cmbPrinter.Text;
                    //printer.PrinterSettings.SetHdevmode(settings.GetHdevmode());
                    
                    
                    printer.PrinterSettings = settings;
                    
                    
                    //var mode = CustomprinterSettings.DEVMODE;
                    //Marshal.PtrToStructure(settings.GetHdevmode(), mode); // RetrieveVirtualItemEventArgs dm, yDevModeData, true);

                    //var mode = (CustomprinterSettings.DEVMODE)Marshal.PtrToStructure(settings.GetHdevmode(), typeof(CustomprinterSettings.DEVMODE));
                    //printer.PrinterSettings.SetHdevmode(settings.GetHdevmode());
                    //printer.PrinterSettings.SetHdevnames(settings.GetHdevnames());

                    //mode = (CustomprinterSettings.DEVMODE)Marshal.PtrToStructure(printer.PrinterSettings.GetHdevmode(), typeof(CustomprinterSettings.DEVMODE));

                    //printer.PrinterSettings.PrinterName = cmbPrinter.Text;
                    //mode = (CustomprinterSettings.DEVMODE)Marshal.PtrToStructure(printer.PrinterSettings.GetHdevmode(), typeof(CustomprinterSettings.DEVMODE));
                    //mode.dmMediaType = 240;
                    //IntPtr p = Marshal.AllocHGlobal(1000000);
                    //Marshal.StructureToPtr(mode, p, false);
                    //printer.PrinterSettings.SetHdevmode(p);

                }
                else
                    printer.PrinterSettings.PrinterName = cmbPrinter.Text;
                //printer.PrinterSettings = Program.mainForm.LoadPrinterSettings(Controller.profile.PresetId.ToString());
                //printer.PrinterSettings.PrinterName = cmbPrinter.Text;
            }
            printer.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(orderGridView.SelectedRows.Count == 0)
                return;

            var p = new SelectProfileForm();
            p.ExcludeName = Controller.profile.ProfileName;
            if(p.ShowDialog(this) == DialogResult.OK)
            {

                var db = Controller.db;
                ProfileOrder profileOrder;
                var pname = p.cmbProfiles.Text;
                foreach (DataGridViewRow moveItem in orderGridView.SelectedRows)
                {
                    profileOrder = (moveItem.DataBoundItem as PrintItemDataRow).ProfileOrder;
                    if (profileOrder.Order.OrderStatus == OrderStatus.Complete || profileOrder.PrintingStatus == PrintingStatus.Printing)
                        continue;
                    //var profileOrder = (orderGridView.Curr ataBoundItem as PrintItemDataRow).ProfileOrder;


                    //var profileOrder = (orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow).ProfileOrder;
                    db.ProfileOrders.Remove(profileOrder);
                    //profileOrder.Id = 0;
                    
                    var profile = db.PrintProfiles.AsNoTracking().SingleOrDefault(d => d.ProfileName == pname);
                    if (profile != null)
                    {
                        db.SaveChanges();

                        profileOrder.ProfileId = profile.Id;
                        profileOrder.Id = 0;
                        profileOrder.Order = null;
                        profileOrder.Profile = null;
                        //var porder = new ProfileOrder();
                        //porder.OrderId = profileOrder.OrderId;
                        //porder.PrintingOrder = profileOrder.PrintingOrder;
                        db.Add(profileOrder);
                        db.SaveChanges();
                    }
                }

                Controller.LoadProfileOrders();
                var destController = Program.mainForm.ProfileControllers.SingleOrDefault(d => d.profile.ProfileName == pname);
                destController.LoadProfileOrders();
                MessageBox.Show("Done!. The selected items has been moved to profile " + destController.profile.ProfileName);


            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            var f = new SelectPrintStatusForm();
            if (f.ShowDialog(this) == DialogResult.OK)
            {
                ProfileOrder profileOrder;

                var orderList = orderGridView.SelectedRows.OfType<DataGridViewRow>().Select(d => d.DataBoundItem).OfType<PrintItemDataRow>()
                                                    .Where(d => d.ProfileOrder.PrintingStatus != PrintingStatus.Printing)
                                                    .Select(d => d.ProfileOrder);

                if (f.comboBox1.Text == "Pending")
                {
                    Controller.ChangeToPending(orderList);
                    // Controller.LoadProfileOrders();
                }
                else if (f.comboBox1.Text == "Paused")
                {
                    Controller.ChangeToPaused(orderList);
                    //orderGridView.Refresh();
                }
                else if (f.comboBox1.Text == "Complete")
                {
                    Controller.ChangeToComplete(orderList);
                }


                //foreach (DataGridViewRow item in orderGridView.SelectedRows)
                //{
                //    profileOrder = (item.DataBoundItem as PrintItemDataRow).ProfileOrder;
                //    if (profileOrder.PrintingStatus == PrintingStatus.Printing)
                //        continue;

                //    //var profileOrder = (orderGridView.CurrentRow.DataBoundItem as PrintItemDataRow).ProfileOrder;


                //    if (f.comboBox1.Text == "Pending")
                //    {
                //        Controller.ChangeToPending(profileOrder);
                //       // Controller.LoadProfileOrders();
                //    }
                //    else if (f.comboBox1.Text == "Paused")
                //    {
                //        Controller.ChangeToPaused(profileOrder);
                //        //orderGridView.Refresh();
                //    }
                //    else if (f.comboBox1.Text == "Complete")
                //    {
                //        Controller.ChangeToComplete(profileOrder);                        
                //    }
                //}

                //Controller.LoadProfileOrders();

                //orderGridView.Refresh();

            }
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            if(MessageBox.Show("Please confirm.","Kill current print job", MessageBoxButtons.OKCancel)== DialogResult.OK)
                Controller.Stop(true);
        }

        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            //
            if (Program.mainForm.tabControl1.TabPages.Count > 0)
            {
                //Program.mainForm.tabControl1.TabPages[0].Invalidate();
                var s = new Size();
                s.Width = Program.mainForm.tabControl1.ItemSize.Width + 1;
                s.Height = Program.mainForm.tabControl1.ItemSize.Height;
                Program.mainForm.tabControl1.ItemSize = s; //.Width = 71;
                s = new Size();
                s.Width = Program.mainForm.tabControl1.ItemSize.Width - 1;
                s.Height = Program.mainForm.tabControl1.ItemSize.Height;
                Program.mainForm.tabControl1.ItemSize = s; //.Width = 71;
                //Application.DoEvents();
                //Program.mainForm.Invalidate();
                //Application.DoEvents();
            }
        }

        private void cmbPrintRun_SelectedIndexChanged(object sender, EventArgs e)
        {
            var i = 0;
        }

        private void cmbPrintRun_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Program.mainForm.config.PrintRun = cmbPrintRu
            if (cmbPrintRun.SelectedItem != null)
            {
                dynamic x = cmbPrintRun.SelectedItem;
                Controller.profile.PrintRun = x.Value;
                Controller.db.SaveChanges();
            }

        }

        private void cmbPrintRun_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void BtnDeleteProfile_Click(object sender, EventArgs e)
        {
            Program.mainForm.DeleteProfile(txtProfileName.Text);
        }

        private void BtnEditFilter_Click(object sender, EventArgs e)
        {
            var f = new FilterForm();
            f.LoadData(Controller.profile.ProfileFilters.ToList());
            f.LoadLinkedSKU(Controller.profile.IgnoreLinkingTexts.ToList());
            if (f.ShowDialog() == DialogResult.OK)
            {
                var filterData = f.GetFilterData();
                Controller.SetFilterData(filterData);
                Controller.SetLinkedFilters(f.GetLinkedSKUFilters());
                //Controller.db.Update(Controller.profile);
                //Controller.db.SaveChanges();
            }
        }

        private void CmbPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Controller.CurrPage != Convert.ToInt32(cmbPageSelector.Text))
            {
                //Controller.CurrPage = Convert.ToInt32(cmbPageSelector.Text);
                var page = Convert.ToInt32(cmbPageSelector.Text);
                Controller.LoadProfileOrders(page);
            }
        }
    }

    class PrintRunData
    {
        public string PrintRunName { get; set; }
        public int PendingCount { get; set; }
    }


    public class PrintItemDataRow
    {
        public int Quantity { get; set; }
        public string SKU { get; set; }
        public string Linked { get; set; }
        public string PrintingStatus { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public string CustomerName { get; set; }
        public string OrderNotes { get; set; }
        public string CustomerEmail { get; set; }
        public int PrintingOrder { get; set; }
        public int Id { get; set; }
        public ProfileOrder ProfileOrder { get; set; }
        public Order order { get; set; }
        public string LinkedFileName { get; set; }
        public string Reprint { get; set; } = "Reprint";
        public string Remark { get; set; }
        public string OrderId { get; set; }
        public string ShortTitle { get; set; }
        public string PrintRun { get; set; }

    }

}
