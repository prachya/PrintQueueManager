// ---------------------------------------
// Print code sample PDFRasterizer.NET in a WPF- MVVM context
// Copyright TallComponents BV 2002-2014
// ---------------------------------------
//
// This part implements the View-Model of the MVVM
// There is a possible discussion on how to implement data changes that origins
// from the model, there are two possibilities:
// (1) The model should provide notifications that the viewmodel should follow,
// (2) The model does not, the viewmodel just reads it when it need to know something.
// 
// In this sample code the latter is choosen, as it should only 
// demonstrate the PDF rasterizer in a WPF (MVVM), and the knowledge of the
// user-interactions and items that is viewed is located only in this 
// viewmodel.
//
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Size = System.Windows.Size;
using VerticalAlignment = System.Windows.VerticalAlignment;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using PageOrientation = System.Printing.PageOrientation;


using System.Reflection;



namespace ScaledPrintingWPF
{
  
    /// <summary>
    /// The view-model for the main page
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Canvas _previewCanvas;
        private readonly PdfPrintAndPreview _pdf;
        private int _printingPageNumber = -1;

      
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            ViewIsLoaded = false;
            _pdf = new PdfPrintAndPreview();
            _pdf.PageBeingPrinted = PrintProgressInformation;
        }

        /// <summary>
        /// Occurs when any property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the view is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool ViewIsLoaded { get; set; }

        /// <summary>
        /// Gets or sets the size (width * height) of the view box.
        /// It is called from the view - when the height and width
        /// of the preview box is known here.
        /// </summary>
        /// <value>
        /// The height of the view box.
        /// </value>
        public Size PreviewSize
        {
            get { return _pdf.PreviewSize; }
            set { _pdf.PreviewSize = value; }
        }

        /// <summary>
        /// The user selected: menu file open.
        /// Now create a OpenFileDialog and read the PDF file
        /// </summary>
        public void UserActionFileOpen()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog { DefaultExt = ".pdf", Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*" };
            bool? fileOpenResult = openFileDialog.ShowDialog();
            if (fileOpenResult == true)
            {
                _pdf.Filename = openFileDialog.FileName;
                _pdf.OpenPdfDocument();
                _pdf.PageNr = 0;
                _pdf.PrintPageRange = PrintPageRange.All;
                _pdf.FirstPageToPrint = 0;
                _pdf.LastPageToPrint = _pdf.NumberOfPagesInPdf - 1;
                NotifyPropertyChanged("CurrentFileName", "PrintRangeFirst", "PrintRangeLast",
                                      "PreviewOfOnePdfPage", "DocumentInformation", "MaximumPageNumber",
                                      "CurrentPageNumber");
            }
        }

        /// <summary>
        /// Gets or sets the name of the current PDF file.
        /// </summary>
        /// <value>
        /// The name of the current file.
        /// </value>
        public string CurrentFileName
        {
            get { return _pdf.Filename; }
            set { } // user interface wont update without it
        }

        /// <summary>
        /// Gets a list of the installed printers.
        /// </summary>
        public PrintQueueCollection InstalledPrinters
        {
            get { return _pdf.InstalledPrinters; }
        }

        /// <summary>
        /// Gets or sets the currently selected printer.
        /// </summary>
        /// <value>
        /// The selected printer.
        /// </value>
        public PrintQueue SelectedPrinter
        {
            get { return _pdf.SelectedPrinter; }

            set
            {
                if (value == null)
                {
                    return;
                }
                if (_pdf.SelectedPrinter.Name == value.Name)
                {
                    return;
                }

                _pdf.SelectedPrinter = value;
                NotifyPropertyChanged("SelectedPrinter", "PrinterPaperSizes", "PrinterPaperSources",
                                      "SelectedPaperSize", "SelectedPaperSource", "PreviewOfOnePdfPage",
                                      "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets the paper sizes of the selected printer.
        /// </summary>
        public ReadOnlyCollection<PageMediaSize> PrinterPaperSizes
        {
            get { return _pdf.PrinterPaperMediaSizes; }
        }

        /// <summary>
        /// Gets or sets the size of the selected paper.
        /// </summary>
        /// <value>
        /// The size of the selected paper.
        /// </value>
        public PageMediaSize SelectedPaperSize
        {
            get { return _pdf.PrinterPaperMediaSize; }

            set
            {
                if (_pdf.PrinterPaperMediaSize == value)
                {
                    return;
                }
                _pdf.PrinterPaperMediaSize = value;
                NotifyPropertyChanged("SelectedPaperSize", "DocumentInformation", "PreviewOfOnePdfPage",
                                      "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets the paper sources of the selected printer.
        /// </summary>
        public ReadOnlyCollection<InputBin> PrinterPaperSources
        {
            get { return _pdf.PrinterPaperSources; } 
        }

        /// <summary>
        /// Gets or sets the default / selected paper source.
        /// </summary>
        public InputBin SelectedPaperSource
        {
            get { return _pdf.PrinterPaperSource; }

            set
            {
                if (_pdf.PrinterPaperSource == value)
                {
                    return;
                }
                _pdf.PrinterPaperSource = value;
                NotifyPropertyChanged("SelectedPaperSource", "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets or sets the maximum page number, used in the page slider in the view (XAML)
        /// </summary>
        /// <remarks>
        /// There is no setter, and so no notify as this value is stored on the model.
        /// </remarks>
        public int MaximumPageNumber
        {
            get
            {
                return _pdf.NumberOfPagesInPdf;
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for printing, sends notifies to update the view
        /// </summary>
        /// <value>
        /// The vertical alignment { top | center | bottom }
        /// </value>
        public VerticalAlignment PrintPdfVerticalAlignment
        {
            get
            {
                return _pdf.VerticalAlignment;
            }

            set
            {
                if (_pdf.VerticalAlignment == value)
                {
                    return;
                }
                _pdf.VerticalAlignment = value;
                NotifyPropertyChanged("PrintPdfVerticalAlignment", "PreviewOfOnePdfPage");
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for printing, sends notifies to update the view
        /// </summary>
        /// <value>
        /// The vertical alignment { left | center | right }
        /// </value>
        public HorizontalAlignment PrintPdfHorizontalAlignment
        {
            get
            {
                return _pdf.HorizontalAlignment;
            }

            set
            {
                if (_pdf.HorizontalAlignment == value)
                {
                    return;
                }
                _pdf.HorizontalAlignment = value;
                NotifyPropertyChanged("PrintPdfHorizontalAlignment", "PreviewOfOnePdfPage");
            }
        }

        /// <summary>
        /// Gets or sets the orientation for printing, sends notifies to update the view
        /// </summary>
        /// <value>
        /// The orientation { portrait | landscape }
        /// </value>
        public PageOrientation PrintPdfOrientation
        {
            get
            {
                return _pdf.PageOrientation;
            }

            set
            {
                if (_pdf.PageOrientation == value)
                {
                    return;
                }
                _pdf.PageOrientation = value;
                NotifyPropertyChanged("PrintPdfOrientation", "PreviewOfOnePdfPage", "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the document must scale to fit while printing
        /// </summary>
        /// <value>
        ///   <c>true</c> if the document scales to fit; otherwise, <c>false</c>.
        /// </value>
        public bool ScaleToFit
        {
            get
            {
                return _pdf.ScaleToFit;
            }

            set
            {
                if (_pdf.ScaleToFit == value)
                {
                    return;
                }
                _pdf.ScaleToFit = value;
                NotifyPropertyChanged("ScaleToFit", "PreviewOfOnePdfPage", "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the document must use the printable area
        /// </summary>
        /// <value>
        ///   <c>true</c> if document uses the printable area; otherwise, <c>false</c>.
        /// </value>
        public bool UsePrintableArea
        {
            get
            {
                return _pdf.UsePrintableArea;
            }

            set
            {
                if (_pdf.UsePrintableArea == value)
                {
                    return;
                }
                _pdf.UsePrintableArea = value;
                NotifyPropertyChanged("UsePrintableArea", "PreviewOfOnePdfPage", "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the document must printed on both sides
        /// </summary>
        /// <value>
        ///   <c>true</c> if document is printed on both sides; otherwise, <c>false</c>.
        /// </value>
        public bool PrintDuplex
        {
            get
            {
                return _pdf.PrintDuplex;
            }

            set
            {
                if (_pdf.PrintDuplex == value)
                {
                    return;
                }
                _pdf.PrintDuplex = value;
                NotifyPropertyChanged("DocumentInformation");
            }
        }
        

        /// <summary>
        /// Gets or sets the current page number in the PDF model
        /// </summary>
        /// <value>
        /// The current page number.
        /// </value>
        public int CurrentPageNumber
        {
            get
            {
                // __pdfModel counts from 0, the view counts from 1, translate this here.
                return _pdf.PageNr + 1;
            }

            set
            {
                int newPageNr = System.Math.Max(0, value - 1); // counting from 0 here
                newPageNr = System.Math.Min(_pdf.NumberOfPagesInPdf - 1, newPageNr);
                if (newPageNr == _pdf.PageNr)
                {
                    return;
                }
                _pdf.PageNr = newPageNr;
                NotifyPropertyChanged("CurrentPageNumber", "PreviewOfOnePdfPage", "DocumentInformation");
            }
        }

        /// <summary>
        /// Gets or sets the print page range.
        /// </summary>
        /// <value>
        /// The print page range.
        /// </value>
        public PrintPageRange PrintPageRange
        {
            get
            {
                return _pdf.PrintPageRange;
            }

            set
            {
                if (_pdf.PrintPageRange == value)
                {
                    return;
                }
                _pdf.PrintPageRange = value;
                NotifyPropertyChanged("PrintPageRange", "PrintRangeFirst", "PrintRangeLast");
            }
        }

        /// <summary>
        /// Gets or sets the first page in a print range
        /// </summary>
        /// <value>
        /// The page number.
        /// </value>
        public string PrintRangeFirst
        {
            // __pdfModel counts the pages from 0, the view counts from 1, translate this here.
            get
            {
                int firstPageCountingFromOne = _pdf.FirstPageToPrint + 1;
                return (_pdf.PrintPageRange == PrintPageRange.Range)
                       ? firstPageCountingFromOne.ToString(CultureInfo.InvariantCulture)
                       : "From:";
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                int pageNumber;
                if (!int.TryParse(value, out pageNumber))
                {
                    return;
                }

                pageNumber = Math.Max(1, pageNumber);
                pageNumber = Math.Min(_pdf.NumberOfPagesInPdf + 1, pageNumber);
                if (pageNumber == _pdf.FirstPageToPrint + 1)
                {
                    return;
                }

                _pdf.FirstPageToPrint = pageNumber - 1;
                NotifyPropertyChanged("PrintRangeFirst");
            }
        }

        /// <summary>
        /// Gets or sets the last page to print
        /// </summary>
        /// <value>
        /// The page number
        /// </value>
        public string PrintRangeLast
        {
            // __pdfModel counts the pages from 0, the view counts from 1, translate this here.
            get
            {
                int lastPageCountingFromOne = _pdf.LastPageToPrint + 1;
                return _pdf.PrintPageRange == PrintPageRange.Range
                           ? lastPageCountingFromOne.ToString(CultureInfo.InvariantCulture)
                           : "To:";
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                int pageNumber;
                if (!int.TryParse(value, out pageNumber))
                {
                    return;
                }

                pageNumber = Math.Max(1, pageNumber);
                pageNumber = Math.Min(_pdf.NumberOfPagesInPdf, pageNumber);
                var lastPageCountingFromOne = _pdf.LastPageToPrint;
                if (lastPageCountingFromOne == pageNumber)
                {
                    return;
                }

                _pdf.LastPageToPrint = pageNumber - 1;
                NotifyPropertyChanged("PrintRangeLast");
            }
        }

        /// <summary>
        /// Gets the PDF preview.
        /// </summary>
        public Canvas PreviewOfOnePdfPage
        {
            get
            {
                if (_previewCanvas == null)
                {
                    _previewCanvas = new Canvas { Width = PreviewSize.Width, Height = PreviewSize.Height };
                }
                else if ((int)_previewCanvas.Width != (int)PreviewSize.Width
                      || (int)_previewCanvas.Height != (int)PreviewSize.Height)
                {
                    _previewCanvas.Children.Clear();
                    _previewCanvas = null;
                    _previewCanvas = new Canvas { Width = PreviewSize.Width, Height = PreviewSize.Height };
                }
                else
                {
                    _previewCanvas.Children.Clear();
                }
                _pdf.DrawPreviewOfOnePdfPage(ref _previewCanvas);
                return _previewCanvas;
            }
        }


        private void PrintProgressInformation(int n)
        {
            _printingPageNumber = n;
            NotifyPropertyChanged("DocumentInformation");
        }
        /// <summary>
        /// Gets or sets the information of the document in a human readable format
        /// </summary>
        /// <value>
        /// The document information.
        /// </value>
        public String DocumentInformation
        {
            get
            {
                var docInfo = new StringBuilder("PDF document:");
                if (_printingPageNumber >= 0)
                {
                    docInfo.AppendFormat("Printing page {0}\n", _printingPageNumber);
                }
                else if (_pdf.PdfDocument != null)
                {
                    var pdfSize = _pdf.CurrentPdfPageSize;
                    var printSize = _pdf.PrinterPaperSize;
                    docInfo.AppendFormat(" {0} pages\n", _pdf.NumberOfPagesInPdf);
                    docInfo.AppendFormat("PDF page size = {0:f2}\" x {1:f2}\"\n", pdfSize.Width / 72.00, pdfSize.Height / 72.00);
                    docInfo.AppendFormat("Printer paper size = {0:f2}\" x {1:f2}\"\n", printSize.Width / 96, printSize.Height / 96);
                    if (_pdf.UsePrintableArea) 
                    {
                        var printableArea = _pdf.PrinterPaperImageableArea;
                        docInfo.AppendFormat("Printable area = {0:f2}\" x {1:f2}\"\n", 
                                             printableArea.ExtentWidth / 96, 
                                             printableArea.ExtentHeight / 96);
                    }
                    if (_pdf.PrintDuplex)
                    {
                        docInfo.AppendFormat("Duplex printing = ON\n");
                    }
                    docInfo.AppendFormat("Scale to print = {0:N2}%", _pdf.GetScaleToFitPdfPageOnPrinterPaper() * 100);
                }
                return docInfo.ToString();
            }

            set { } // user interface wont update without it
        }

        /// <summary>
        /// User interface action: the user wants to print the PDF document
        /// </summary>
        public void UserActionStartPrint()
        {
            _pdf.PrintPdfDocument();
        }

        /// <summary>
        /// User interface action: the user wants to to cancel this dialog
        /// </summary>
        public void UserActionCancel()
        {
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// User interface action: the user wants to see the previous page
        /// </summary>
        public void UserActionPreviousPage()
        {
            CurrentPageNumber--;
        }

        /// <summary>
        /// User interface action: the user wants to see the next page
        /// </summary>
        public void UserActionNextPage()
        {
            CurrentPageNumber++;
        }

        /// <summary>
        /// Notifies the property changed.
        /// This method is called by the Set accessor of each property. 
        /// The CallerMemberName attribute that is applied to the optional propertyName 
        /// parameter causes the property name of the caller to be substituted as an argument. 
        /// </summary>
        /// <param name="propertyNames">
        /// one or more names of properties that have been changed
        /// </param>
        private void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null && ViewIsLoaded)
            {
                foreach (string propertyName in propertyNames)
                {
                    // check if the property names are valid, as they are not checked compile-time
                    var type = GetType();
                    if (type.GetProperty(propertyName) == null)
                    {
                        MessageBox.Show("This view model does not have the property '" + propertyName + "'");
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                    }
                }
            }
        }
    }
}
