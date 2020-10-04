Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Drawing.Printing
Imports System.IO
Imports System.Windows.Forms

Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Configuration

Namespace PrintDemo

   Public Class MainForm
      Inherits System.Windows.Forms.Form
      Private toolBarButton3 As System.Windows.Forms.ToolBarButton
      Private toolBarButton5 As System.Windows.Forms.ToolBarButton
      Private pagesList As System.Windows.Forms.ListBox
      Private toolBar As System.Windows.Forms.ToolBar
      Private openFile As System.Windows.Forms.MenuItem
      Private fileMenu As System.Windows.Forms.MenuItem
      Private mainMenu As System.Windows.Forms.MainMenu
      Private imageList As System.Windows.Forms.ImageList

      Public Sub New()
         InitializeComponent()

         TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
      End Sub

      Private Sub InitializeComponent()
         Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(MainForm))
         Me.mainMenu = New System.Windows.Forms.MainMenu
         Me.fileMenu = New System.Windows.Forms.MenuItem
         Me.openFile = New System.Windows.Forms.MenuItem
         Me.imageList = New System.Windows.Forms.ImageList
         Me.toolBarButton3 = New System.Windows.Forms.ToolBarButton
         Me.toolBarButton5 = New System.Windows.Forms.ToolBarButton
         Me.toolBar = New System.Windows.Forms.ToolBar
         Me.pagesList = New System.Windows.Forms.ListBox
         Me.SuspendLayout()
         Me.mainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileMenu})
         Me.fileMenu.Index = 0
         Me.fileMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.openFile})
         Me.fileMenu.Text = "File"
         Me.openFile.Index = 0
         Me.openFile.Text = "Open..."
         AddHandler Me.openFile.Click, AddressOf Me.openFile_Click
         Me.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
         Me.imageList.ImageSize = New System.Drawing.Size(16, 16)
         Me.imageList.ImageStream = CType((resources.GetObject("imageList.ImageStream")), System.Windows.Forms.ImageListStreamer)
         Me.imageList.TransparentColor = System.Drawing.Color.Red
         Me.toolBarButton3.ImageIndex = 0
         Me.toolBarButton3.Tag = "print"
         Me.toolBarButton3.ToolTipText = "Print selection"
         Me.toolBarButton5.ImageIndex = 1
         Me.toolBarButton5.Tag = "preview"
         Me.toolBarButton5.ToolTipText = "Preview selection"
         Me.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat
         Me.toolBar.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.toolBarButton3, Me.toolBarButton5})
         Me.toolBar.DropDownArrows = True
         Me.toolBar.ImageList = Me.imageList
         Me.toolBar.Name = "toolBar"
         Me.toolBar.ShowToolTips = True
         Me.toolBar.Size = New System.Drawing.Size(292, 25)
         Me.toolBar.TabIndex = 0
         AddHandler Me.toolBar.ButtonClick, AddressOf Me.toolBar_ButtonClick
         Me.pagesList.Dock = System.Windows.Forms.DockStyle.Fill
         Me.pagesList.Location = New System.Drawing.Point(0, 25)
         Me.pagesList.Name = "pagesList"
         Me.pagesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
         Me.pagesList.Size = New System.Drawing.Size(292, 238)
         Me.pagesList.TabIndex = 1
         Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
         Me.ClientSize = New System.Drawing.Size(292, 266)
         Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.pagesList, Me.toolBar})
         Me.Menu = Me.mainMenu
         Me.Name = "Form1"
         Me.Text = "PDFRasterizer.NET Print Demo"
         Me.ResumeLayout(False)
      End Sub

      <STAThread()> _
      Shared Sub Main()
         Application.Run(New MainForm)
      End Sub

      Private Sub openFile_Click(ByVal sender As Object, ByVal e As System.EventArgs)
         ' ask the user for a PDF file
         Dim dialog As OpenFileDialog = New OpenFileDialog
         If dialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            Using pdfFile As New FileStream(dialog.FileName, FileMode.Open, FileAccess.Read)
               ' instantiate the PDF document class
               document = New Document(pdfFile)
               ' fill the list with all pages in the document
               pagesList.Items.Clear()
               Dim i As Integer = 0
               For i = 1 To document.Pages.Count
                  pagesList.Items.Add("page " & i)
               Next i
               If pagesList.Items.Count > 0 Then
                  pagesList.SelectedIndex = 0
               End If
            End Using
         End If
      End Sub

      Sub print()
         If Nothing Is document OrElse pagesList.SelectedIndices.Count = 0 Then Return

         selectedPages = pagesList.SelectedIndices.GetEnumerator
         selectedPages.Reset()
         If selectedPages.MoveNext Then
            Dim printDocument As PrintDocument = New PrintDocument

            printDocument.DocumentName = document.DocumentInfo.Title
            printDocument.DefaultPageSettings.Margins.Left = 0
            printDocument.DefaultPageSettings.Margins.Top = 0
            printDocument.OriginAtMargins = True

            Dim setupDialog As PageSetupDialog = New PageSetupDialog
            setupDialog.Document = printDocument
            If Windows.Forms.DialogResult.OK = setupDialog.ShowDialog Then
               printDocument.DefaultPageSettings = setupDialog.PageSettings
               printDocument.PrinterSettings = setupDialog.PrinterSettings
               AddHandler printDocument.PrintPage, AddressOf printPage
               printDocument.Print()
            End If
         End If
      End Sub

      Sub printPreview()
         If Nothing Is document OrElse pagesList.SelectedIndices.Count = 0 Then
            Return
         End If
         selectedPages = pagesList.SelectedIndices.GetEnumerator
         selectedPages.Reset()
         If selectedPages.MoveNext Then
            Dim printDocument As PrintDocument = New PrintDocument
            AddHandler printDocument.PrintPage, AddressOf printPage
            Dim previewDialog As PrintPreviewDialog = New PrintPreviewDialog
            previewDialog.Document = printDocument
            previewDialog.ShowDialog()
         End If
      End Sub

      Sub printPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
         e.Graphics.PageUnit = GraphicsUnit.Point
         If Not (Nothing Is selectedPages.Current) Then
            Dim pageIndex As Integer = CType(selectedPages.Current, Integer)
            Dim page As Page = document.Pages(pageIndex)
            pageIndex = pageIndex + 1
            autoPosition(e.Graphics, (e.PageSettings.PaperSize.Width * 72) / 100, (e.PageSettings.PaperSize.Height * 72) / 100, page.Width, page.Height)
            page.Draw(e.Graphics)
         End If
         e.HasMorePages = selectedPages.MoveNext
      End Sub

      Sub autoPosition(ByVal graphics As Graphics, ByVal printerWidth As Double, ByVal printerHeight As Double, ByVal pdfWidth As Double, ByVal pdfHeight As Double)

         Dim scaleX As Double = printerWidth / pdfWidth
         Dim scaleY As Double = printerHeight / pdfHeight
         Dim scale As Double = Math.Min(scaleX, scaleY)

         Dim scaledPdfWidth As Double = scale * pdfWidth
         Dim scaledPdfHeight As Double = scale * pdfHeight

         Dim dx As Double = (printerWidth - scaledPdfWidth) / 2
         Dim dy As Double = (printerHeight - scaledPdfHeight) / 2

         graphics.TranslateTransform(CSng(dx), CSng(dy))
         graphics.ScaleTransform(CSng(scale), CSng(scale))
      End Sub


      Private Sub toolBar_ButtonClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs)
         Select Case e.Button.Tag.ToString
            Case "print"
               If Not (Nothing Is document) Then
                  print()
               End If
               ' break
            Case "preview"
               If Not (Nothing Is document) Then
                  printPreview()
               End If
               ' break
         End Select
      End Sub
      Private selectedPages As IEnumerator
      Private document As Document
   End Class
End Namespace

