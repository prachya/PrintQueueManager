

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;
using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Fonts;
using TallComponents.PDF.Rasterizer.Configuration;
using TallComponents.PDF.Rasterizer.Diagnostics;


namespace WpfApplication1
{
   /// <summary>
   /// Interaction logic for Window1.xaml
   /// </summary>
   public partial class Window1
   {
      public Window1()
      {
         InitializeComponent();

         ConvertToXPSButton.IsEnabled = false;
      }

      private void MenuItemOpen(object sender, RoutedEventArgs e)
      {
         var dialog = new Microsoft.Win32.OpenFileDialog
                         {
                            // Filter files by extension
                            DefaultExt = ".pdf",
                            Filter = "PDF documents (.pdf)|*.pdf"
                         };

         bool? result = dialog.ShowDialog();
         if (result == true)
         {
            String path = dialog.FileName;
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
               _document = new Document(file);
               ConvertToXPSButton.IsEnabled = true;
               string name = _document.DocumentInfo.Title;
               if (string.IsNullOrEmpty(name))
               {
                  name = path;
               }

               ConvertToXPSButton.Content = "Convert " + name + " to XPS...";
            }
         }
      }

      private void MenuItemSaveXpsStreamed( object sender, RoutedEventArgs e )
      {

         var dialog = new Microsoft.Win32.SaveFileDialog
                         {
                            // Filter files by extension
                            DefaultExt = ".xps",
                            Filter = "XPS documents (.xps)|*.xps"
                         };

         bool? result = dialog.ShowDialog();
         if (result == true)
         {
            string path = dialog.FileName;
            String xpsFilename = Path.GetFileNameWithoutExtension(path);
            String xpsPath = Path.GetDirectoryName(path);
            String xpsBigFileName = xpsPath + "/" + xpsFilename + "_big.xps";
            String xpsSmallFileName = xpsPath + "/" + xpsFilename + "_small.xps";
            CompactXpsWriter(xpsBigFileName, xpsSmallFileName);
            ConvertToXPSButton.Content = "Convert done";
            Progess.Text = "Ready";
         }
      }

      void TextSettings_ResolveFont(TextRenderSettings sender, ResolveFontEventArgs args)
      {
         Trace.WriteLine(String.Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath));

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



