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

            ConvertToXPSButton.Content = "Convert " & name & " to PNG..."
         End Using

      End If
   End Sub

   Private Sub MenuItem_Quit(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
      Close()
   End Sub

   Private Sub ConvertToPNGButton_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
      Dim dialog As New Microsoft.Win32.SaveFileDialog()
        dialog.DefaultExt = ".png"
        dialog.Filter = "PNG documents (.png)|*.png" ' Filter files by extension

      Dim result As Boolean = dialog.ShowDialog()

      If result Then

         Dim path As String = System.IO.Path.GetDirectoryName (dialog.FileName) + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileNameWithoutExtension( dialog.FileName )
         Dim extension As String = System.IO.Path.GetExtension(dialog.FileName)

         Dim renderSettings As New RenderSettings()
         AddHandler renderSettings.TextSettings.ResolveFont, AddressOf TextSettings_ResolveFont

         Dim convertOptions As New ConvertToWpfOptions()
         Dim summary As New Summary()

         Dim count As Integer = _document.Pages.Count
         Dim dpi As Double = 300

         Dim i As Integer
         For i = 0 To count - 1

            System.Diagnostics.Trace.Write("converting page " & i)
            Progess.Text = "Converting page " & i

            Dim page As TallComponents.PDF.Rasterizer.Page = _document.Pages(i)
            Dim fixedPage As FixedPage = page.ConvertToWpf(renderSettings, convertOptions, summary)

            'Create a new bitmap render target.
            Dim width As Double = page.Width * dpi / 72.0
            Dim height As Double = page.Height * dpi / 72.0
            Dim bmp As New System.Windows.Media.Imaging.RenderTargetBitmap(width, height, dpi, dpi, System.Windows.Media.PixelFormats.Pbgra32)

            'Render the fixed page into the bitmap target.
            bmp.Render(fixedPage)

            'Write the bitmap to file.
            Using outStream As New FileStream(string.Format("{0}{1}{2}", path, i, extension), FileMode.Create)
               
                  Dim enc As PngBitmapEncoder = new PngBitmapEncoder()
                  enc.Frames.Add(BitmapFrame.Create(bmp))
                  enc.Save(outStream)
               
            End Using

         Next

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
