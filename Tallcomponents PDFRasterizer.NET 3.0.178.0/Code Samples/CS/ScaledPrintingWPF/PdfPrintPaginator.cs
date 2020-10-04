using System;
using System.Printing;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

using Document = TallComponents.PDF.Rasterizer.Document;
using Size = System.Windows.Size;
using Page = TallComponents.PDF.Rasterizer.Page;

namespace ScaledPrintingWPF
{
    /// <summary>
    /// Class used to print a PDF document one page at the time.
    /// Each PDF page in indivually scaled and positioned, as the size of each page may vary
    /// </summary>
    public class PdfPrintPaginator : DocumentPaginator
    {
        private Rect _pdfPageArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaledPrintingWPF.PdfPrintPaginator"/> class.
        /// </summary>
        public PdfPrintPaginator()
        {
            _pdfPageArea = new Rect(0, 0, 800, 1066);
        }


        // Tranport configuration from 'PdfPrintAndPreview' to here.
        public Document PdfDocument { get; set; }
        public int FirstPageToPrint { get; set; }
        public int LastPageToPrint { get; set; }
        public bool ScaleToFit { get; set; }
        public bool UsePrintableArea { get; set; }
        public Rect PrintableArea { get; set; }
        public PageOrientation PageOrientation { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }


        /// <summary>
        /// When overridden in a derived class, gets a value 
        /// indicating whether PageCount is the total number of pages.
        /// This is always true in this case because the PDF will never be repaginated.  
        /// </summary>
        public override bool IsPageCountValid
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a count 
        /// of the number of pages currently formatted.
        /// </summary>
        public override int PageCount
        {
            get { return LastPageToPrint - FirstPageToPrint + 1; }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the suggested width and height of each page.
        /// </summary>
        public override Size PageSize
        {
            get
            {
                Size size;
                if (PdfDocument != null && PdfDocument.Pages != null)
                {
                    Page page = PdfDocument.Pages[0];
                    size = new Size(page.Width * 96.00 / 72.00, page.Height * 96.00 / 72.00);
                }
                else
                {
                    size = new Size(100, 100);
                }
                return size;
            }

            set { throw new NotImplementedException(); }
        }


        /// <summary>
        /// When overridden in a derived class, returns the element being paginated.
        /// </summary>
        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }


        /// <summary>
        /// When overridden in a derived class, gets the DocumentPage for the specified page number
        /// </summary>
        /// <param name="pageNumber">
        /// The zero-based page number of the document page that is needed.
        /// </param>
        /// <returns>
        /// The DocumentPage for the specified pageNumber, or DocumentPage.Missing if the page does not exist.
        /// </returns>
        public override DocumentPage GetPage(int pageNumber)
        {
            var pdfPage = PdfDocument.Pages[pageNumber + FirstPageToPrint];
            _pdfPageArea = new Rect(0.00, 0.00, pdfPage.Width, pdfPage.Height);

            // Convert PDF page to a WPF fixed page
            var renderSettings = new RenderSettings();
            var renderOptions = new ConvertToWpfOptions {ConvertToImages = false};
            renderSettings.RenderPurpose = RenderPurpose.Print;
            renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality;
            renderSettings.TextSettings.ResolveFont += PdfFontResolve.Resolve;
            var printPageInWpfFormat = pdfPage.ConvertToWpf(renderSettings, renderOptions);

            // Create a new ContainerVisual as a new parent for page children
            // This prevents the "Fixed page cannot contain another Fixedpage" exception
            // look this construction on the internet, 
            // you will find this workaround like this and its explanation.
            var container = new ContainerVisual();
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            foreach (var child in (printPageInWpfFormat.Children))
            {
                var childClone = (UIElement) child.GetType().GetMethod("MemberwiseClone", bindingFlags).Invoke(child, null);
                var parentField = childClone.GetType().GetField("_parent", bindingFlags);
                if (parentField != null)
                {
                    parentField.SetValue(childClone, null);
                    container.Children.Add(childClone);
                }
            }

            // Transform the WPF fixedpage corresponding to the settings
            container.Transform = GetPrintTransformation();

            return new DocumentPage(container);
        }


