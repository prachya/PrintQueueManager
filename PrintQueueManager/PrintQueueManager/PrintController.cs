using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Printing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
//using PdfSharp.Pdf.Printing;
using System.Runtime.InteropServices;
//using EvoPdf.PdfPrint;
using TallComponents.PDF.Rasterizer;
using System.Drawing.Drawing2D;

namespace PrintQueueManager.Print
{
    using Data;

    public enum PrintingStatus { Printing, Idle, Initialized, Aborted}
    public enum PrintingStatusDetail { Offline, PaperOut, Paused , Unknown}
    public delegate void PrintingStatusChangeHandler(object sender, PrintingStatus status);
    public delegate void PrinterStatusInfoHandler(object sender, string info);
    public delegate void PrinterPrintProgressHandler(object sender, string info);

    public class PrintController
    {
        public event PrintingStatusChangeHandler OnPrintingStatusChanged;
        public event PrinterStatusInfoHandler OnPrinterStatusInfo;
        public event PrinterPrintProgressHandler OnPrinterPrintProgress;
        public string PrinterName { get; set; }
        private Timer JobMonitorTimer;
        private PrintingStatus status { get; set; } = PrintingStatus.Idle;
        private PrintingStatusDetail printingStatusDetail { get; set; } = PrintingStatusDetail.Unknown;
        public PrintController()
        {
                     
        }

        void StartJobMonitor2()
        {
            //JobMonitorTimer = new Timer()
            JobMonitorTimer = new Timer(
                        new TimerCallback(TickTimer),
                        null,
                        2000,
                        1000);
        }