      //
      // This converts a PDF file to 2 XPS files:
      //
      // 1: a big one using the MS .NET XpsDocument API. 
      //    This will render all images to lossless compressed PNG files, which
      //    will be part of the resulting XPS file.
      //
      // 2: a smaller one which is a copy of the previous XPS file, but in which all
      //    these PNG files are converted to JPEG files with lossy compression, 
      //
      private void CompactXpsWriter( String bigXpsFileName, String smallXpsFileName )
      {
         if ( File.Exists( bigXpsFileName ) )
         {
            File.Delete( bigXpsFileName );
         }

         if ( File.Exists( smallXpsFileName ) )
         {
            File.Delete( smallXpsFileName );
         }

         // first create the XPS file in the usual way,
         var bigXpsDocument = new XpsDocument( bigXpsFileName, FileAccess.ReadWrite );
         XpsDocumentWriter bigXpsWriter = XpsDocument.CreateXpsDocumentWriter( bigXpsDocument );
         SerializerWriterCollator bigCollator = bigXpsWriter.CreateVisualsCollator();
         Debug.Assert(bigCollator != null, "bigCollator != null");
         bigCollator.BeginBatchWrite();

         // TAllComponents PDF rasterizer part: convert the PDF document to an XPS file.
         // XPS can become big if images are involved, as they appear to be put in the
         // file at full resolution, no matter what the output resolution is. 
         // Also: they are all converted to PNGs, so JPEG compression gets lost.
         var renderSettings = new RenderSettings();
         renderSettings.TextSettings.ResolveFont += TextSettings_ResolveFont;
         var convertOptions = new ConvertToWpfOptions();
         var summary = new Summary();
         int count = _document.Pages.Count;
         for ( int pageNr = 0; pageNr < count; pageNr++ )
         {
            FixedPage fixedPage = _document.Pages[pageNr].ConvertToWpf( renderSettings, convertOptions, summary );

            // The ticket below determines the size of the page. so we must pass one if we
            // want the XPS page to assume the size of the pdf page.
            var ticket = new System.Printing.PrintTicket
                            {
                               PageMediaSize = new System.Printing.PageMediaSize(fixedPage.Width, fixedPage.Height)
                            };

            // We return pages with a single canvas as their child. WPF does not allow us
            // to pass a FixedPage to the collator, but we can pass the canvas.
            bigCollator.Write( fixedPage.Children[0], ticket );
         }
         bigCollator.EndBatchWrite();


         // Second: process XPS again and use lossy image compression to make 
         // the images smaller. Essentially the PNG files are converted to JPEG files
         // After that, the references (which originally included the "png" extension in the
         // name) must be modified too.
         IXpsFixedDocumentSequenceReader fixedDocSeqReader = bigXpsDocument.FixedDocumentSequenceReader;
         Debug.Assert(fixedDocSeqReader != null, "fixedDocSeqReader != null");
         ICollection<IXpsFixedDocumentReader> fixedDocuments = fixedDocSeqReader.FixedDocuments;
         IEnumerator<IXpsFixedDocumentReader> enumerator = fixedDocuments.GetEnumerator();
         enumerator.MoveNext();
         var smallXpsDocument = new XpsDocument( smallXpsFileName, FileAccess.ReadWrite, CompressionOption.Maximum );
         IXpsFixedDocumentSequenceWriter smallDocumentSequenceWriter = smallXpsDocument.AddFixedDocumentSequence();

         // This is a list containing the URI to the old and big PNG files and also the URI to the 
         // new and smaller JPEG files.
         var replacedImageUris = new Dictionary<String, String>();

         // Iterate through all documents in the XPS file (usually just one)
         foreach ( IXpsFixedDocumentReader bigDocumentReader in fixedDocSeqReader.FixedDocuments )
         {
            // Iterate through all pages in this XPS document
            IXpsFixedDocumentWriter smallDocumentWriter = smallDocumentSequenceWriter.AddFixedDocument();
            foreach ( IXpsFixedPageReader bigPage in bigDocumentReader.FixedPages )
            {
               // Create a smaller copy of this page with everything that the original big one has.
               IXpsFixedPageWriter smallPageWriter = smallDocumentWriter.AddFixedPage();

               // Collection of all resources for this page. 
               //   Key: "XpsImage", "XpsFont" 
               //   Value: List of XpsImage or XpsFont
               var smallPageResources = new Dictionary<string, List<XpsResource>>();

               // In each page, copy any colorcontext to the new page
               var colorContexts = bigPage.ColorContexts;
               if ( colorContexts.Count > 0 )
               {
                  foreach ( XpsColorContext colorContext in bigPage.ColorContexts )
                  {
                     var cc = smallPageWriter.AddColorContext();
                     cc.Uri = colorContext.Uri;
                     cc.Commit();
                  }
               }

               // In each page, copy any font to the new page
               var xpsFonts = bigPage.Fonts;
               if ( xpsFonts.Count > 0 )
               {
                  var xpsFontResources = new List<XpsResource>();
                  //  foreach ( XpsFont font in bigPage.Fonts )
                  //  {
                  //    not necessary in this context
                  //    see http://dedjo.blogspot.nl/2008/04/converting-fixeddocument-xpsdocument.html
                  //    if this code is used in another context
                  //  }
                  smallPageResources.Add( "XpsFont", xpsFontResources );
               }

               // Convert the PNG to JPEG files. 
               // This alone may make the XPS files much smaller.
               // If this is not enough, you may also reduce to resolution of these.
               var xpsImages = bigPage.Images;
               if ( xpsImages.Count > 0 )
               {
                  var xpsImageResources = new List<XpsResource>();
                  foreach ( XpsImage image in xpsImages )
                  {
                     if ( !smallPageResources.ContainsKey( image.Uri.ToString() ) )
                     {
                        XpsImage bigImage = bigPage.GetImage( image.Uri );
                        XpsImage smallImage = smallPageWriter.AddImage( XpsImageType.JpegImageType );
                        System.Drawing.Image jpegImage = System.Drawing.Image.FromStream( bigImage.GetStream() );
                        jpegImage.Save( smallImage.GetStream(), System.Drawing.Imaging.ImageFormat.Jpeg );
                        smallImage.Commit();
                        replacedImageUris.Add( image.Uri.ToString(), smallImage.Uri.ToString() );
                     }
                  }
                  smallPageResources.Add( "XpsImage", xpsImageResources );
               }

               //  foreach ( XpsResourceDictionary resourceDictionary in bigPage.ResourceDictionaries )
               //  {
               //    not necessary in this context
               //  }

               XpsStructure storyFragment = bigPage.StoryFragment;
               if ( storyFragment != null )
               {
                  // not necessary in this context
               }

               // Now process and copy the XML structure of the fixedpage, replacing all references to
               // the new compressed images
               while ( bigPage.XmlReader.Read() )
               {
                  var bigPageInXml = bigPage.XmlReader.ReadOuterXml();
                  var doc = new XmlDocument();
                  doc.LoadXml( bigPageInXml );

                  // Copy all namespaces of the XML document to the namespacemanager.
                  // Note the "xmlns", this one is needed too and its given the name "ns",
                  // which is later used to find the ImageBrush nodes
                  var nsmgr = new XmlNamespaceManager( doc.NameTable );
                  var selectSingleNode = doc.SelectSingleNode("/*");
                  if ( selectSingleNode != null )
                  {
                     Debug.Assert(selectSingleNode.Attributes != null, "selectSingleNode.Attributes != null");
                     foreach (XmlAttribute attr in selectSingleNode.Attributes)
                     {
                        if (attr.Name == "xmlns")
                        {
                           nsmgr.AddNamespace("ns", attr.Value);
                        }

                        if (attr.Prefix == "xmlns")
                        {
                           nsmgr.AddNamespace(attr.LocalName, attr.Value);
                        }
                     }
                  }

                  // Now find all the ImageBrush nodes, these may contain the references to the PNG files.
                  XmlNodeList nodes = doc.SelectNodes( "//ns:ImageBrush", nsmgr );
                  Debug.Assert(nodes != null, "nodes != null");
                  
                  foreach ( XmlNode node in nodes )
                  {
                     // replace the reference in the "ImageSource" attribure
                     Debug.Assert(node.Attributes != null, "node.Attributes != null");
                     var imageSourceAttribute = node.Attributes["ImageSource"];
                     String oldUri = imageSourceAttribute.Value;
                     if ( replacedImageUris.ContainsKey( oldUri ) )
                     {
                        String newUri = replacedImageUris[oldUri];
                        imageSourceAttribute.Value = newUri;
                     }
                  }

                  // This will copy the modified XML to the new and smaller page, 
                  // and it will take care of the these of errors: "Specified element is already the logical
                  // child of another element. Disconnect it first."
                  var xmlWriter = smallPageWriter.XmlWriter;
                  xmlWriter.WriteRaw( doc.OuterXml );
               }
               smallPageWriter.Commit();
            }
            smallDocumentWriter.Commit();
         }
         smallDocumentSequenceWriter.Commit();
         bigXpsDocument.Close();
         smallXpsDocument.Close();
      }

      private void MenuItemQuit(object sender, RoutedEventArgs e)
      {
         Close();
      }

      private Document _document;
   }
}
