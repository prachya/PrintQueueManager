// ---------------------------------------
// Print code sample PDFRasterizer.NET 
// in a WPF- MVVM context
//
// Copyright TallComponents BV 2002-2013
// ---------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows; 
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Media;
using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

using Size = System.Windows.Size;
using Page = TallComponents.PDF.Rasterizer.Page;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace ScaledPrintingWPF
{
    public enum PrintPageRange
    {
        All,
        Current,
        Range
    }

    /// <summary>
    /// Class that prints or previews a PDFPDF document in WPF
    /// </summary>
    public class PdfPrintAndPreviewModel
    {
        private LocalPrintServer _printServer;
        private PrintQueue _selectedPrinter;
        private PrintTicket _printTicket;
        private PrintCapabilities _printerCapabilities;
        private readonly Dictionary<int, FixedPage> _previouslyRenderedPdfPages = new Dictionary<int, FixedPage>();

        public delegate void PageBeingPrintedDelegate(int n);
        public PageBeingPrintedDelegate PageBeingPrinted { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfPrintAndPreviewModel"/> class.
        /// </summary>
        public PdfPrintAndPreviewModel()
        {
            _printServer = new LocalPrintServer();
            _selectedPrinter = LocalPrintServer.GetDefaultPrintQueue();
            _printTicket = _selectedPrinter.DefaultPrintTicket;
            TextRenderSettings.CMapFolder = new DirectoryInfo(@"..\..\..\..\..\Support\CMaps\").FullName;
            PageNr = 0;
            NumberOfPagesInPdf = 2;
            FirstPageToPrint = 0;
            LastPageToPrint = 0;
            PreviewSize = new Size(250.00, 310.00);
        }

        public Size PreviewSize { get; set; }

        ////////////////////////////////////////////
        //                                        //
        //          P D F   c o n f i g           //
        //                                        //
        ////////////////////////////////////////////

        #region PDFconfigiration

        public Document PdfDocument { get; private set; }
        public string Filename { get; set; }
        public int PageNr { get; set; }
        public int NumberOfPagesInPdf { get; set; }
        public PageOrientation PageOrientation { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Opens the PDF files specified by "Filename"
        /// </summary>
        public void OpenPdfDocument()
        {
            using (var pdfFile = new FileStream(Filename, FileMode.Open, FileAccess.Read))
            {
                PdfDocument = new Document(pdfFile);
                PageNr = 0;
                NumberOfPagesInPdf = PdfDocument.Pages.Count;
                FirstPageToPrint = 0;
                LastPageToPrint = NumberOfPagesInPdf - 1;
                _previouslyRenderedPdfPages.Clear();
            }
        }

        /// <summary>
        /// Gets the size of the current PDF page.
        /// </summary>
        /// <returns>
        /// The size (is width and height) of the current page.
        /// </returns>
        public Size CurrentPdfPageSize
        {
            get
            {
                var size = new Size(0.0, 0.0);
                if (PdfDocument != null)
                {
                    var page = PdfDocument.Pages[PageNr];
                    size.Width = page.Width;
                    size.Height = page.Height;
                }
                return size;
            }
        }

        #endregion

        ////////////////////////////////////////////
        //                                        //
        //        P r i n t   c o n f i g         //
        //                                        //
        ////////////////////////////////////////////

        #region PrintConfigiration

        public int FirstPageToPrint { get; set; }
        public int LastPageToPrint { get; set; }
        public bool ScaleToFit { get; set; }
        public bool UsePrintableArea { get; set; }
        public PrintPageRange PrintPageRange { get; set; }


        /// <summary>
        /// Gets the list of usable printers on this system
        /// </summary>
        public PrintQueueCollection InstalledPrinters
        {
            get
            {
                EnumeratedPrintQueueTypes[] enumerationFlags =
                    {
                        EnumeratedPrintQueueTypes.Local,
                        EnumeratedPrintQueueTypes.Connections
                    };
                return _printServer.GetPrintQueues(enumerationFlags);
            }
        }


        /// <summary>
        /// Gets or sets the currently selected printer
        /// </summary>
        public PrintQueue SelectedPrinter
        {
            get { return _selectedPrinter; }

            set
            {
                _selectedPrinter = value;
                // may take some time, try to minimize these:
                _printTicket = _selectedPrinter.DefaultPrintTicket;
                _printerCapabilities = _selectedPrinter.GetPrintCapabilities();
            }
        }

        /// <summary>
        /// Gets all page media size of the currently selected printer
        /// </summary>
        public ReadOnlyCollection<PageMediaSize> PrinterPaperMediaSizes
        {
            get { return _printerCapabilities.PageMediaSizeCapability; }
        }

        /// <summary>
        /// Gets or sets the page media size (is width, height and media size name)
        /// </summary>
        public PageMediaSize PrinterPaperMediaSize
        {
            get { return _printTicket.PageMediaSize; }
            set { _printTicket.PageMediaSize = value; }
        }

        /// <summary>
        /// Gets all page sources of the currently selected printer
        /// We read this directly from the printticket
        /// </summary>
        public ReadOnlyCollection<InputBin> PrinterPaperSources
        {
            get { return _printerCapabilities.InputBinCapability; }
        }

        /// <summary>
        /// Gets or sets the paper source of the currently selected printer
        /// We read or write this directly in the printticket.
        /// </summary>
        public InputBin PrinterPaperSource
        {
            get { return _printTicket.InputBin.GetValueOrDefault(); }
            set { _printTicket.InputBin = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the document must printed on both sides.
        /// We read or write this directly in the printticket
        /// </summary>
        public bool PrintDuplex
        {
            get
            {
                if (!_printerCapabilities.DuplexingCapability.Contains(Duplexing.TwoSidedLongEdge))
                {
                    return false;
                }
                return _printTicket.Duplexing == Duplexing.TwoSidedLongEdge;
            }
            set
            {
                if (_printerCapabilities.DuplexingCapability.Contains(Duplexing.TwoSidedLongEdge))
                {
                    _printTicket.Duplexing = (value)
                                           ? Duplexing.TwoSidedLongEdge
                                           : Duplexing.OneSided;
                }
            }
        }


        /// <summary>
        /// Gets the width of the selected pagesize on the currently selected printer
        /// </summary>
        public Size PrinterPaperSize
        {
            get
            {
                var size = new Size(783.00, 1023.00);
                if (_printTicket.PageMediaSize != null)
                {
                    size.Width = _printTicket.PageMediaSize.Width.GetValueOrDefault();
                    size.Height = _printTicket.PageMediaSize.Height.GetValueOrDefault();
                }
                return size;
            }
        }

        public PageImageableArea PrinterPaperImageableArea
        {
            get { return _printerCapabilities.PageImageableArea; }
        }

        /// <summary>
        /// Gets the scaling of one page to fit on a printed page
        /// It is only used by the document information box (under the preview)
        /// </summary>
        /// <returns>
        /// The scale used when printing this PDF.
        /// </returns>
        public double GetScaleToFitPdfPageOnPrinterPaper()
        {
            var scale = 1.00;
            if (SelectedPrinter != null)
            {
                // get the printable area
                var printableArea = GetScaledConfiguredPrintableArea(1.00);

                // get the dimensions of the current PDF page
                var pdfPageSize = GetScaledAndRotatedPdfPageSize(1.00); // do not scale, only get the dimensions

                double scaleX = printableArea.Width / pdfPageSize.Width;
                double scaleY = printableArea.Height / pdfPageSize.Height;
                scale = Math.Min(scaleX, scaleY);
            }
            return scale;
        }

        #endregion

        ///////////////////////////////////////////////////
        //                                               //
        //     P a g e   p r e v i e w   w i n d o w     //
        //                                               //
        ///////////////////////////////////////////////////
        
        #region printPreview

        /// <summary>
        /// Gets a preview of the currently selected page of the PDF document.
        /// It contains in this overlapping order:
        /// 1 - a rectangle that shows the the preview box
        /// 2 - a filled rectangle that shows the borders of the printer paper, may be overwritten by the PDF
        /// 3 - a rendered PDF page
        /// 4 - a rectangle that shows the borders of the PDF page
        /// 5 - a rectangle that shows the borders of the printer paper
        /// 6 - a rectangle that shows the usable area on the printer paper
        /// This way it is visible which part of the PDF page can not be printed, and which part
        /// of the printed page will remain empty.
        /// </summary>
        /// <param name="canvas">
        /// The canvas to preview
        /// </param>
        public void DrawPreviewOfOnePdfPage(ref Canvas canvas)
        {
            if (canvas == null || _printServer == null || PrinterPaperImageableArea == null)
            {
                return;
            }

            // The previewScale is the scale factor what is needed to be sure both the
            // printerpaper and the PDF page will fit in the preview window
            double previewScale = GetPreviewScale();

            // (1) Fill the background of the entire preview
            canvas.Background = new SolidColorBrush(Colors.LightGray);

            // (2) draw the inside of the printer-paper in the preview
            // it will be placed behind the PDF image
            // create a transformation for the printer page
            var printPaper = PrinterPaperSize;
            var printerPaperTransformGroup = GetPreviewOfPrinterPaperTransformGroup(previewScale);
            var printerPaperInside = new Rectangle
                                       {
                                           Width = printPaper.Width,
                                           Height = printPaper.Height,
                                           StrokeThickness = 0.00,
                                           Fill = new SolidColorBrush(Colors.Linen),
                                           RenderTransform = printerPaperTransformGroup
                                       };
            canvas.Children.Add(printerPaperInside);

            // (3) If a PDF file is opened, draw one page of it.
            if (PdfDocument != null && PdfDocument.Pages != null)
            {
                // Get a PDF page as WPF, using caching if possible
                // transform the PDF page into the preview box
                FixedPage previewPageInWpfFormat = GetPdfPageAsWpf(false);
                var pdfPageTransformGroup = GetPreviewOfPdfPageTransformGroup(previewScale);
                previewPageInWpfFormat.RenderTransform = pdfPageTransformGroup;
                canvas.Children.Add(previewPageInWpfFormat);

                // (4) Draw the ouline of the PDF page in red and also transform it into the preview box
                var pdfPageSize = GetScaledPdfPageSize(1.00);
                var pdfPageOutline = new Rectangle
                                         {
                                             Width = pdfPageSize.Width,
                                             Height = pdfPageSize.Height,
                                             Stroke = new SolidColorBrush(Colors.Red),
                                             StrokeThickness = 1.0,
                                             RenderTransform = pdfPageTransformGroup
                                         };
                canvas.Children.Add(pdfPageOutline);
            }

            // (5) draw outline of the printer-page in the preview
            // it will be placed on top of the PDF image
            var printPaperOutline = new Rectangle
                                        {
                                            Width = printPaper.Width,
                                            Height = printPaper.Height,
                                            Stroke = new SolidColorBrush(Colors.DimGray),
                                            StrokeThickness = 5.0,
                                            RenderTransform = printerPaperTransformGroup
                                        };
            canvas.Children.Add(printPaperOutline);

            // (6) Draw a rectangle that shows the usable area on the printer paper
            if (UsePrintableArea)
            {
                var imageableArea = GetScaledPrintableArea(scale: 1.00);
                var printableAreaTransformGroup = GetPreviewOfPrintableAreaTransformGroup(previewScale);
                var printableOutline = new Rectangle
                                           {
                                               Width = imageableArea.Width,
                                               Height = imageableArea.Height,
                                               Stroke = new SolidColorBrush(Colors.DarkGray),
                                               StrokeThickness = 5.0,
                                               RenderTransform = printableAreaTransformGroup
                                           };
                canvas.Children.Add(printableOutline);
            }
        }


        /// <summary>
        /// Gets the PDF page in WPF format.
        /// Cache one page for efficiency reasons.
        /// </summary>
        /// <param name="renderAsBitmap">
        /// The render As Bitmap.
        /// </param>
        /// <returns>
        /// A rendered PDF page in WPF format
        /// </returns>
        private FixedPage GetPdfPageAsWpf(bool renderAsBitmap)
        {
            const int memoizationLimit = 10;

            // dont recalculate the rendered page when it is not necessary
            // cache one page and reuse it if only printer settings changes (like portrait / landscape)
            // This is more usefull as you may as WPF tends to updaet the screen multiple times
            // if any update event occurs.
            FixedPage previewPageInWpfFormat;
            if (!_previouslyRenderedPdfPages.TryGetValue(PageNr, out previewPageInWpfFormat))
            {
                // Memoization of this PDF page dit not work, so render this one
                Page pdfPage = PdfDocument.Pages[PageNr];
                var renderSettings = new RenderSettings();
                var renderOptions = new ConvertToWpfOptions { ConvertToImages = renderAsBitmap };
                renderSettings.RenderPurpose = RenderPurpose.View;
                renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighSpeed;
                renderSettings.TextSettings.ResolveFont += PdfFontResolve.Resolve;
                previewPageInWpfFormat = pdfPage.ConvertToWpf(renderSettings, renderOptions);

                // store the rendered PDF pages but not too many of then,
                if (_previouslyRenderedPdfPages.Count >= memoizationLimit)
                {
                    // remove any page (not necessarily the oldest) 
                    _previouslyRenderedPdfPages.Remove(_previouslyRenderedPdfPages.First().Key);
                }
                _previouslyRenderedPdfPages.Add(PageNr, previewPageInWpfFormat);
            }

            return previewPageInWpfFormat;
        }


        
        /// <summary>
        /// Gets the preview scale to fit the PDF page and the printer paper in the preview box.
        /// </summary>
        /// <returns>
        /// The scale to fit the printer paper and the PDF page in the preview window
        /// </returns>
        private double GetPreviewScale()
        {
            var pdfPageScale = 1.00;
            const double printPageBorderSize = 2.00;

            var printerPaperSize = GetScaledPrinterPaperSize(1.00); // not scaled
            var orientation = GetConfiguredPdfPageOrientation();

            // Use an undersized preview area so that we can add a border around the printed page
            var underSizedPreview = new Size(PreviewSize.Width, PreviewSize.Height);
            underSizedPreview.Width -= (4 * printPageBorderSize);
            underSizedPreview.Height -= (4 * printPageBorderSize);
            var printerPaperScaleX = underSizedPreview.Width / printerPaperSize.Width;
            var printerPaperScaleY = underSizedPreview.Height / printerPaperSize.Height;
            var printerPaperScale = Math.Min(printerPaperScaleX, printerPaperScaleY);

            // also adjust the scale if a PDF page is larger than the printer page
            if (PdfDocument != null)
            {
                if ( ! ScaleToFit)
                {
                    // When not scale to fit, make the PDF page fit in the preview.
                    // Reserve extra space (oversized) if the PDF size is bigger than the printable area
                    // the pdf page would move out out the preview window otherwise.
                    var pdfPageSize = GetScaledAndRotatedPdfPageSize(scale: 1.00); // no scaling
                    if (orientation == PageOrientation.Portrait)
                    {
                        var overSizedY = (2.00 * pdfPageSize.Height) - printerPaperSize.Height;
                        pdfPageScale = underSizedPreview.Height/Math.Max(overSizedY, pdfPageSize.Height);
                    }
                    else // landscape:
                    {
                        var overSizedX = (2.00 * pdfPageSize.Height) - printerPaperSize.Width;
                        pdfPageScale = underSizedPreview.Width/Math.Max(overSizedX, pdfPageSize.Height);
                    }
                }
            }
            return Math.Min(printerPaperScale, pdfPageScale);
        }


        /// <summary>
        /// Gets the transform group to preview a printer page with the dimensions of the currently
        /// selected paper format of the selected printer.
        /// </summary>
        /// <returns>
        /// The transformation group for the scale and the offset 
        /// </returns>
        private TransformGroup GetPreviewOfPrinterPaperTransformGroup(double scale)
        {
            var transformGroup = new TransformGroup();

            var origin = GetScaledPrinterPaperOrigin(scale);

            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            transformGroup.Children.Add(new TranslateTransform(origin.X, origin.Y));

            return transformGroup;
        }

        /// <summary>
        /// Gets the transform group to preview a printable area on a printer page with the dimensions
        /// of the currently selected paper format of the selected printer.
        /// </summary>
        /// <returns>
        /// The transformation group for the scale and the offset 
        /// </returns>
        private TransformGroup GetPreviewOfPrintableAreaTransformGroup(double scale)
        {
            var transformGroup = new TransformGroup();

            var origin = GetScaledPrintableAreaOrigin(scale);

            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            transformGroup.Children.Add(new TranslateTransform(origin.X, origin.Y));

            return transformGroup;
        }

        /// <summary>
        /// Gets the PDF page transform group to fit the PDF page into the preview box,
        /// and positioning at the correct location, rotation and scale,
        /// taking the options 'scale to fit' and the horizontal- and vertical alignment
        /// into account.
        /// </summary>
        /// <returns>
        /// The transformation group for the scale and the offset 
        /// </returns>
        private TransformGroup GetPreviewOfPdfPageTransformGroup(double scale)
        {
            var transformGroup = new TransformGroup();
            var orientation = GetConfiguredPdfPageOrientation();
            double pdfPreviewScale = scale;

            // areas to be used:
            var scaledPrintableArea = GetScaledConfiguredPrintableArea(scale);
//            var scaledPrintableOrigin = GetScaledConfiguredPrinterPaperPartOrigin(scale);
            var scaledPdfPageSize = GetScaledAndRotatedPdfPageSize(scale);

            // If 'scale to fit' is true, make the PDF page fit into the available printer page earea.
            if (ScaleToFit)
            {
                double scaleX = scaledPrintableArea.Width / scaledPdfPageSize.Width;
                double scaleY = scaledPrintableArea.Height / scaledPdfPageSize.Height;
                double scaleSmallest = Math.Min(scaleX, scaleY);
                pdfPreviewScale *= scaleSmallest;
                scaledPdfPageSize.Width *= scaleSmallest;
                scaledPdfPageSize.Height *= scaleSmallest;
            }

            // process the horizontal alignment option, working in the dimensions of the preview box
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    break;

                case HorizontalAlignment.Center:
                    scaledPrintableArea.X += (scaledPrintableArea.Width - scaledPdfPageSize.Width) / 2.00;
                    break;

                case HorizontalAlignment.Right:
                    scaledPrintableArea.X += scaledPrintableArea.Width - scaledPdfPageSize.Width;
                    break;
            }

            // also process the vertical alignment option
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    break;

                case VerticalAlignment.Center:
                    scaledPrintableArea.Y += (scaledPrintableArea.Height - scaledPdfPageSize.Height) / 2;
                    break;

                case VerticalAlignment.Bottom:
                    scaledPrintableArea.Y += scaledPrintableArea.Height - scaledPdfPageSize.Height;
                    break;
            }

            if (orientation == PageOrientation.Portrait)
            {
                // if prortait, just make a transformation group to scale the the PDF pages and to 
                // move it to the correct position relative to the printer page
                transformGroup.Children.Add(new ScaleTransform(pdfPreviewScale, pdfPreviewScale));
                transformGroup.Children.Add(new TranslateTransform(scaledPrintableArea.X, scaledPrintableArea.Y));
            }
            else
            {
                // if landscape, rotate the PDF page -90 degrees around coordinate (0,0)
                // -a- the PDF page will be rotated to a higher position, like:
                // -b- to compensate that, the offset is increaed to the width of the PDF document
                //     (which is stored in the 'pdfSizeHeight' variable as this page is rotated!)
                //                              +---------+
                //                              |>>>>>>>>>|
                //                              |>>>>>>>>>|
                // (0,0)+======+---+       (0.0)+=========++            +---------++
                //      |VVVVVV|   |            |          |            |>>>>>>>>>||
                //      |VVVVVV|   |    -a-     |          |    -b-     |>>>>>>>>>||
                //      |VVVVVV|   |   =====>   |          |   =====>   +---------+|
                //      |VVVVVV|   |   rotate   |          |  offset Y  |          |
                //      +------+   |   (-90)    |          |  += width  |          |
                //      +----------+            +----------+            +----------+
                scaledPrintableArea.Y += scaledPdfPageSize.Height;
                transformGroup.Children.Add(new ScaleTransform(pdfPreviewScale, pdfPreviewScale));
                transformGroup.Children.Add(new RotateTransform(-90.000, 0, 0));
                transformGroup.Children.Add(new TranslateTransform(scaledPrintableArea.X, scaledPrintableArea.Y));
            }

            return transformGroup;
        }


        /// <summary>
        /// Get the scaled size of the printer paper, scaled to fit in the preview box
        /// </summary>
        /// <returns>
        /// A size with the width and height of the printer paper
        /// </returns>
        private Size GetScaledPrinterPaperSize(double scale)
        {
            var size = new Size(PrinterPaperSize.Width, PrinterPaperSize.Height);

            // adjust the dimensions to the (preview) scale
            size.Width *= scale;
            size.Height *= scale;

            return size;
        }


        /// <summary>
        /// get the location of the upper left edge of the printer paper in the preview box
        /// </summary>
        /// <returns>
        /// A point with the X and Y coordinates of the origin of the printer paper
        /// </returns>
        private System.Windows.Point GetScaledPrinterPaperOrigin(double scale)
        {
            var printerPaperSize = GetScaledPrinterPaperSize(scale);
            var origin = new System.Windows.Point
            {
                X = (PreviewSize.Width - printerPaperSize.Width) / 2.00,
                Y = (PreviewSize.Height - printerPaperSize.Height) / 2.00
            };
            return origin;
        }

        private Rect GetScaledPrinterPaperArea(double scale)
        {
            double width = PrinterPaperSize.Width * scale;
            double height = PrinterPaperSize.Height * scale;
            var area = new Rect()
                           {
                               X = (PreviewSize.Width - width)/2.00,
                               Y = (PreviewSize.Height - height)/2.00,
                               Width = PrinterPaperSize.Width,
                               Height = PrinterPaperSize.Height,
                           };
            
            return area;
        }


        /// <summary>
        /// get the scaled size of the printable area of the printer paper, scaled to fit in the preview box,
        /// clipped to the size of the currently selected printer paper format
        /// </summary>
        /// <returns>
        /// A size with the width and height of the printer paper
        /// </returns>
        /*
        private Size GetScaledPrintableAreaSize(double scale)
        {
            // The the imageable area and adjust it to the (preview) scale
            var printableSize = new Size(PrinterPaperImageableArea.ExtentWidth, PrinterPaperImageableArea.ExtentHeight);
            printableSize.Width *= scale;
            printableSize.Height *= scale;

            // need this as fallback:
            var paperArea = GetScaledPrinterPaperArea(scale);

            // It is possible that a driver returns bogus data for the printable area. If that is
            // obviously the case, we are going to ignore the usePrintableArea settings 
            // and use the printer paper values instead
            if ((printableSize.Width <= 10.00) || (printableSize.Height <= 10.00))
            {
                printableSize.Width = paperArea.Width;
                printableSize.Height = paperArea.Height;
            }

            // In WPF, the printable area is printer specific, not paper format specific
            // A bit weird as usually the bottom part of a paper cannot be printed, due to the paper handling
            // So this cannot work correctly when printing small paper sizes like A5.
            // Anyway, clipped it as the printable area should not be larger as the printer paper itself
            var printableOrigin = GetScaledPrintableAreaOrigin(scale);
            if ((printableOrigin.X + printableSize.Width) > (paperArea.X + paperArea.Width))
            {
                printableSize.Width = paperArea.X + paperArea.Width - printableOrigin.X;
            }

            if ((printableOrigin.Y + printableSize.Height) > (paperArea.Y + paperArea.Height))
            {
                printableSize.Height = paperArea.Y + paperArea.Height - printableOrigin.Y;
            }

            return printableSize;
        }
        */

        /// <summary>
        /// get the location of the left top of the printable area in the preview box
        /// </summary>
        /// <returns>
        /// A point with the X and Y coordinates of the origin of the printable area
        /// </returns>
        /*
        private System.Windows.Point GetScaledPrintableAreaOrigin(double scale)
        {
            var paperOrigin = GetScaledPrinterPaperOrigin(scale);

            // get the scaled origin of the printable area
            var printableAreaOrigin = new System.Windows.Point
                                          {
                                              X = PrinterPaperImageableArea.OriginWidth*scale,
                                              Y = PrinterPaperImageableArea.OriginHeight*scale
                                          };

            // add the printer paper origin as the imageable area is inside it
            printableAreaOrigin.X += paperOrigin.X;
            printableAreaOrigin.Y += paperOrigin.Y;

            return printableAreaOrigin;
        }
        */
        private Rect GetScaledPrintableArea(double scale)
        {
            var paperArea = GetScaledPrinterPaperArea(scale);

            // It is possible that a driver returns bogus data for the printable area. If that is
            // obviously the case, we are going to ignore the usePrintableArea settings 
            // and use the printer paper values instead
            if (  (PrinterPaperImageableArea.ExtentWidth <= 10.00) 
               || (PrinterPaperImageableArea.ExtentHeight <= 10.00))
            {
                return paperArea;
            }

            var printableArea = new Rect()
                                    {
                                        X = paperArea.X + PrinterPaperImageableArea.OriginWidth * scale,
                                        Y = paperArea.Y + PrinterPaperImageableArea.OriginHeight * scale,
                                        Width = PrinterPaperImageableArea.ExtentWidth * scale,
                                        Height = PrinterPaperImageableArea.ExtentHeight * scale,
                                    };

            // In WPF, the printable area is printer specific, not paper format specific
            // A bit weird as sometimes the bottom part of a paper cannot be printed, 
            // due to the mechnics of the paper handling
            // So this cannot work correctly when printing small paper sizes like A5.
            // Anyway, clipped it as the printable area should not be larger as the printer paper itself
            if ((printableArea.X + printableArea.Width) > (paperArea.X + paperArea.Width))
            {
                printableArea.Width = paperArea.X + paperArea.Width - printableArea.X;
            }

            if ((printableArea.Y + printableArea.Height) > (paperArea.Y + paperArea.Height))
            {
                printableArea.Height = paperArea.Y + paperArea.Height - printableArea.Y;
            }
            return printableArea;
        }



        /// <summary>
        /// get the scaled size of the area to be used when printing, which depend on the 'UsePrintableArea' configuration
        /// </summary>
        /// <returns>
        /// A size with the width and height of the either the printer paper of the printable area
        /// </returns>
        private Size GetScaledConfiguredPrintableSize(double scale)
        {
            if (UsePrintableArea)
            {
                return GetScaledPrintableAreaSize(scale);
            }
            return GetScaledPrinterPaperSize(scale);
        }
        private Rect GetScaledConfiguredPrintableArea(double scale)
        {
            if (UsePrintableArea)
            {
                return GetScaledPrintableArea(scale);
            }
            return GetScaledPrinterPaperArea(scale);
        }


        /// <summary>
        /// get the origin of the area to be used when printing, which depend on the 'UsePrintableArea' configuration
        /// </summary>
        /// <returns>
        /// A point with the X and Y coordinates of the either the printer paper of the printable area
        /// </returns>
        private System.Windows.Point GetScaledConfiguredPrinterPaperPartOrigin(double scale)
        {
            if (UsePrintableArea)
            {
                return GetScaledPrintableAreaOrigin(scale);
            }
            return GetScaledPrinterPaperOrigin(scale);
        }

        /// <summary>
        /// get the scaled size of the currently selected PDF page
        /// </summary>
        /// <returns>
        /// A size with the width and height of the currently selected PDF page
        /// </returns>
        private Size GetScaledPdfPageSize(double scale)
        {
            var scaledPdfPageSize = new Size(816, 1056);
            if (PdfDocument != null)
            {
                var pdfPage = PdfDocument.Pages[PageNr];
                scaledPdfPageSize = new Size(pdfPage.Width, pdfPage.Height);

                // WPF works at 96DPI, PDF in 72DPI, so convert the PDF to WPF
                scaledPdfPageSize.Width *= 96.00/72.00;
                scaledPdfPageSize.Height *= 96.00/72.00;

                // adjust the dimensions to the (preview) scale
                scaledPdfPageSize.Width *= scale;
                scaledPdfPageSize.Height *= scale;
            }
            return scaledPdfPageSize;
        }

        /// <summary>
        /// get the scaled size of the currently viewed PDF page
        /// </summary>
        /// <returns>
        /// A size with the width and height of the either the printer paper of the printable area
        /// </returns>
        private Size GetScaledAndRotatedPdfPageSize(double scale)
        {
            var scaledPdfPageSize = GetScaledPdfPageSize(scale);
            var orientation = GetConfiguredPdfPageOrientation();

            if (orientation == PageOrientation.Landscape)
            {
                Turn(ref scaledPdfPageSize);
            }

            return scaledPdfPageSize;
        }


        /// <summary>
        /// Gets the PDF page orientation as follows:
        /// landscape and portrait remains untranslated, unknown is translated to either portrait or landscape
        /// </summary>
        /// <returns>
        /// The preferred orientation
        /// </returns>
        private PageOrientation GetConfiguredPdfPageOrientation()
        {
            PageOrientation orientation = PageOrientation;
            switch (orientation)
            {
                case PageOrientation.Landscape:
                case PageOrientation.Portrait:
                    break;

                default:
                    var printableArea = GetScaledConfiguredPrintableArea(1.00);
                    var pdfPageSize = GetScaledPdfPageSize(1.00);
                    var printerPaperRatio = printableArea.Width / printableArea.Height;
                    var pdfPageRatio = pdfPageSize.Width / pdfPageSize.Height;
                    if ((pdfPageRatio > 1 && printerPaperRatio < 1) || (pdfPageRatio < 1 && printerPaperRatio > 1))
                    {
                        orientation = PageOrientation.Landscape;
                    }
                    else
                    {
                        orientation = PageOrientation.Portrait;
                    }
                    break;
            }
            return orientation;
        }

        #endregion

        ///////////////////////////////////////////////////////
        //                                                   //
        //         P r i n t i n g   t h e   P D F           //
        //                                                   //
        ///////////////////////////////////////////////////////

        #region Printing

        /// <summary>
        /// Callback called when the printing is finished
        /// </summary>
        private void AsyncPrintCompleted(object sender, WritingCompletedEventArgs e)
        {
            if (PageBeingPrinted != null)
            {
                PageBeingPrinted(-1);
            }
        }

        /// <summary>
        /// Callback called when some progress is made (this is: 1 page printed)
        /// </summary>
        private void AsyncPrintProgressChanged(object sender, WritingProgressChangedEventArgs e)
        {
            if (PageBeingPrinted != null)
            {
                PageBeingPrinted(e.Number);
            }
        }

        /// <summary>
        /// Prints the PDF document.
        /// </summary>
        public void PrintPdfDocument()
        {
            if ((PdfDocument == null) || (_selectedPrinter == null))
            {
                return;
            }

            // figure out which pages must be printed
            switch (PrintPageRange)
            {
                case PrintPageRange.Current:
                    FirstPageToPrint = PageNr;
                    LastPageToPrint = PageNr;
                    break;

                case PrintPageRange.All:
                    FirstPageToPrint = 0;
                    LastPageToPrint = PdfDocument.Pages.Count - 1;
                    break;
            }

            var documentPrinter = PrintQueue.CreateXpsDocumentWriter(_selectedPrinter);
            documentPrinter.WritingCompleted += AsyncPrintCompleted;
            documentPrinter.WritingProgressChanged += AsyncPrintProgressChanged;
            FirstPageToPrint = Math.Max(0, FirstPageToPrint);
            LastPageToPrint = Math.Min(PdfDocument.Pages.Count - 1, LastPageToPrint);

            var printPaginator = new PdfPrintPaginator()
                                     {
                                         PdfDocument = PdfDocument,
                                         FirstPageToPrint = FirstPageToPrint,
                                         LastPageToPrint = LastPageToPrint,
                                         ScaleToFit = ScaleToFit,
                                         UsePrintableArea = UsePrintableArea,
                                         ConfiguredPrintableAreaSize = PrinterPaperSize,
                                   //      PrinterPaperImageableArea = PrinterPaperImageableArea,
                                         PageOrientation = this.GetConfiguredPdfPageOrientation(),
                                         VerticalAlignment = VerticalAlignment,
                                         HorizontalAlignment = HorizontalAlignment,
                                     };
            documentPrinter.WriteAsync(printPaginator, _printTicket);
        }

        /// <summary>
        /// Turn portrait into landscape or vise versa
        /// </summary>
        private void Turn(ref Size s)
        {
            var temp = s.Height;
            s.Height = s.Width;
            s.Width = temp;
        }

        #endregion Printing
    }
}