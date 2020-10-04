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

''' <summary>
''' Interaction logic for Window1.xaml
''' </summary>
Partial Public Class Window1
   Inherits Window
   Public Sub New()
      InitializeComponent()
   End Sub

   Private Sub MenuItem_Open(ByVal sender As Object, ByVal e As RoutedEventArgs)
      Dim dialog As New Microsoft.Win32.OpenFileDialog()
      dialog.DefaultExt = ".pdf"
      dialog.Filter = "PDF documents (.pdf)|*.pdf"
      ' Filter files by extension
      Dim result As Nullable(Of Boolean) = dialog.ShowDialog()

      If result = True Then
         Dim path As String = dialog.FileName

         Using file As New FileStream(path, FileMode.Open, FileAccess.Read)
            _document = New Document(file)
            Dim renderSettings As New RenderSettings()
            Dim summary As New Summary()
            AddHandler renderSettings.TextSettings.ResolveFont, New ResolveFontEventHandler(AddressOf TextSettings_ResolveFont)
            'renderSettings.ImageSettings.RenderImages = false;
            'renderSettings.TextSettings.RenderText = false;

            Dim convertOptions As New ConvertToWpfOptions()

            'convertOptions.ConvertToImages = true;

            ' Converting large documents can take a while, so we only convert 10.
            DocumentViewer.Document = _document.ConvertToWpf(renderSettings, convertOptions, 0, 9, summary)
         End Using
      End If
   End Sub

   Private Sub TextSettings_ResolveFont(ByVal sender As TextRenderSettings, ByVal args As ResolveFontEventArgs)
      System.Diagnostics.Trace.WriteLine([String].Format("font ({0}, {1}, {2})", args.PdfFontName, args.SystemFontName, args.FontPath))

      If args.FontLocation <> FontLocation.System Then
         ' We try to map all fonts on to system fonts.

         ' First handle a number of common extensions.

         Dim fontName As String = args.PdfFontName.ToLower()

         If fontName.EndsWith("bold") Then
            If fontName.EndsWith("-bold") OrElse fontName.EndsWith(",bold") Then
               fontName = args.PdfFontName.Remove(fontName.Length - 5)
            Else
               fontName = args.PdfFontName.Remove(fontName.Length - 4)
            End If
            args.Bold = True
         ElseIf fontName.EndsWith("italic") Then
            If fontName.EndsWith("-italic") OrElse fontName.EndsWith(",italic") Then
               fontName = args.PdfFontName.Remove(fontName.Length - 7)
            Else
               fontName = args.PdfFontName.Remove(fontName.Length - 6)
            End If
            args.Italic = True
         ElseIf fontName.EndsWith("regular") Then
            If fontName.EndsWith("-regular") OrElse fontName.EndsWith(",regular") Then
               fontName = args.PdfFontName.Remove(fontName.Length - 8)
            Else
               fontName = args.PdfFontName.Remove(fontName.Length - 7)
            End If
         Else
            fontName = args.PdfFontName
         End If

         Select Case fontName
            Case "TimesNewRoman"
               args.SystemFontName = "Times New Roman"
               Exit Select
            Case "LucidaConsole"
               args.SystemFontName = "Lucida Console"
               Exit Select
            Case "Univers-Condensed"
               args.SystemFontName = "Univers Condensed Medium"
               Exit Select
            Case "GillSansMT-Schlbk"
               args.SystemFontName = "Gill Sans MT"
               Exit Select
            Case "GillSansMT-SchlbkBold"
               args.SystemFontName = "Gill Sans MT Bold"
               Exit Select
            Case "GillSansMT-SchlbkItalic"
               args.SystemFontName = "Gill Sans MT Italic"
               Exit Select
            Case "Courier"
               args.SystemFontName = "Courier New"
               Exit Select
            Case "Times-Roman"
               args.SystemFontName = "Times New Roman"
               Exit Select
            Case "Times"
               args.SystemFontName = "Times New Roman"
               Exit Select
            Case "TimesNewRomanPSMT"
               args.SystemFontName = "Times New Roman"
               Exit Select
            Case "Helvetica"
               args.SystemFontName = "Arial"
               Exit Select
            Case "ArialNarrow"
               args.SystemFontName = "Arial Narrow"
               Exit Select
            Case Else

               If Not args.Embedded Then
                  ' We map all other non-embedded fonts to Arial unicode.
                  args.SystemFontName = "Arial Unicode MS"
               Else
                  ' For embedded fonts, we try to load a system font with the
                  ' same name.
                  args.SystemFontName = args.PdfFontName
               End If
               Exit Select
         End Select
      End If
   End Sub

   Private Sub MenuItem_SaveXPS(ByVal sender As Object, ByVal e As RoutedEventArgs)
      Dim dialog As New Microsoft.Win32.SaveFileDialog()
      dialog.DefaultExt = ".xps"
      dialog.Filter = "XPS documents (.xps)|*.xps"
      ' Filter files by extension
      Dim result As Nullable(Of Boolean) = dialog.ShowDialog()

      If result = True Then
         Dim path As String = dialog.FileName

         Dim xpsd As New XpsDocument(path, FileAccess.Write)
         Dim xw As XpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(xpsd)
         xw.Write(DirectCast(DocumentViewer.Document, FixedDocument))
         xpsd.Close()
      End If
   End Sub

   Private Sub MenuItem_Quit(ByVal sender As Object, ByVal e As RoutedEventArgs)
      Close()
   End Sub

   Private _document As Document = Nothing
End Class
