


Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Packaging
Imports System.Windows
Imports System.Windows.Documents
Imports System.Windows.Documents.Serialization
Imports System.Windows.Xps
Imports System.Windows.Xps.Packaging
Imports System.Xml
Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Fonts
Imports TallComponents.PDF.Rasterizer.Configuration
Imports TallComponents.PDF.Rasterizer.Diagnostics


Class Window1

   Dim _document As Document

   Private Sub MenuItem_Open(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
      Dim dialog As New Microsoft.Win32.OpenFileDialog()
      dialog.DefaultExt = ".pdf"
      dialog.Filter = "PDF documents (.pdf)|*.pdf" ' Filter files by extension

      Dim result As Boolean = dialog.ShowDialog()

      If result Then

         Dim pdfPath As String = dialog.FileName
         Using pdfFile As New FileStream(pdfPath, FileMode.Open, FileAccess.Read)

            _document = New Document(pdfFile)

            ConvertToXPSButton.IsEnabled = True

            Dim name As String = _document.DocumentInfo.Title
            If name Is Nothing Or name.Length = 0 Then
               name = pdfPath
            End If

            ConvertToXPSButton.Content = "Convert " & name & " to XPS..."
         End Using

      End If
    End Sub



   Private Sub MenuItem_Quit(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
      Close()
    End Sub



   Private Sub ConvertToXPSButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        ' Filter files by extension
        Dim dialog = New Microsoft.Win32.SaveFileDialog()
        dialog.DefaultExt = ".xps"
        dialog.Filter = "XPS documents (.xps)|*.xps"

        Dim result As System.Nullable(Of Boolean) = dialog.ShowDialog()
        If result = True Then
            Dim path__1 As String = dialog.FileName
            Dim xpsFilename As [String] = Path.GetFileNameWithoutExtension(path__1)
            Dim xpsPath As [String] = Path.GetDirectoryName(path__1)
            Dim xpsBigFileName As [String] = xpsPath & "/" & xpsFilename & "_big.xps"
            Dim xpsSmallFileName As [String] = xpsPath & "/" & xpsFilename & "_small.xps"
            compactXpsWriter(xpsBigFileName, xpsSmallFileName)
            ConvertToXPSButton.Content = "Convert done"
            Progess.Text = "Ready"
        End If
    End Sub




    '
    ' This converts a PDF file to 2 XPS files:
    '
    ' 1: a big one using the MS .NET XpsDocument API. 
    '    This will render all images to lossless compressed PNG files, which
    '    will be part of the resulting XPS file.
    '
    ' 2: a smaller one which is a copy of the previous XPS file, but in which all
    '    these PNG files are converted to JPEG files with lossy compression, 
    '
    Private Sub compactXpsWriter(bigXpsFileName As [String], smallXpsFileName As [String])
        If File.Exists(bigXpsFileName) Then
            File.Delete(bigXpsFileName)
        End If

        If File.Exists(smallXpsFileName) Then
            File.Delete(smallXpsFileName)
        End If

        ' first create the XPS file in the usual way,
        Dim bigXpsDocument As New XpsDocument(bigXpsFileName, FileAccess.ReadWrite)
        Dim bigXpsWriter As XpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(bigXpsDocument)
        Dim bigCollator As SerializerWriterCollator = bigXpsWriter.CreateVisualsCollator()
        bigCollator.BeginBatchWrite()

        ' TAllComponents PDF rasterizer part: convert the PDF document to an XPS file.
        ' XPS can become big if images are involved, as they appear to be put in the
        ' file at full resolution, no matter what the output resolution is. 
        ' Also: they are all converted to PNGs, so JPEG compression gets lost.
        Dim renderSettings As New RenderSettings()
        AddHandler renderSettings.TextSettings.ResolveFont, AddressOf TextSettings_ResolveFont
        Dim convertOptions As New ConvertToWpfOptions()
        Dim summary As New Summary()
        Dim count As Integer = _document.Pages.Count
        For pageNr As Integer = 0 To count - 1
            Dim fixedPage As FixedPage = _document.Pages(pageNr).ConvertToWpf(renderSettings, convertOptions, summary)

            ' The ticket below determines the size of the page. so we must pass one if we
            ' want the XPS page to assume the size of the pdf page.
            Dim ticket As New System.Printing.PrintTicket()
            ticket.PageMediaSize = New System.Printing.PageMediaSize(fixedPage.Width, fixedPage.Height)

            ' We return pages with a single canvas as their child. WPF does not allow us
            ' to pass a FixedPage to the collator, but we can pass the canvas.
            bigCollator.Write(fixedPage.Children(0), ticket)
        Next
        bigCollator.EndBatchWrite()


        ' Second: process XPS again and use lossy image compression to make 
        ' the images smaller. Essentially the PNG files are converted to JPEG files
        ' After that, the references (which originally included the "png" extension in the
        ' name) must be modified too.
        Dim fixedDocSeqReader As IXpsFixedDocumentSequenceReader = bigXpsDocument.FixedDocumentSequenceReader
        Dim fixedDocuments As ICollection(Of IXpsFixedDocumentReader) = fixedDocSeqReader.FixedDocuments
        Dim enumerator As IEnumerator(Of IXpsFixedDocumentReader) = fixedDocuments.GetEnumerator()
        enumerator.MoveNext()
        Dim fixedPages As ICollection(Of IXpsFixedPageReader) = enumerator.Current.FixedPages
        Dim smallXpsDocument As New XpsDocument(smallXpsFileName, FileAccess.ReadWrite)
        Dim smallDocumentSequenceWriter As IXpsFixedDocumentSequenceWriter = smallXpsDocument.AddFixedDocumentSequence()

        ' This is a list containing the URI to the old and big PNG files and also the URI to the 
        ' new and smaller JPEG files.
        Dim replacedImageUris = New Dictionary(Of [String], [String])()

        ' Iterate through all documents in the XPS file (usually just one)
        For Each bigDocumentReader As IXpsFixedDocumentReader In fixedDocSeqReader.FixedDocuments
            ' Iterate through all pages in this XPS document
            Dim smallDocumentWriter As IXpsFixedDocumentWriter = smallDocumentSequenceWriter.AddFixedDocument()
            For Each bigPage As IXpsFixedPageReader In bigDocumentReader.FixedPages
                ' Create a smaller copy of this page with everything that the original big one has.
                Dim smallPageWriter As IXpsFixedPageWriter = smallDocumentWriter.AddFixedPage()

                ' Collection of all resources for this page. 
                '   Key: "XpsImage", "XpsFont" 
                '   Value: List of XpsImage or XpsFont
                Dim smallPageResources As New Dictionary(Of String, List(Of XpsResource))()

                ' In each page, copy any colorcontext to the new page
                Dim colorContexts = bigPage.ColorContexts
                If colorContexts.Count > 0 Then
                    For Each colorContext As XpsColorContext In bigPage.ColorContexts
                        Dim cc = smallPageWriter.AddColorContext()
                        cc.Uri = colorContext.Uri
                        cc.Commit()
                    Next
                End If

                ' In each page, copy any font to the new page
                Dim xpsFonts = bigPage.Fonts
                If xpsFonts.Count > 0 Then
                    Dim xpsFontResources = New List(Of XpsResource)()
                    ' not necessary in this context
                    ' see http://dedjo.blogspot.nl/2008/04/converting-fixeddocument-xpsdocument.html
                    ' if this code is used in another context
                    For Each font As XpsFont In bigPage.Fonts
                    Next
                    smallPageResources.Add("XpsFont", xpsFontResources)
                End If

                ' Convert the PNG to JPEG files. 
                ' This alone may make the XPS files much smaller.
                ' If this is not enough, you may also reduce to resolution of these.
                Dim xpsImages = bigPage.Images
                If xpsImages.Count > 0 Then
                    Dim xpsImageResources As New List(Of XpsResource)()
                    For Each image As XpsImage In xpsImages
                        If Not smallPageResources.ContainsKey(image.Uri.ToString()) Then
                            Dim bigImage As XpsImage = bigPage.GetImage(image.Uri)
                            Dim smallImage As XpsImage = smallPageWriter.AddImage(XpsImageType.JpegImageType)
                            Dim jpegImage As System.Drawing.Image = System.Drawing.Image.FromStream(bigImage.GetStream())
                            jpegImage.Save(smallImage.GetStream(), System.Drawing.Imaging.ImageFormat.Jpeg)
                            smallImage.Commit()
                            replacedImageUris.Add(image.Uri.ToString(), smallImage.Uri.ToString())
                        End If
                    Next
                    smallPageResources.Add("XpsImage", xpsImageResources)
                End If

                ' not necessary in this context
                For Each resourceDictionary As XpsResourceDictionary In bigPage.ResourceDictionaries
                Next

                Dim storyFragment As XpsStructure = bigPage.StoryFragment
                ' not necessary in this context
                If storyFragment IsNot Nothing Then
                End If

                ' Now process and copy the XML structure of the fixedpage, replacing all references to
                ' the new compressed images
                While bigPage.XmlReader.Read()
                    Dim bigPageInXml = bigPage.XmlReader.ReadOuterXml()
                    Dim doc As New XmlDocument()
                    doc.LoadXml(bigPageInXml)

                    ' Copy all namespaces of the XML document to the namespacemanager.
                    ' Note the "xmlns", this one is needed too and its given the name "ns",
                    ' which is later used to find the ImageBrush nodes
                    Dim nsmgr As New XmlNamespaceManager(doc.NameTable)
                    For Each attr As XmlAttribute In doc.SelectSingleNode("/*").Attributes
                        If attr.Name = "xmlns" Then
                            nsmgr.AddNamespace("ns", attr.Value)
                        End If

                        If attr.Prefix = "xmlns" Then
                            nsmgr.AddNamespace(attr.LocalName, attr.Value)
                        End If
                    Next

                    ' Now find all the ImageBrush nodes, these may contain the references to the PNG files.
                    Dim nodes As System.Xml.XmlNodeList = doc.SelectNodes("//ns:ImageBrush", nsmgr)
                    For Each node As XmlNode In nodes
                        ' replace the reference in the "ImageSource" attribure
                        Dim imageSourceAttribute = node.Attributes("ImageSource")
                        Dim oldUri As [String] = imageSourceAttribute.Value
                        If replacedImageUris.ContainsKey(oldUri) Then
                            Dim newUri As [String] = replacedImageUris(oldUri)
                            imageSourceAttribute.Value = newUri
                        End If
                    Next

                    ' This will copy the modified XML to the new and smaller page, 
                    ' and it will take care of the these of errors: "Specified element is already the logical
                    ' child of another element. Disconnect it first."
                    Dim xmlWriter = smallPageWriter.XmlWriter
                    xmlWriter.WriteRaw(doc.OuterXml)
                End While
                smallPageWriter.Commit()
            Next
            smallDocumentWriter.Commit()
        Next
        smallDocumentSequenceWriter.Commit()
        bigXpsDocument.Close()
        smallXpsDocument.Close()
    End Sub


    Private Sub TextSettings_ResolveFont(ByVal sender As TextRenderSettings, ByVal args As ResolveFontEventArgs)

        System.Diagnostics.Trace.WriteLine(String.Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath))

        If args.FontLocation <> FontLocation.System Then
            Select Case args.PdfFontName
                Case "Times-Roman"
                    args.SystemFontName = "Times New Roman"
                Case "Times-Bold"
                    args.SystemFontName = "Times New Roman"
                    args.Bold = True
                Case "Times-Italic"
                    args.SystemFontName = "Times New Roman"
                    args.Italic = True
                Case "Helvetica"
                    args.SystemFontName = "Arial Unicode MS"
                Case "Helvetica-Bold"
                    args.SystemFontName = "Arial Unicode MS"
                    args.Bold = True
                Case "Helvetica-Italic"
                    args.SystemFontName = "Arial Unicode MS"
                    args.Italic = True
            End Select
        End If

    End Sub

End Class
