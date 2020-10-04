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
               RenderSettings renderSettings = new RenderSettings();
               Summary summary = new Summary();
               renderSettings.TextSettings.ResolveFont += new ResolveFontEventHandler(TextSettings_ResolveFont);
               //renderSettings.ImageSettings.RenderImages = false;
               //renderSettings.TextSettings.RenderText = false;

               ConvertToWpfOptions convertOptions = new ConvertToWpfOptions();

               //convertOptions.ConvertToImages = true;

               // Converting large documents can take a while, so we only convert 10.
               DocumentViewer.Document = _document.ConvertToWpf(renderSettings, convertOptions, 0, 9, summary);
            }
         }
      }

      void TextSettings_ResolveFont(TextRenderSettings sender, ResolveFontEventArgs args)
      {
         System.Diagnostics.Trace.WriteLine(String.Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath));

         if (args.FontLocation != FontLocation.System)
         {
            // We try to map all fonts on to system fonts.

            // First handle a number of common extensions.

            string fontName = args.PdfFontName.ToLower();

            if (fontName.EndsWith("bold"))
            {
               if (fontName.EndsWith("-bold") || fontName.EndsWith(",bold"))
               {
                  fontName = args.PdfFontName.Remove(fontName.Length - 5);
               }
               else
               {
                  fontName = args.PdfFontName.Remove(fontName.Length - 4);
               }
               args.Bold = true;
            }
            else if (fontName.EndsWith("italic"))
            {
               if (fontName.EndsWith("-italic") || fontName.EndsWith(",italic"))
               {
                  fontName = args.PdfFontName.Remove(fontName.Length - 7);
               }
               else
               {
                  fontName = args.PdfFontName.Remove(fontName.Length - 6);
               }
               args.Italic = true;
            }
            else if (fontName.EndsWith("regular"))
            {
               if (fontName.EndsWith("-regular") || fontName.EndsWith(",regular"))
               {
                  fontName = args.PdfFontName.Remove(fontName.Length - 8);
               }
               else
               {
                  fontName = args.PdfFontName.Remove(fontName.Length - 7);
               }
            }
            else
            {
               fontName = args.PdfFontName;
            }

            switch (fontName)
            {
               case "TimesNewRoman":
                  args.SystemFontName = "Times New Roman";
                  break;
               case "LucidaConsole":
                  args.SystemFontName = "Lucida Console";
                  break;
               case "Univers-Condensed":
                  args.SystemFontName = "Univers Condensed Medium";
                  break;
               case "GillSansMT-Schlbk":
                  args.SystemFontName = "Gill Sans MT";
                  break;
               case "GillSansMT-SchlbkBold":
                  args.SystemFontName = "Gill Sans MT Bold";
                  break;
               case "GillSansMT-SchlbkItalic":
                  args.SystemFontName = "Gill Sans MT Italic";
                  break;
               case "Courier":
                  args.SystemFontName = "Courier New";
                  break;
               case "Times-Roman":
                  args.SystemFontName = "Times New Roman";
                  break;
               case "Times":
                  args.SystemFontName = "Times New Roman";
                  break;
               case "TimesNewRomanPSMT":
                  args.SystemFontName = "Times New Roman";
                  break;
               case "Helvetica":
                  args.SystemFontName = "Arial";
                  break;
               case "ArialNarrow":
                  args.SystemFontName = "Arial Narrow";
                  break;
               default:

                  if (!args.Embedded)
                  {
                     // We map all other non-embedded fonts to Arial unicode.
                     args.SystemFontName = "Arial Unicode MS";
                  }
                  else
                  {
                     // For embedded fonts, we try to load a system font with the
                     // same name.
                     args.SystemFontName = args.PdfFontName;
                  }
                  break;
            }
         }
      }

      private void MenuItem_SaveXPS(object sender, RoutedEventArgs e)
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
            xw.Write((FixedDocument)DocumentViewer.Document);
            xpsd.Close();
         }
      }

      private void MenuItem_Quit(object sender, RoutedEventArgs e)
      {
         Close();
      }

      private Document _document = null;

      private void MenuItem_Click(object sender, RoutedEventArgs e)
      {

      }
   }
}