        void TickTimer(object state)
        {
            //Console.Write("Tick! ");
            //Console.WriteLine(
            //    Thread.CurrentThread.
            //    ManagedThreadId.ToString());
            //Thread.Sleep(4000);

            string part2 = "";
            string part1 = "";
            PrintQueue q = null;
            if (IsNetworkedPrinter(PrinterName, out part1, out part2))
            {
                var ps = new PrintServer(part1);
                q = ps.GetPrintQueue(part2);
            }
            else
            {
                var printerServer = new LocalPrintServer(); // new PrintServer(PrinterName);
                q = printerServer.GetPrintQueue(PrinterName);
            }

            q.Refresh();
            var qs = q.QueueStatus;
            var jobs = q.GetPrintJobInfoCollection();

            if (qs == PrintQueueStatus.Paused)  // print queue of the printer is paused.
            {
                FirePrinterStatusInfo("Print queue paused " + q.Description);

                if (jobs.Count() == 0)      // even Printer paused, current print job still printing so need to check if the job being printed.
                                            // if finished.
                {
                    if (status != PrintingStatus.Initialized)
                    {
                        ChangeStatus(PrintingStatus.Idle);
                    }
                    else
                    {
                        printingStatusDetail = PrintingStatusDetail.Paused;
                        ChangeStatus(PrintingStatus.Printing);
                    }
                }
                else
                {
                    printingStatusDetail = PrintingStatusDetail.Paused;
                    ChangeStatus(PrintingStatus.Printing);
                }
            }
            else
            {
                //Debug.Print(q.QueueStatus.ToString());
                //if (q.IsPrinting)
                //{
                //    Debug.Print("Printing");
                //}
                //var jobs = q.GetPrintJobInfoCollection();
                if (jobs.Count() > 0)
                {
                    //FirePrinterStatusInfo("Job count on queue " + q.Description + " " + jobs.Count().ToString());
                    var job = jobs.FirstOrDefault();
                    if (job.JobStatus == PrintJobStatus.Offline)
                    {
                        FirePrinterStatusInfo("Job status Offline " + q.Description + " " + job.JobName);
                        printingStatusDetail = PrintingStatusDetail.Offline;
                    }
                    else if (job.JobStatus == PrintJobStatus.PaperOut)
                    {
                        FirePrinterStatusInfo("Job status Paperout " + q.Description + " " + job.JobName);
                        printingStatusDetail = PrintingStatusDetail.PaperOut;
                    }
                    else if (job.JobStatus == PrintJobStatus.Paused)
                    {
                        FirePrinterStatusInfo("Job status Paused " + q.Description + " " + job.JobName);
                        printingStatusDetail = PrintingStatusDetail.Paused;
                    }
                    else if (job.JobStatus == PrintJobStatus.Printing)
                    {
                        FirePrinterStatusInfo("Printer's job status [printing] " + q.Description + " " + job.JobName);
                        //printingStatusDetail = PrintingStatusDetail.Paused;
                    }
                    else if (job.JobStatus == PrintJobStatus.Completed)
                        FirePrinterStatusInfo(job.JobName + " Completed, " + job.JobStatus.ToString() + " " + q.Description + " ");
                    else
                    {
                        job.Refresh();
                        FirePrinterStatusInfo("Job status " + job.JobStatus.ToString()
                                                    + " " + q.Description
                                                    + " " + job.Name);         
#if DEBUG
                        Debug.Print(job.JobStatus.ToString());
#endif
                    }

                    FirePrinterPrintProgress("Page: " + job.NumberOfPages.ToString()
                                                      + "    " + job.JobStatus.ToString()
                                                      + " job: " + job.Name + " on " + q.Name);
                    //job.NumberOfPagesPrinted
                    //foreach (var job in jobs)
                    //{
                    //    Debug.Print(job.JobStatus.ToString());
                    //}

                    ChangeStatus(PrintingStatus.Printing);
                }
                else
                {
                    if (status != PrintingStatus.Initialized)
                        ChangeStatus(PrintingStatus.Idle);
                }
            }

            bool IsNetworkedPrinter(string name, out string part11, out string part22)
            {
                bool result = false;
                part22 = "";
                part11 = "";
                string root = Path.GetPathRoot(name);

                // Check if root starts with "\\", clearly an UNC
                if (root.StartsWith(@"\\"))
                    result = true;

                if (result)
                {
                    var c = name.IndexOf(@"\", 2);
                    part22 = new string(name.Skip(c + 1).ToArray());
                    part11 = new string(name.Take(c).ToArray());
                }

                return result;
            }

        }


        private void ChangeStatus(PrintingStatus status)
        {
            //if (status == PrintingStatus.Idle)
            //    terminated = false;
            this.status = status;
            OnPrintingStatusChanged?.Invoke(this, this.status);
        }

        private void FirePrinterStatusInfo(string info)
        {
            OnPrinterStatusInfo?.Invoke(this, info);
        }

        private void FirePrinterPrintProgress(string progresstext)
        {
            OnPrinterPrintProgress?.Invoke(this, progresstext);
        }

        bool terminated;
        public bool Aborted { get; set; }
        public async Task Stop()
        {
            terminated = true;
            await Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (status == PrintingStatus.Aborted)
                    {                        
                        break;
                    }
                    Thread.Sleep(1000);
                }

                ChangeStatus(PrintingStatus.Idle);
                terminated = false;
                Aborted = true;
            });
        }

        public void StartJobMonitor()
        {
            Task.Factory.StartNew(() =>
            {
                string part2 = "";
                string part1 = "";
                PrintQueue q = null;
                if (IsNetworkedPrinter(PrinterName,out part1,out part2))
                {
                    var ps = new PrintServer(part1);
                    q = ps.GetPrintQueue(part2);                    
                }
                else
                {
                    var printerServer = new LocalPrintServer(); // new PrintServer(PrinterName);
                    q = printerServer.GetPrintQueue(PrinterName);
                }

                //var printerServer = new PrintServer(@"\\acer-dev"); // PrinterName);
                //q = printerServer.GetPrintQueue(PrinterName);

                //var t = q.DefaultPrintTicket;
                //t.PageMediaType = PageMediaType.PhotographicGlossy;
                //var c = q.GetPrintCapabilities();
                //var stream = new FileStream(@"c:\temp\f.xml", FileMode.CreateNew);
                //t.SaveTo(stream);
                //stream.Close();

                // var q = printerServer.GetPrintQueue("Canon iP2700 series"); // PrinterName); // new[] { EnumeratedPrintQueueTypes.Local }); //  PrinterName); // Microsoft XPS Document Writer");
                //var q = printerServer.GetPrintQueues(); // .GetPrintQueue(PrinterName); // new[] { EnumeratedPrintQueueTypes.Local }); //  PrinterName); // Microsoft XPS Document Writer");
                                                                  //var o = q.IsOffline;
                                                                  //var n = q.FullName;
                //var c = q.GetPrintCapabilities();


                //foreach(var item in q)
                //{
                //    item.Refresh();
                //    Debug.Print(item.QueueStatus.ToString());

                //    //var j = item.GetPrintJobInfoCollection();
                //    //foreach(var job in j)
                //    //{
                //    //    j
                //    //}
                //}

                while (true)
                {                   
                    q.Refresh();
                    var qs = q.QueueStatus;                   
                    var jobs = q.GetPrintJobInfoCollection();                    
                    if (terminated)
                    {
                        foreach (var pj in jobs)
                            pj.Cancel();
                        status = PrintingStatus.Aborted;
                        break;
                    }

                    if (qs == PrintQueueStatus.Paused)  // print queue of the printer is paused.
                    {
                        FirePrinterStatusInfo("Print queue paused " + q.Description);

                        if (jobs.Count() == 0)   // even Printer paused, current print job still printing so need to check if the job being printed.
                                                 // if finished.
                        {
                            if (status != PrintingStatus.Initialized)
                            {
                                ChangeStatus(PrintingStatus.Idle);
                            }
                            else
                            {
                                printingStatusDetail = PrintingStatusDetail.Paused;
                                ChangeStatus(PrintingStatus.Printing);
                            }
                        }
                        else
                        {
                            printingStatusDetail = PrintingStatusDetail.Paused;
                            ChangeStatus(PrintingStatus.Printing);
                        }
                    }
                    else
                    {
                        //Debug.Print(q.QueueStatus.ToString());
                        //if (q.IsPrinting)
                        //{
                        //    Debug.Print("Printing");
                        //}
                        //var jobs = q.GetPrintJobInfoCollection();
                        if (jobs.Count() > 0)
                        {
                            //FirePrinterStatusInfo("Job count on queue " + q.Description + " " + jobs.Count().ToString());
                            var job = jobs.FirstOrDefault();
                            if (job.JobStatus == PrintJobStatus.Offline)
                            {
                                FirePrinterStatusInfo("Job status Offline " + q.Description + " " + job.JobName);
                                printingStatusDetail = PrintingStatusDetail.Offline;
                            }
                            else if (job.JobStatus == PrintJobStatus.PaperOut)
                            {
                                FirePrinterStatusInfo("Job status Paperout " + q.Description + " " + job.JobName);
                                printingStatusDetail = PrintingStatusDetail.PaperOut;
                            }
                            else if (job.JobStatus == PrintJobStatus.Paused)
                            {
                                FirePrinterStatusInfo("Job status Paused " + q.Description + " " + job.JobName);
                                printingStatusDetail = PrintingStatusDetail.Paused;
                            }
                            else if (job.JobStatus == PrintJobStatus.Printing)
                            {
                                FirePrinterStatusInfo("Printer's job status [printing] " + q.Description + " " + job.JobName);
                                //printingStatusDetail = PrintingStatusDetail.Paused;
                            }
                            else if (job.JobStatus == PrintJobStatus.Completed)
                                FirePrinterStatusInfo(job.JobName + " Completed, " + job.JobStatus.ToString() + " " + q.Description + " ");
                            else
                            {
                                job.Refresh();
                                FirePrinterStatusInfo("Job status " + job.JobStatus.ToString()
                                                            + " " + q.Description
                                                            + " " + job.Name);

#if DEBUG
                                Debug.Print(job.JobStatus.ToString());
#endif
                            }
                            
                            //job.NumberOfPagesPrinted
                            //foreach (var job in jobs)
                            //{
                            //    Debug.Print(job.JobStatus.ToString());
                            //}

                            ChangeStatus(PrintingStatus.Printing);
                        }
                        else
                        {
                            if (status != PrintingStatus.Initialized)
                                ChangeStatus(PrintingStatus.Idle);
                        }
                    }
                    Thread.Sleep(2000);
                }

                Debug.Print("Monitor exit...");
            });

            Debug.Print("done");
            
            bool IsNetworkedPrinter(string name,out string part1,out string part2)
            {
                bool result = false;
                part2 = "";
                part1 = "";
                string root = Path.GetPathRoot(name);

                // Check if root starts with "\\", clearly an UNC
                if (root.StartsWith(@"\\"))
                    result = true;

                if (result)
                {
                    var c = name.IndexOf(@"\", 2);
                    part2 = new string( name.Skip(c+1).ToArray());
                    part1 = new string(name.Take(c).ToArray());
                }

                //// Check if the drive is a network drive
                //DriveInfo drive =  DriveInfo(root);
                //if (drive.DriveType == DriveType.Network)
                //    return true;

                return result;
                

            }
        }

        int currentPage = 0;
        Document document;
        PrintDocument printDocument;

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
            if (rotate)
            {
                transformation.Rotate(-90);
                transformation.Translate(-scaledPdfHeight, 0);
            }

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
               // ( e.PageSettings.PrintableArea.Height * 72 ) / 100,
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

        public async Task<bool> Send3(string filePath, PrinterSettings settings, short noOfCopies = 1)
        {
            Aborted = false;
            ChangeStatus(PrintingStatus.Initialized);

            Task.Run(() =>
            {
                currentPage = 0;
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        document = new Document(file);
                        //settings.DefaultPageSettings.Landscape = true;

                        printDocument = new PrintDocument();
                        printDocument.DocumentName = document.DocumentInfo.Title;
                        if (settings != null)
                            printDocument.PrinterSettings = settings;
                        else
                            printDocument.PrinterSettings.PrinterName = PrinterName;
                        printDocument.DocumentName = filePath;
                        printDocument.PrintPage += new PrintPageEventHandler(printPage);
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.OriginAtMargins = true;
                        printDocument.PrinterSettings.Copies = noOfCopies;
                        //printDocument.PrinterSettings.DefaultPageSettings.Landscape = true;
                        if(settings != null)
                            printDocument.DefaultPageSettings = settings.DefaultPageSettings;
                        //printDocument.DefaultPageSettings.Landscape = true;
                        printDocument.Print();
                    }
                }
                catch (Exception ex)
                {

                    // The conversion failed
                    if (ex.Message != "The operation completed successfully")
                        throw new Exception(String.Format("An error occurred. {0}", ex.Message));
                        //System.Windows.Forms.MessageBox.Show(String.Format("An error occurred. {0}", ex.Message), "Error");
                        //return;
                        //return false;
                }
                finally
                {
                    //Cursor = Cursors.Arrow;
                }

            });

            //ChangeStatus(PrintingStatus.Printing);

            //StartJobMonitor();
            StartJobMonitor2();

            await Task.Factory.StartNew(() =>            {
                while (status == PrintingStatus.Printing || status == PrintingStatus.Initialized)
                    Thread.Sleep(2000);
            });

            JobMonitorTimer.Change(Timeout.Infinite, Timeout.Infinite);

            return true;

            //PDFViewer viewer = new PDFViewer();
            //viewer.ActivateLicense("7F31-B744-6AFD-6540-D4A9-C0D2-6103-D9A3");


            //PDFPrinter PDFPrinter1 = new PDFPrinter("7F31-B744-6AFD-6540-D4A9-C0D2-6103-D9A3");

            //PDFPrinter1.LoadDocument(filePath);
            //PDFPrinter1.PrintOptions.PrinterName = PrinterName;
            //    // Start printing
            //    PDFPrinter1.Print();
        }

        //public async Task<bool> Send2(string filePath,PrinterSettings settings,short noOfCopies=1)
        //{
           

        //    ChangeStatus(PrintingStatus.Initialized);

        //    Task.Run( ()=>
        //    {
        //        try
        //        {
        //            // create the PDF printer 
        //            PdfPrint pdfPrint = new PdfPrint();
        //            //settings.PrinterName = PrinterName;
        //            if (settings != null)
        //            {
        //                pdfPrint.PrinterSettings.SetHdevmode(settings.GetHdevmode());
        //                pdfPrint.PrinterSettings.SetHdevnames(settings.GetHdevnames());
        //            }
        //            else
        //                pdfPrint.PrinterSettings.PrinterName = PrinterName;
                                     
        //            // set the license key
        //            pdfPrint.LicenseKey = "gA4eDxofD2YCeE5dSlVwHR8eGA8YARsPHB4BHh0BFhYWFg8e"; // oy08LDo/LDwsOiI8LD89Ij0+IjU1NTUsPA==";
        //            //if (settings != null)
        //            //    pdfPrint.PrinterSettings.DefaultPageSettings = settings.DefaultPageSettings; // = settings; //.PrinterName = PrinterName;
        //            //pdfPrint.
        //            pdfPrint.ShowStatusDialog = false;
        //            pdfPrint.DocumentName = filePath;
        //            //pdfPrint.PrinterSettings.PrinterName = PrinterName;
                    
        //            pdfPrint.PrinterSettings.Copies = noOfCopies;
                    
                   
        //            //var p = new  System.Windows.Forms.PrintDialog();
        //            //p.PrinterSettings = pdfPrint.PrinterSettings;

                    
                    
        //            //p.ShowDialog();
        //            pdfPrint.Print(filePath); //, startPageNumber, endPageNumber);
                   
                    

        //           // pdfPrint.PrinterSettings.Set
        //        }
        //         catch (Exception ex)
        //         {
        //            // The conversion failed
        //            //MessageBox.Show(String.Format("An error occurred. {0}", ex.Message), "Error");
        //            //return;
        //            //return false;
        //        }
        //         finally
        //         {
        //            //Cursor = Cursors.Arrow;
        //        }

        //     });

        //    //ChangeStatus(PrintingStatus.Printing);

        //    StartJobMonitor();

        //    await Task.Factory.StartNew(() =>
        //    {
        //        while (status == PrintingStatus.Printing || status == PrintingStatus.Initialized)
        //            Thread.Sleep(2000);
        //    });



        //    return true;
        //}


        public async Task<bool> Send(string filePath)
        {

            if (!File.Exists(filePath))
            {                
                return false;
            }

            //var stream = new StreamReader(filePath);

            //var file = File.ReadAllBytes(filePath);
            //var ps = new PrintServer();
            //var qq = ps.GetPrintQueue(PrinterName); // LocalPrintServer.GetDefaultPrintQueue();
            //var job = qq.AddJob();
            //using (var stream = job.JobStream)
            //{
            //    stream.Write(file, 0, file.Length);

            //}

            //return;

            //RawPrinterHelper.SendFileToPrinter(filePath);
            //return;


            string flagNoSplashScreen = "/s /o";
            string flagOpenMinimized = "/h";
            var flagPrintFileToPrinter = string.Format("/t \"{0}\" \"{1}\" ", filePath, PrinterName); // "Microsoft XPS Document Writer"); // "Microsoft Print to PDF");


            var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);
            //var args = string.Format("/p {0}", flagPrintFileToPrinter);

            var startInfo = new ProcessStartInfo
            {
#if DEBUG
                FileName = @"C:\projects2\PrintQueueManager\SumatraPDF.exe", // @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",
#else
                FileName = "SumatraPDF.exe",
#endif
                Arguments = "-print-to \"" + PrinterName + "\" -exit-when-done \"" + filePath + "\"",
                //FileName = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe",
                //Arguments = args,
                CreateNoWindow = true,
                ErrorDialog = false,
                UseShellExecute = false,
                Verb = "print",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                RedirectStandardOutput = false
            };


            //var info = new ProcessStartInfo();
            //info.Verb = "print";
            //info.FileName = @"C:\Program Files (x86)\Adobe\Acrobat Reader DC\Reader\AcroRd32.exe";
            //info.Arguments = String.Format(@"/p /h {0}", filePath);
            //info.UseShellExecute = false;
            //info.CreateNoWindow = true;
            //info.WindowStyle = ProcessWindowStyle.Hidden;

            var p = new Process();
            p.StartInfo = startInfo;

            
            ChangeStatus(PrintingStatus.Initialized);
            
            p.Start();

            StartJobMonitor();

            await Task.Factory.StartNew(() =>
            {
                while (status == PrintingStatus.Printing || status == PrintingStatus.Initialized)
                    ;
            });

            
            //OnPrintingFinished?.Invoke(this, profileOrder);
            return true;

            //p.WaitForInputIdle();
            //System.Threading.Thread.Sleep(3000);
            //if (false == p.CloseMainWindow())
            //    p.Kill();

            //var print = new PdfFilePrinter();
            //print.PdfFileName = filePath;
            //print.PrinterName = "Microsoft Print to PDF";
            //print.Print();


            // check print queue to make sure it finishes printing
            //int count = 0;
            //bool exit = false;
            //while (true)
            //{
            //    var coll = LocalPrintServer..GetPrinterQueue(PrinterName);
            //    txtPrintLabelOutput.Text += "print queue count = " + coll1.Count + "\n";
            //    if (coll1.Count > 0)
            //    {
            //        int count2 = 0;
            //        while (true)
            //        {
            //            System.Threading.Thread.Sleep(200);

            //            StringCollection coll2 = GetPrinterQueue(zebraPrinter);
            //            if (coll2.Count == 0)
            //            {
            //                System.Threading.Thread.Sleep(200); // sometimes it does print the first time...
            //                exit = true;
            //                break;
            //            }

            //            if (++count2 > 30) exit = true;
            //            {
            //                exit = true;
            //                break;
            //            }
            //        }
            //    }
            ///if (++count > 30 || exit) break;
            //System.Threading.Thread.Sleep(200);
            //}

            



        }
        
    }

    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        #region dll Wrappers
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int size);

        #endregion dll Wrappers

        #region Methods
        /// <summary>
        /// This function gets the pdf file name.
        /// This function opens the pdf file, gets all its bytes & send them to print.
        /// </summary>
        /// <param name="szPrinterName">Printer Name</param>
        /// <param name="szFileName">Pdf File Name</param>
        /// <returns>true on success, false on failure</returns>
        public static bool SendFileToPrinter(string pdfFileName)
        {
            try
            {
                #region Get Connected Printer Name
                PrintDocument pd = new PrintDocument();
                StringBuilder dp = new StringBuilder(256);
                int size = dp.Capacity;
                if (GetDefaultPrinter(dp, ref size))
                {
                    pd.PrinterSettings.PrinterName = dp.ToString().Trim();
                }
                #endregion Get Connected Printer Name

                // Open the PDF file.
                FileStream fs = new FileStream(pdfFileName, FileMode.Open);
                // Create a BinaryReader on the file.
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = new Byte[fs.Length];
                bool success = false;
                // Unmanaged pointer.
                IntPtr ptrUnmanagedBytes = new IntPtr(0);
                int nLength = Convert.ToInt32(fs.Length);
                // Read contents of the file into the array.
                bytes = br.ReadBytes(nLength);
                // Allocate some unmanaged memory for those bytes.
                ptrUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
                // Copy the managed byte array into the unmanaged array.
                Marshal.Copy(bytes, 0, ptrUnmanagedBytes, nLength);
                // Send the unmanaged bytes to the printer.
                success = SendBytesToPrinter(pd.PrinterSettings.PrinterName, ptrUnmanagedBytes, nLength);
                // Free the unmanaged memory that you allocated earlier.
                Marshal.FreeCoTaskMem(ptrUnmanagedBytes);
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This function gets the printer name and an unmanaged array of bytes, the function sends those bytes to the print queue.
        /// </summary>
        /// <param name="szPrinterName">Printer Name</param>
        /// <param name="pBytes">No. of bytes in the pdf file</param>
        /// <param name="dwCount">Word count</param>
        /// <returns>True on success, false on failure</returns>
        private static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            try
            {
                Int32 dwError = 0, dwWritten = 0;
                IntPtr hPrinter = new IntPtr(0);
                DOCINFOA di = new DOCINFOA();
                bool success = false; // Assume failure unless you specifically succeed.

                di.pDocName = "PDF Document";
                di.pDataType = "RAW";

                // Open the printer.
                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    // Start a document.
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        // Start a page.
                        if (StartPagePrinter(hPrinter))
                        {
                            // Write the bytes.
                            success = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                            EndPagePrinter(hPrinter);
                        }
                        EndDocPrinter(hPrinter);
                    }
                    ClosePrinter(hPrinter);
                }

                // If print did not succeed, GetLastError may give more information about the failure.
                if (success == false)
                {
                    dwError = Marshal.GetLastWin32Error();
                }
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion Methods
    }

}