        /// <summary>
        /// Compute the proper transformation for printing a page.
        /// </summary>
        /// <returns>The page transformation.</returns>
        /// <remarks>The printable area is the area after the landscape flag has been
        /// taken into account. For example, if letter is specified, and landscape has been set,
        /// _printableArea.Width > _printableArea.Height.
        /// </remarks>
        private TransformGroup GetPrintTransformation()
        {
            var transforms = new TransformGroup();
            double pdfPageScale = 1.00;

            // check if the page needs to be rotated:
            var orientation = GetConfiguredPdfPageOrientation();

            // WPF works at 96DPI, PDF in 72DPI, so convert the PDF to WPF dimensions
            var adjustedPdfPage = new Rect
                                    {
                                        X = 0.00,
                                        Y = 0.00,
                                        Width = _pdfPageArea.Width * 96.00 / 72.00,
                                        Height = _pdfPageArea.Height * 96.00 / 72.00
                                    };

            // If needed, rotate the PDF page
            bool doRotate = (orientation == PageOrientation.Landscape);
            if (doRotate)
            {
               var temp = adjustedPdfPage.Height;
               adjustedPdfPage.Height = adjustedPdfPage.Width;
               adjustedPdfPage.Width = temp;
            }

            // check if the pdf page needs scaled into the printer page
            if (ScaleToFit)
            {
                double scaleX = PrintableArea.Width / adjustedPdfPage.Width;
                double scaleY = PrintableArea.Height / adjustedPdfPage.Height;
                pdfPageScale = Math.Min(scaleX, scaleY);
                adjustedPdfPage.Width *= pdfPageScale;
                adjustedPdfPage.Height *= pdfPageScale;
            }

            // calculate the requested alignment on the X axis
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    break;

                case HorizontalAlignment.Center:
                    adjustedPdfPage.X += (PrintableArea.Width - adjustedPdfPage.Width) / 2;
                    break;

                case HorizontalAlignment.Right:
                    adjustedPdfPage.Y += (PrintableArea.Height - adjustedPdfPage.Height) / 2;
                    break;
            }

            // calculate the requested alignment on the Y axis
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    break;

                case VerticalAlignment.Center:
                    adjustedPdfPage.Y = (PrintableArea.Height - adjustedPdfPage.Height) / 2;
                    break;

                case VerticalAlignment.Bottom:
                    adjustedPdfPage.Y = PrintableArea.Height - adjustedPdfPage.Height;
                    break;
            }

            if ( ! doRotate)
            {
                // if prortait, just make a transformation group to scale the the PDF pages and to 
                // move it to the correct position relative to the printer page
                // -a- first scale to the requirend size
                // -b- move the page according to the configured horizontal and vertical alignments
                //
                // (0,0)+===+------+    (0,0)+=======+--+    (0.0)+-+=======+-+
                //      |VVV|      |         |VVVVVVV|  |         | |VVVVVVV| |
                //      |VVV|      |-a-      |VVVVVVV|  |-b-      | |VVVVVVV| |
                //      +---+      |=======> |VVVVVVV|  |=======> | |VVVVVVV| |
                //      |          |scale    |VVVVVVV|  |adjust   | |VVVVVVV| |
                //      |          |         +-------+  |(x,y)    | +=======+ |
                //      +----------+         +----------+         +-----------+
                transforms.Children.Add(new ScaleTransform(pdfPageScale, pdfPageScale));
                transforms.Children.Add(new TranslateTransform(adjustedPdfPage.X, adjustedPdfPage.Y));
            }
            else
            {
                // if landscape is needed, rotate the PDF page -90 degrees around coordinate (0,0)
                // -a- first scale to the requirend size
                // -b- the PDF page will be rotated, alas to a higher position, like in the picture below
                // -c- to compensate that, the offset is increaed to the width of the PDF document
                // -d- finally (not depicted) the position is adjusted according to the alignments.
                //                                                +---------+
                //                                                |>>>>>>>>>|
                //                                                |>>>>>>>>>|
                // (0,0)+===+------+    (0,0)+======+---+    (0.0)+=========++         +---------++
                //      |VVV|      |         |VVVVVV|   |         |          |         |>>>>>>>>>||
                //      |VVV|      |-a-      |VVVVVV|   |-b-      |          |-c-      |>>>>>>>>>||
                //      +---+      |=======> |VVVVVV|   |=======> |          |=======> +---------+|
                //      |          |scale    |VVVVVV|   |rotate   |          |offset Y |          |
                //      |          |         +------+   |(-90)    |          |         |          |
                //      +----------+         +----------+         +----------+         +----------+
                transforms.Children.Add(new ScaleTransform(pdfPageScale, pdfPageScale));
                transforms.Children.Add(new RotateTransform(-90.00));
                transforms.Children.Add(new TranslateTransform(0.00, adjustedPdfPage.Height));
                transforms.Children.Add(new TranslateTransform(adjustedPdfPage.X, adjustedPdfPage.Y));
            }
            return transforms;
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
                    var printerPaperRatio = PrintableArea.Width / PrintableArea.Height;
                    var pdfPageRatio = _pdfPageArea.Width / _pdfPageArea.Height;
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
    }
}