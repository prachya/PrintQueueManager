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

               ConvertToXPSButton.Content = "Convert " + name + " to PNG...";
            }
         }
      }

      private void MenuItem_SavePNG(object sender, RoutedEventArgs e)
      {
         Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
         dialog.DefaultExt = "png";
         dialog.Filter = "png files (.png)|*.png"; // Filter files by extension

         Nullable<bool> result = dialog.ShowDialog();

         if (result == true)
         {
            string path = System.IO.Path.GetDirectoryName (dialog.FileName) + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileNameWithoutExtension( dialog.FileName );
            string extension = System.IO.Path.GetExtension(dialog.FileName);

            RenderSettings renderSettings = new RenderSettings();
            renderSettings.TextSettings.ResolveFont += new ResolveFontEventHandler(TextSettings_ResolveFont);

            ConvertToWpfOptions convertOptions = new ConvertToWpfOptions();
            Summary summary = new Summary();

            int count = _document.Pages.Count;
            double dpi = 300;

            for (int i = 0; i < count; i++)
            {
               System.Diagnostics.Trace.Write("converting page " + i);
               Progess.Text = "Converting page " + i;

               TallComponents.PDF.Rasterizer.Page page = _document.Pages[i];
               FixedPage fixedPage = page.ConvertToWpf(renderSettings, convertOptions, summary);

               // Create a new bitmap render target.
               double width = page.Width * dpi / 72.0;
               double height = page.Height * dpi / 72.0;
               System.Windows.Media.Imaging.RenderTargetBitmap bmp =
                  new System.Windows.Media.Imaging.RenderTargetBitmap((int)(width), (int)(height), dpi, dpi, System.Windows.Media.PixelFormats.Pbgra32);

               // Render the fixed page into the bitmap target.
               bmp.Render(fixedPage);


               // Write the bitmap to file.
               using (FileStream outStream = new FileStream(string.Format(@"{0}{1}{2}", path, i, extension), FileMode.Create))
               {
                  PngBitmapEncoder enc = new PngBitmapEncoder();
                  enc.Frames.Add(BitmapFrame.Create(bmp));
                  enc.Save(outStream);
               }
               Progess.Text = "Ready";

               // For some PDF documents this samples has a memory usage pattern that appears to be affected by a garbage collection issue in the
               // Large Object Heap. See https://www.simple-talk.com/dotnet/.net-framework/the-dangers-of-the-large-object-heap/
               // Strangely but true: calling GC.GetTotalMemory appears to be able to avoid that issue, so we do that here.

               GC.GetTotalMemory(true);
            }
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
