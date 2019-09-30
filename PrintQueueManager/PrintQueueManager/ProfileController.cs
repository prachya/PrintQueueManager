using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace PrintQueueManager
{
    using Data;
    using Print;
    public class ProfileController
    {
        public enum ProfileStatus { Started, Stopped,Stopping }
        public delegate void PrintingFinishedHandler(object sender, ProfileOrder profileOrder);
        public delegate void PrintItemStatusChanged(object sender, ProfileOrder profileOrder, Data.PrintingStatus status);

        public event PrintingFinishedHandler OnPrintingFinished;
        public event PrintItemStatusChanged OnPrintItemStatusChanged;

        public PrintProfile profile { get; set; }
        public ProfileStatus Status { get; set; } = ProfileStatus.Stopped;
        public MainForm mainForm { get; set; }
        public dataContext db { get; set; }
        public PrintController printController { get; set; } = new PrintController();
        public ProfileController(MainForm main, dataContext db, PrintProfile profile)
        {
            mainForm = main;
            this.db = db;
            this.profile = profile;
            printController.OnPrintingStatusChanged += PrintController_OnPrintingStatusChanged;
            //printController.PrinterName = "Canon Inkjet 0278 Class Driver";

            MakePrintingsToPendings();
        }

        public void SetFilterData(List<Filter> filterData)
        {
            //profile.ProfileFilters.Clear();
            //foreach (var item in profile.ProfileFilters)
            //{
            //    db.Remove(item);
            //    db.SaveChanges();
            //}
            

            profile.ProfileFilters = filterData;
            db.Update(profile); //.ProfileFilters);
            db.SaveChanges();
        }

        public void SetLinkedFilters(List<IgnoreLinkingText> ignoredTextList)
        {
            profile.IgnoreLinkingTexts = ignoredTextList;
            db.Update(profile); //.ProfileFilters);
            db.SaveChanges();
        }

        private void MakePrintingsToPendings()
        {
            foreach(var item in profile.ProfileOrders)
            {
                item.PrintingStatus = (item.Order.OrderStatus == OrderStatus.Pending) || 
                                      (item.PrintingStatus == Data.PrintingStatus.Printing) ? Data.PrintingStatus.Pending : item.PrintingStatus;
                db.Update(item);                
            }
            db.SaveChanges();
        }

        private async void PrintController_OnPrintingStatusChanged(object sender, Print.PrintingStatus status)
        {

            //if(this.Status == ProfileStatus.Started)
            //{
            //    if (status == Print.PrintingStatus.Idle)
            //        await PrintNext();
            //    else if(status == Print.PrintingStatus.Printing)
            //    {

            //    }

            //}

            //if (status == Print.PrintingStatus.Idle)
            //
        }

        public ProfileFrame frame { get; set; }
        private List<PrintItemDataRow> CurrentList;

        public void DeleteFromDatabase()
        {
            
            db.PrintProfiles.Remove(profile);               
            db.SaveChanges();
        }

        public void Show()
        {
            frame = new ProfileFrame();
            frame.txtProfileName.Text = profile.ProfileName;
            frame.Controller = this;

            mainForm.AddNewTab(frame);

            //CreateCurrentList();
            //frame.LoadProfileOrders(CurrentList);
            LoadProfileOrders();

        }

        public List<PrintItemDataRow> Search(string keyword)
        {
            DateTime date;
            List<PrintItemDataRow> result = null;
            if (DateTime.TryParse(keyword, out date))
            {
                var rec = CurrentList.Where(d => d.TimeOfOrder.Day == date.Day
                                                           && d.TimeOfOrder.Month == date.Month
                                                           && d.TimeOfOrder.Year == date.Year);
                if (rec != null)
                {
                    result = rec.ToList();
                }

            }
            else {
                
                var rec = CurrentList.Where(d => d.SKU == keyword); //.Contains(keyword));
                rec = !rec.Any() ? CurrentList.Where(d => d.CustomerName == keyword) : rec;
                rec = !rec.Any() ? CurrentList.Where(d => d.CustomerName.ContainsWord(keyword) ) : rec;
                rec = !rec.Any() ? CurrentList.Where(d => String.Equals(d.CustomerEmail,keyword,StringComparison.CurrentCultureIgnoreCase)): rec;
                result = rec.ToList();
            }

            return result;
            

        }

        private void CreateCurrentList()
        {
            CurrentList = db.ProfileOrders.Where(d => d.ProfileId == profile.Id).Select(d => new PrintItemDataRow
            {
                Quantity = d.Order.Quantity,
                SKU = d.Order.Sku,
                Linked = d.Order.IsLinked ? "Yes" : "--",
                PrintingStatus = d.Order.OrderStatus == OrderStatus.Pending ? "Pending" : d.PrintingStatus.ToString(),
                TimeOfOrder = d.Order.ReceivedDate,
                CustomerName = d.Order.CustomerName,
                OrderNotes = d.Order.OrderNotes,
                CustomerEmail = d.Order.CustomerEmail,
                PrintingOrder = d.PrintingOrder,
                LinkedFileName = d.Order.LinkedFilename,
                ShortTitle = d.Order.ShortTitle,
                Remark = d.Order.Remark,
                Id = d.OrderId,
                PrintRun = d.Order.PrintRun,
                ProfileOrder = d

            }).OrderByDescending(d => d.PrintingOrder).ThenByDescending(d => d.TimeOfOrder).ToList();

        }

        public void OrderUP(ProfileOrder profileOrder)
        {
            if (profileOrder.PrintingOrder == 0)
                return;

            var upIdx = CurrentList.FindIndex(d => d.Id == profileOrder.OrderId);
            if (upIdx == 0)
                return;

            var downOrder = CurrentList[upIdx - 1];
            var upPrintingOrder = profileOrder.PrintingOrder;
            profileOrder.PrintingOrder = downOrder.PrintingOrder;
            downOrder.ProfileOrder.PrintingOrder = upPrintingOrder;

            //db.Update(downOrder.ProfileOrder);
            db.Update(profileOrder);
            db.SaveChanges();

            LoadProfileOrders();

        }

        private int GetNextLastPrintingOrder()
        {
            
            var lst = CurrentList.Where(d => d.ProfileOrder.PrintingStatus == Data.PrintingStatus.Pending);
            int min = 0;
            if (lst.Any())
            {
                min = lst.Min(r => r.ProfileOrder.PrintingOrder);
                --min;
            }
            else
                min = 100;

            return min;

        }

        public void ChangeToPaused(IEnumerable<ProfileOrder> profileOrders)
        {
            foreach(var profileOrder in profileOrders)
            {
                profileOrder.Order.OrderStatus = OrderStatus.Paused;
                profileOrder.PrintingStatus = Data.PrintingStatus.Paused;
                //db.Update(profileOrder.Order);
                db.Update(profileOrder);
            }            
            db.SaveChanges();
        }

        public void ChangeToComplete(IEnumerable<ProfileOrder> profileOrders)
        {
            foreach (var profileOrder in profileOrders)
            {
                profileOrder.Order.OrderStatus = OrderStatus.Complete;
                profileOrder.PrintingStatus = Data.PrintingStatus.Complete;
                profileOrder.PrintingOrder = 0;
                //profileOrder.Order.CompletedDateTime = 
                //db.Update(profileOrder.Order);
                db.Update(profileOrder);
            }
            db.SaveChanges();
        }

        public void ChangeToPending(IEnumerable<ProfileOrder> profileOrders)
        {
            //var pendingList = db.ProfileOrders.Where(d => d.ProfileId == profile.Id
            //                                                        && (d.PrintingStatus == Data.PrintingStatus.Pending
            //                                                              || d.PrintingStatus == Data.PrintingStatus.Printing));

            //var wasComplete = profileOrder.Order.OrderStatus == OrderStatus.Complete;
            

            foreach (var profileOrder in profileOrders)
            {
                if (profileOrder.Order.OrderStatus == OrderStatus.Complete)
                {
                    profileOrder.PrintingOrder = GetNextLastPrintingOrder();
                }
                profileOrder.Order.OrderStatus = OrderStatus.Pending;
                profileOrder.PrintingStatus = Data.PrintingStatus.Pending;
                db.Update(profileOrder.Order);
                db.Update(profileOrder);
            }
            db.SaveChanges();
            

            LoadProfileOrders();

        }

        public void OrderDown(ProfileOrder profileOrder)
        {
            if (profileOrder.PrintingOrder == 0)
                return;

            var downIdx = CurrentList.FindIndex(d => d.Id == profileOrder.OrderId);
            if (downIdx == CurrentList.Count - 1)
                return;

            var upOrder = CurrentList[downIdx + 1];
            var downPrintingOrder = profileOrder.PrintingOrder;
            profileOrder.PrintingOrder = upOrder.PrintingOrder;
            upOrder.ProfileOrder.PrintingOrder = downPrintingOrder;

            db.Update(upOrder.ProfileOrder);
            db.Update(profileOrder);
            db.SaveChanges();

            LoadProfileOrders();

        }

        public void LoadProfileOrders()
        {
            // recreate current list.
            CreateCurrentList();
            frame.LoadProfileOrders(CurrentList);
        }

        public int GetNoOfPages()
        {
            var c = profile.ProfileOrders.Count / 100;
            if (c == 0)
                c = 1;
            else
            {
                if ((profile.ProfileOrders.Count % 100) != 0)
                    c++;
            }
            return c;
        }

        public static string RemoveIgnoreText(PrintProfile profile,string sku)
        {
            var res = sku;
            foreach (var item in profile.IgnoreLinkingTexts)
            {
                //if (sku.StartsWith(item.IgnoreText))
                if(res.Contains(item.IgnoreText))
                {
                    var startIdx = sku.IndexOf(item.IgnoreText);
                    res = res.Remove(startIdx,item.IgnoreText.Length);                   
                }
            }
            return res;
        }

        public ProfileOrder Remove(int ProfileOrderId)
        {
            var obj = db.ProfileOrders.SingleOrDefault(d => d.Id == ProfileOrderId);
            if (obj != null)
            {
                db.Remove(obj);
                db.SaveChanges();
                LoadProfileOrders();
                return obj;
            }
            else
                return null;            
        }

        public bool Add(ProfileOrder profileOrder)
        {
            var obj = db.ProfileOrders.SingleOrDefault(d => d.Id == profileOrder.Id);
            if (obj != null)
            {
                obj.ProfileId = profile.Id;

                var pendingList = db.ProfileOrders.Where(d => d.ProfileId == profile.Id
                                                                                    && (d.PrintingStatus == Data.PrintingStatus.Pending
                                                                                          || d.PrintingStatus == Data.PrintingStatus.Printing));
                var lastPrintingOrder = 0;
                if (pendingList.Any())
                    lastPrintingOrder = pendingList.Max(d => d.PrintingOrder);
                
                obj.PrintingOrder = ++lastPrintingOrder;
                db.Update(obj);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public async void Start()
        {
            if (Status == ProfileStatus.Started)
                return;

            Stopping = false;
            Status = ProfileStatus.Started;
            frame.lblStatus.Text = "Started";

            RefreshLinkedFiles();

            frame.lblStatus.ForeColor = System.Drawing.Color.Green;

            CalcETA();

            while (true)
                try
                {
                    if (await PrintNext() == false || Status == ProfileStatus.Stopped)
                        break;

                    await Task.Factory.StartNew(() =>
                    {
                        System.Threading.Thread.Sleep(1000);
                    });
                }
                catch (Exception ex)
                {
                    mainForm.log.AddMessage("Error " + ex.Message);
                    continue;
                }

            

          
        }

        private void CalcETA()
        {
            // speed is minutes.
            //
            var speed = mainForm.GetPrinterSpeed(profile.SelectedPrinter);
            if (speed == 0)
                return;

            int ETAminutes = 0;

            foreach (var item in CurrentList.Where(d=>d.ProfileOrder.Order.IsLinked && d.PrintingStatus == "Pending"))
            {
                
                var filePath = "";
                if (!String.IsNullOrEmpty(item.LinkedFileName))
                    filePath = item.LinkedFileName;
                else
                    filePath = mainForm.config.PDFFolder + item.SKU + ".pdf";

                var noOfPages = mainForm.GetPDFNoOfpages(filePath);
                if (noOfPages != 0)
                    ETAminutes += (noOfPages * speed);
            }

            var hrs = (ETAminutes / 60.0);
            frame.lblLeft.Text = hrs.ToString("#.##") + " hrs."; // change to Hours.
        }

        public bool Stopping { get; set; }

        public async void Stop(bool kill = false)
        {
            if (Stopping || Status == ProfileStatus.Stopped)
                return;

            if (!kill)
            {
                if (CurrentList.Where(d => d.ProfileOrder.Order.IsLinked && d.ProfileOrder.PrintingStatus == Data.PrintingStatus.Pending).Count() == 0)
                {
                    //mainForm.log.AddMessage("Printing... " + filePath + " Stopped");

                    MakePrintingsToPendings();

                    Stopping = false;
                    this.Status = ProfileStatus.Stopped;
                    frame.lblStatus.Text = "Stopped";
                    frame.lblStatus.ForeColor = System.Drawing.Color.Red;
                }
                else
                    Stopping = true;
            }
            else
            { 
                await printController.Stop();
                //MakePrintingsToPendings();

                //Stopping = false;
                //Status = ProfileStatus.Stopping;
                //this.Status = ProfileStatus.Stopped;
                //frame.lblStatus.Text = "Stopped";
                //frame.lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
      
        public void SetReprint(ProfileOrder profileOrder,int qty)
        {
            profileOrder.Order.ReprintedQuantity = qty; // Int32.Parse(reprint.txtQty.Text);

            
            
            profileOrder.PrintingOrder = GetNextLastPrintingOrder();
            profileOrder.Order.OrderStatus = OrderStatus.Pending;
            profileOrder.Order.Remark = profileOrder.Order.Remark + " Reprint(" + profileOrder.Order.ReprintedQuantity + ")";
            profileOrder.PrintingStatus = Data.PrintingStatus.Pending;
            
            // var downIdx = CurrentList.FindIndex(d => d.Id == profileOrder.OrderId);


            db.Update(profileOrder);
            db.Update(profileOrder.Order);
            db.SaveChanges();


            LoadProfileOrders();

        }

        public void SetPrinter(string printerName)
        {
            printController.PrinterName = printerName;
            profile.SelectedPrinter = printerName;
            db.Update(profile);
            db.SaveChanges();
            
        }

        public void RefreshLinkedFiles()
        {
            var lst = db.ProfileOrders.Include(d=>d.Order).Where(d => d.ProfileId == profile.Id && d.Order.OrderStatus != OrderStatus.Complete);
            foreach(var item in lst)
            {
                item.Order.Linked = System.IO.File.Exists(mainForm.config.PDFFolder + ProfileController.RemoveIgnoreText(profile,item.Order.Sku) + ".pdf");
                db.Update(item.Order);                
            }
            db.SaveChanges();
            LoadProfileOrders();     
        }
      
        public static IEnumerable<Order> GetOrdersInProfile(PrintProfile profile,IEnumerable<Order> orders)
        {
            IList<Order> inProfileList = new List<Order>();
            foreach (var order in orders)
            {
                bool orderInProfile = false;
                foreach (var item in profile.ProfileFilters)
                {
                    if (!string.IsNullOrEmpty(item.StartsWith))
                    {
                        orderInProfile = (order.Sku.StartsWith(item.StartsWith));
                        if (orderInProfile)
                            break;
                    }

                    if (!String.IsNullOrEmpty( item.EndsWith))
                    {
                        orderInProfile = (order.Sku.EndsWith(item.EndsWith));
                        if (orderInProfile)
                            break;
                    }

                    if (!string.IsNullOrEmpty(item.Contains))
                    {
                        orderInProfile = order.Sku.Contains(item.Contains);
                        if (orderInProfile)
                            break;
                    }
                    else if(!string.IsNullOrEmpty(item.DoesNotContain))
                    {
                        orderInProfile = !order.Sku.Contains(item.DoesNotContain);
                        if (orderInProfile)
                            break;
                    }
                }
                if (orderInProfile==true)
                    inProfileList.Add(order);
            }

            return inProfileList;   
        }
        private async Task<bool> PrintNext()
        {
            //var printItem = CurrentList.Where(d => d.ProfileOrder.PrintingStatus == Data.PrintingStatus.Pending).LastOrDefault();
            PrintItemDataRow printItem = null;
            if (this.profile.PrintRun == "Run All")
            {
                printItem = CurrentList.Where(d => d.ProfileOrder.Order.IsLinked &&
                                                   d.ProfileOrder.PrintingStatus == Data.PrintingStatus.Pending).OrderBy(d => d.TimeOfOrder).FirstOrDefault(); //.LastOrDefault();
            }
            else
            {
                printItem = CurrentList.Where(d => d.ProfileOrder.Order.IsLinked &&
                                                   d.ProfileOrder.PrintingStatus == Data.PrintingStatus.Pending &&
                                                   d.ProfileOrder.Order.PrintRun == this.profile.PrintRun).OrderBy(d => d.TimeOfOrder).FirstOrDefault(); //.LastOrDefault();
            }

            if (printItem != null)
            {
                try
                {               
                    //var filePath = @"c:\WLP3_Manual_EN_Innen_print.pdf"; // TestManage.zip";
                    var filePath = "";
                    if (!String.IsNullOrEmpty(printItem.LinkedFileName))
                    {
                        filePath = printItem.LinkedFileName;
                    }
                    else
                    {
                        //if( (printItem.SKU.StartsWith("AMZ_")) ||
                        //    (printItem.SKU.StartsWith("EBY_")) ||
                        //    (printItem.SKU.StartsWith("SHP_")) ||
                        //    (printItem.SKU.StartsWith("ETS_")) ||
                        //    (printItem.SKU.StartsWith("EB2_")) ||
                        //    (printItem.SKU.StartsWith("AM2_")) ||
                        //    (printItem.SKU.StartsWith("SH2_")) ||
                        //    (printItem.SKU.StartsWith("ET2_"))  )
                        //{
                        //    filePath = mainForm.config.PDFFolder + printItem.SKU.Remove(0,4) + ".pdf";
                        //}
                        //else

                        filePath = mainForm.config.PDFFolder + ProfileController.RemoveIgnoreText(profile, printItem.SKU) + ".pdf";
                    }

                    

                    if (!printItem.ProfileOrder.Order.IsLinked)
                    {
                        Program.mainForm.log.AddMessage(printItem.SKU + " - " + printItem.ProfileOrder.Order.CustomerEmail + " not printed");
                        return true;
                    }

                    printItem.ProfileOrder.StaredPrintingTime = DateTime.Now;
                    OnPrintItemStatusChanged?.Invoke(this, printItem.ProfileOrder, Data.PrintingStatus.Printing);
                    // if (!await printController.Send2(filePath))
                    System.Drawing.Printing.PrinterSettings settings = null;
                    if (profile.PresetId != null)
                    {
                        var cp = db.PresetPrinters.SingleOrDefault(d => d.PresetId == profile.PresetId && d.PrinterName == profile.SelectedPrinter);
                        if(cp != null)
                            settings = Program.mainForm.LoadPrinterSettings("cfgprinter", cp.Id.ToString());
                    }

                    short printCopies = (short)printItem.ProfileOrder.Order.Quantity;
                    if (printItem.ProfileOrder.Order.ReprintedQuantity != null)
                        printCopies = (short)printItem.ProfileOrder.Order.ReprintedQuantity;
                    mainForm.log.AddMessage("Printing... " + filePath);
                    // ************************************************************************
                    // Send file to printer now.
                    // ************************************************************************
                    var starttime = DateTime.Now;
                    if (!await printController.Send3(filePath,settings,printCopies))
                    {
                        mainForm.log.AddMessage("Failed printing file: " + filePath);
                        OnPrintItemStatusChanged?.Invoke(this, printItem.ProfileOrder, Data.PrintingStatus.Printing);
                        return false;
                    }

                    if (printController.Aborted)
                    {
                        mainForm.log.AddMessage("Printing... " + filePath + " Aborted");
                        MakePrintingsToPendings();
                        Stopping = false;
                        this.Status = ProfileStatus.Stopped;
                        frame.lblStatus.Text = "Stopped";
                        frame.lblStatus.ForeColor = System.Drawing.Color.Red;
                        LoadProfileOrders();
                        return false;
                    }

                    //if (!printController.Aborted)
                    //{
                    printItem.ProfileOrder.CompletedPrintingTime = DateTime.Now;
                    // update item.
                    printItem.ProfileOrder.PrintingStatus = Data.PrintingStatus.Complete;
                    printItem.PrintingOrder = 0;
                    printItem.ProfileOrder.PrintingOrder = 0;
                    printItem.ProfileOrder.Order.OrderStatus = OrderStatus.Complete;
                    printItem.ProfileOrder.Order.CompletedDateTime = DateTime.Now;
                    var elapsedTime = DateTime.Now - starttime;
                    mainForm.CalibratePrinter(filePath, settings, printCopies, profile.PresetId,elapsedTime.TotalMinutes);

                    //db.Update(profile);
                    db.Update(printItem.ProfileOrder);
                    db.SaveChanges();

                    // fire event.
                    OnPrintItemStatusChanged?.Invoke(this, printItem.ProfileOrder, printItem.ProfileOrder.PrintingStatus);
                    mainForm.log.AddMessage("Printing... " + filePath + " Completed");
                    if( Stopping )
                    {
                        mainForm.log.AddMessage("Printing... " + filePath + " Stopped");
                        MakePrintingsToPendings();
                        Stopping = false;                      
                        this.Status = ProfileStatus.Stopped;
                        frame.lblStatus.Text = "Stopped";
                        frame.lblStatus.ForeColor = System.Drawing.Color.Red;
                        return false;
                    }

                    LoadProfileOrders();

                    // done.
                    return true;
                
                }
                catch (Exception ex)
                {
                    mainForm.log.AddMessage(ex.Message);
                    OnPrintItemStatusChanged?.Invoke(this, printItem.ProfileOrder, Data.PrintingStatus.Pending); // reset the Pending if there's a problem.
                    return false;

                }
            }
            else
            {
                mainForm.log.AddMessage("No more items to print - waiting for more orders..." + profile.ProfileName);
                //Stop();
                return true;
            }
                

            //foreach(var item in profile.ProfileOrders.First())
            //var profileOrder = profile.ProfileOrders.First();
            //OnPrintingFinished?.Invoke(this, profileOrder);
        }

    }

    static class StringExtension
    {
        public static bool ContainsWord(this string s, string word)
        {
            string[] ar = s.Split(' ');

            foreach (string str in ar)
            {
                if (str.ToLower() == word.ToLower())
                    return true;
            }
            return false;
        }
    }

}
