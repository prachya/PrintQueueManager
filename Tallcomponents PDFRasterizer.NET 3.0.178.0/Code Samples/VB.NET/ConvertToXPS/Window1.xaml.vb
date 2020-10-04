Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Documents.Serialization
Imports System.Windows.Markup
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Xps
Imports System.Windows.Xps.Packaging

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
      Dim dialog As New Microsoft.Win32.SaveFileDialog()
      dialog.DefaultExt = ".xps"
      dialog.Filter = "XPS documents (.xps)|*.xps" ' Filter files by extension

      Dim result As Boolean = dialog.ShowDialog()

      If result Then

         Dim path As String = dialog.FileName

         Dim xpsd As New XpsDocument(path, FileAccess.Write)
         Dim xw As XpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsd)

         Dim collator As SerializerWriterCollator = xw.CreateVisualsCollator()
         collator.BeginBatchWrite()

         Dim renderSettings As New RenderSettings()
         AddHandler renderSettings.TextSettings.ResolveFont, AddressOf TextSettings_ResolveFont

         Dim convertOptions As New ConvertToWpfOptions()
         Dim summary As New Summary()

         Dim count As Integer = _document.Pages.Count

         Dim i As Integer
         For i = 0 To count - 1

            System.Diagnostics.Trace.Write("converting page " & i)
            Progess.Text = "Converting page " & i

            Dim fp As FixedPage = _document.Pages(i).ConvertToWpf(renderSettings, convertOptions, summary)

            ' The ticket below determines the size of the page. so we must pass one if we
            ' want the XPS page to assume the size of the pdf page.
            Dim ticket As New System.Printing.PrintTicket()
            ticket.PageMediaSize = New System.Printing.PageMediaSize(fp.Width, fp.Height)

            ' We return pages with a single canvas as their child. WPF does not allow us
            ' to pass a FixedPage to the collator, but we can pass the canvas. Alternatively,
            ' it appears to be possible to turn a fixed page into a "visual" that can be
            ' used, but that is a bit 'irky'.
            collator.Write(fp.Children(0), ticket)

            ' Note: XPS can become big if images are involved, as they appear to be put in the
            ' file at full resolution, no matter what the output resolution is. Also: they
            ' are all converted to PNGs, so JPEG compression gets lost.
         Next

         collator.EndBatchWrite()
         xpsd.Close()

         Progess.Text = "Ready"
      End If
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
