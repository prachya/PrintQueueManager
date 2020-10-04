using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Fonts;
using TallComponents.PDF.Rasterizer.Configuration;
using TallComponents.PDF.Rasterizer.Diagnostics;

namespace WpfApplication1
{
   /// <summary>
   /// Interaction logic for Window1.xaml
   /// </summary>
   public partial class Window1 : Window
   {
      public Window1()
      {
         InitializeComponent();

         ConvertToXPSButton.IsEnabled = false;
      }

      private void MenuItem_Open(object sender, RoutedEventArgs e)
      {
         Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
         dialog.DefaultExt = ".pdf";
         dialog.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extension

         Nullable<bool> result = dialog.ShowDialog();

         if (result == true)
         {
            string path = dialog.FileName;

            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
               _document = new Document(file);

               ConvertToXPSButton.IsEnabled = true;

               string name = _document.DocumentInfo.Title;
               if (name == null || name.Length == 0)
               {
                  name = path;
               }

               ConvertToXPSButton.Content = "Convert " + name + " to XPS...";
            }
         }
      }

      private void MenuItem_SaveXPSStreamed(object sender, RoutedEventArgs e)
      {
         Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
         dialog.DefaultExt = ".xps";
         dialog.Filter = "XPS documents (.xps)|*.xps"; // Filter files by extension

         Nullable<bool> result = dialog.ShowDialog();

         if (result == true)
         {
            string path = dialog.FileName;

            XpsDocument xpsd = new XpsDocument(path, FileAccess.Write);
            XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);

            SerializerWriterCollator collator = xw.CreateVisualsCollator();
            collator.BeginBatchWrite();

            RenderSettings renderSettings = new RenderSettings();
            renderSettings.TextSettings.ResolveFont += new ResolveFontEventHandler(TextSettings_ResolveFont);

            ConvertToWpfOptions convertOptions = new ConvertToWpfOptions();
            Summary summary = new Summary();

            int count = _document.Pages.Count;

            for (int i = 0; i < count; i++)
            {
               System.Diagnostics.Trace.Write("converting page " + i);
               Progess.Text = "Converting page " + i;

               FixedPage fixedPage = _document.Pages[i].ConvertToWpf(renderSettings, convertOptions, summary);

               // The ticket below determines the size of the page. so we must pass one if we
               // want the XPS page to assume the size of the pdf page.
               System.Printing.PrintTicket ticket = new System.Printing.PrintTicket();
               ticket.PageMediaSize = new System.Printing.PageMediaSize(fixedPage.Width, fixedPage.Height);

               // We return pages with a single canvas as their child. WPF does not allow us
               // to pass a FixedPage to the collator, but we can pass the canvas. Alternatively,
               // it appears to be possible to turn a fixed page into a "visual" that can be
               // used, but that is a bit 'irky'.
               collator.Write(fixedPage.Children[0], ticket);

               // Note: XPS can become big if images are involved, as they appear to be put in the
               // file at full resolution, no matter what the output resolution is. Also: they
               // are all converted to PNGs, so JPEG compression gets lost.
            }
            collator.EndBatchWrite();
            xpsd.Close();

            Progess.Text = "Ready";
         }
      }

      void TextSettings_ResolveFont(TextRenderSettings sender, ResolveFontEventArgs args)
      {
         System.Diagnostics.Trace.WriteLine(String.Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath));

         if (args.FontLocation != FontLocation.System)
         {
            switch (args.PdfFontName)
            {
               case "Times-Roman":
                  args.SystemFontName = "Times New Roman";
                  break;
               case "Times-Bold":
                  args.SystemFontName = "Times New Roman";
                  args.Bold = true;
                  break;
               case "Times-Italic":
                  args.SystemFontName = "Times New Roman";
                  args.Italic = true;
                  break;
               case "Helvetica":
                  args.SystemFontName = "Arial Unicode MS";
                  break;
               case "Helvetica-Bold":
                  args.SystemFontName = "Arial Unicode MS";
                  args.Bold = true;
                  break;
               case "Helvetica-Italic":
                  args.SystemFontName = "Arial Unicode MS";
                  args.Italic = true;
                  break;
            }
         }
      }

      private void MenuItem_Quit(object sender, RoutedEventArgs e)
      {
         Close();
      }

      private Document _document = null;

   }
}
