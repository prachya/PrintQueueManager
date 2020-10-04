Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms

Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Configuration

Namespace PDFViewer

   Public Class MainForm
      Inherits System.Windows.Forms.Form

      Private document As Document = Nothing
      Private bitmap As Bitmap = Nothing
      Private selectedPage As Integer = -1
      Private path As String = Nothing
      Private dpi As Integer = 72
      Private renderSettings As RenderSettings = New RenderSettings()

      Private Const CAPTION As String = "PDFRasterizer.NET Viewer Application"

      Private WithEvents pagesListBox As System.Windows.Forms.ListBox
      Private splitter1 As System.Windows.Forms.Splitter
      Private WithEvents viewerPanel As System.Windows.Forms.Panel
      Private mainMenu1 As System.Windows.Forms.MainMenu
      Private WithEvents GetTIFFFileNameDialog As System.Windows.Forms.SaveFileDialog
      Private WithEvents fileOpen As System.Windows.Forms.MenuItem
      Private WithEvents fileClose As System.Windows.Forms.MenuItem
      Private WithEvents saveAsBitmap As System.Windows.Forms.MenuItem
      Private WithEvents SaveAsTiff As System.Windows.Forms.MenuItem
      Private WithEvents MenuItem5 As System.Windows.Forms.MenuItem
      Private WithEvents extractImages As System.Windows.Forms.MenuItem
      Private WithEvents dpi48 As System.Windows.Forms.MenuItem
      Private WithEvents dpi72 As System.Windows.Forms.MenuItem
      Private WithEvents dpi150 As System.Windows.Forms.MenuItem
      Private WithEvents dpi300 As System.Windows.Forms.MenuItem
      Private WithEvents MenuItem7 As System.Windows.Forms.MenuItem
      Private WithEvents backgroundColor As System.Windows.Forms.MenuItem
      Private WithEvents fileMenu As System.Windows.Forms.MenuItem
      Private WithEvents settingMenu As System.Windows.Forms.MenuItem
      Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
      Friend WithEvents highSpeed As System.Windows.Forms.MenuItem
      Friend WithEvents loadColorProfile As System.Windows.Forms.MenuItem
      Private WithEvents pageMenu As System.Windows.Forms.MenuItem

      Public Sub New()
         InitializeComponent()
         Me.Text = CAPTION

         TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
      End Sub

      Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
      Friend WithEvents fileSaveAsTiff As System.Windows.Forms.MenuItem

      Private Sub InitializeComponent()
         Me.pagesListBox = New System.Windows.Forms.ListBox
         Me.splitter1 = New System.Windows.Forms.Splitter
         Me.viewerPanel = New System.Windows.Forms.Panel
         Me.mainMenu1 = New System.Windows.Forms.MainMenu
         Me.fileMenu = New System.Windows.Forms.MenuItem
         Me.fileOpen = New System.Windows.Forms.MenuItem
         Me.fileClose = New System.Windows.Forms.MenuItem
         Me.MenuItem1 = New System.Windows.Forms.MenuItem
         Me.fileSaveAsTiff = New System.Windows.Forms.MenuItem
         Me.pageMenu = New System.Windows.Forms.MenuItem
         Me.saveAsBitmap = New System.Windows.Forms.MenuItem
         Me.SaveAsTiff = New System.Windows.Forms.MenuItem
         Me.MenuItem5 = New System.Windows.Forms.MenuItem
         Me.extractImages = New System.Windows.Forms.MenuItem
         Me.settingMenu = New System.Windows.Forms.MenuItem
         Me.dpi48 = New System.Windows.Forms.MenuItem
         Me.dpi72 = New System.Windows.Forms.MenuItem
         Me.dpi150 = New System.Windows.Forms.MenuItem
         Me.dpi300 = New System.Windows.Forms.MenuItem
         Me.MenuItem7 = New System.Windows.Forms.MenuItem
         Me.backgroundColor = New System.Windows.Forms.MenuItem
         Me.MenuItem2 = New System.Windows.Forms.MenuItem
         Me.highSpeed = New System.Windows.Forms.MenuItem
         Me.loadColorProfile = New System.Windows.Forms.MenuItem
         Me.GetTIFFFileNameDialog = New System.Windows.Forms.SaveFileDialog
         Me.SuspendLayout()
         '
         'pagesListBox
         '
         Me.pagesListBox.Dock = System.Windows.Forms.DockStyle.Left
         Me.pagesListBox.IntegralHeight = False
         Me.pagesListBox.Location = New System.Drawing.Point(0, 0)
         Me.pagesListBox.Name = "pagesListBox"
         Me.pagesListBox.Size = New System.Drawing.Size(120, 406)
         Me.pagesListBox.TabIndex = 0
         '
         'splitter1
         '
         Me.splitter1.Location = New System.Drawing.Point(120, 0)
         Me.splitter1.Name = "splitter1"
         Me.splitter1.Size = New System.Drawing.Size(3, 406)
         Me.splitter1.TabIndex = 1
         Me.splitter1.TabStop = False
         '
         'viewerPanel
         '
         Me.viewerPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
         Me.viewerPanel.Dock = System.Windows.Forms.DockStyle.Fill
         Me.viewerPanel.Location = New System.Drawing.Point(123, 0)
         Me.viewerPanel.Name = "viewerPanel"
         Me.viewerPanel.Size = New System.Drawing.Size(413, 406)
         Me.viewerPanel.TabIndex = 2
         '
         'mainMenu1
         '
         Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileMenu, Me.pageMenu, Me.settingMenu})
         '
         'fileMenu
         '
         Me.fileMenu.Index = 0
         Me.fileMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileOpen, Me.fileClose, Me.MenuItem1, Me.fileSaveAsTiff})
         Me.fileMenu.Text = "File"
         '
         'fileOpen
         '
         Me.fileOpen.Index = 0
         Me.fileOpen.Text = "Open..."
         '
         'fileClose
         '
         Me.fileClose.Index = 1
         Me.fileClose.Text = "Close"
         '
         'MenuItem1
         '
         Me.MenuItem1.Index = 2
         Me.MenuItem1.Text = "-"
         '
         'fileSaveAsTiff
         '
         Me.fileSaveAsTiff.Index = 3
         Me.fileSaveAsTiff.Text = "Save as multi-page TIFF"
         '
         'pageMenu
         '
         Me.pageMenu.Index = 1
         Me.pageMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.saveAsBitmap, Me.SaveAsTiff, Me.MenuItem5, Me.extractImages})
         Me.pageMenu.Text = "Page"
         '
         'saveAsBitmap
         '
         Me.saveAsBitmap.Index = 0
         Me.saveAsBitmap.Text = "Save as bitmap"
         '
         'SaveAsTiff
         '
         Me.SaveAsTiff.Index = 1
         Me.SaveAsTiff.Text = "Save as TIFF"
         '
         'MenuItem5
         '
         Me.MenuItem5.Index = 2
         Me.MenuItem5.Text = "-"
         '
         'extractImages
         '
         Me.extractImages.Index = 3
         Me.extractImages.Text = "Extract Images"
         '
         'settingMenu
         '
         Me.settingMenu.Index = 2
         Me.settingMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.dpi48, Me.dpi72, Me.dpi150, Me.dpi300, Me.MenuItem7, Me.backgroundColor, Me.MenuItem2, Me.highSpeed, Me.loadColorProfile})
         Me.settingMenu.Text = "Settings"
         '
         'dpi48
         '
         Me.dpi48.Index = 0
         Me.dpi48.Text = "48 DPI"
         '
         'dpi72
         '
         Me.dpi72.Checked = True
         Me.dpi72.Index = 1
         Me.dpi72.Text = "72 DPI"
         '
         'dpi150
         '
         Me.dpi150.Index = 2
         Me.dpi150.Text = "150 DPI"
         '
         'dpi300
         '
         Me.dpi300.Index = 3
         Me.dpi300.Text = "300 DPI"
         '
         'MenuItem7
         '
         Me.MenuItem7.Index = 4
         Me.MenuItem7.Text = "-"
         '
         'backgroundColor
         '
         Me.backgroundColor.Index = 5
         Me.backgroundColor.Text = "Background color"
         '
         'MenuItem2
         '
         Me.MenuItem2.Index = 6
         Me.MenuItem2.Text = "-"
         '
         'highSpeed
         '
         Me.highSpeed.Checked = True
         Me.highSpeed.Index = 7
         Me.highSpeed.Text = "High Speed Color Transform"
         '
         'loadColorProfile
         '
         Me.loadColorProfile.Index = 8
         Me.loadColorProfile.Text = "Load Color Profile..."
         '
         'GetTIFFFileNameDialog
         '
         Me.GetTIFFFileNameDialog.DefaultExt = "*.tif"
         Me.GetTIFFFileNameDialog.Filter = "TIFF files|*.tif|All files|*.*"
         '
         'MainForm
         '
         Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
         Me.ClientSize = New System.Drawing.Size(536, 406)
         Me.Controls.Add(Me.viewerPanel)
         Me.Controls.Add(Me.splitter1)
         Me.Controls.Add(Me.pagesListBox)
         Me.Menu = Me.mainMenu1
         Me.Name = "MainForm"
         Me.Text = "PDFViewer Demo Application"
         Me.ResumeLayout(False)

      End Sub

      <STAThread()> _
      Shared Sub Main()
         Application.Run(New MainForm)
      End Sub

      Private Overloads Sub close()
         document = Nothing
         If Not (Nothing Is bitmap) Then
            bitmap.Dispose()
            bitmap = Nothing
            viewerPanel.Invalidate()
         End If
         Me.pagesListBox.Items.Clear()
         Me.Text = CAPTION
      End Sub

      Private Sub selectPage(ByVal index As Integer)
         If Nothing Is document Then Return
         Try
            Cursor = Cursors.WaitCursor
            selectedPage = index
            Dim page As Page = document.Pages(selectedPage)
            If Not (Nothing Is bitmap) Then
               bitmap.Dispose()
            End If
            bitmap = New Bitmap(CType((page.Width / 72 * dpi), Integer), CType((page.Height / 72 * dpi), Integer))
            Using graph As Graphics = Graphics.FromImage(bitmap)
               Dim scale As Single = CType((bitmap.Width / page.Width), Single)
               graph.ScaleTransform(scale, scale)
               graph.SmoothingMode = SmoothingMode.AntiAlias
               page.Draw(graph, renderSettings)
            End Using
            viewerPanel.Invalidate()
            Me.Text = CAPTION + String.Format(" (page {0} of {1})", selectedPage, path)
         Finally
            Cursor = Cursors.Arrow
         End Try
      End Sub

      Private Sub fileOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fileOpen.Click
         close()
         Dim dialog As OpenFileDialog = New OpenFileDialog
         dialog.Filter = "PDF files|*.pdf|All files|*.*"
         If Windows.Forms.DialogResult.OK = dialog.ShowDialog Then
            path = dialog.FileName
            Using pdfFile As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read)
               document = New Document(pdfFile)
               Dim i As Integer = 0
               For i = 1 To document.Pages.Count
                  pagesListBox.Items.Add("Page " & i)
               Next i
               ' End Using
               Me.pagesListBox.SelectedIndex = 0
            End Using
         End If
      End Sub

      Private Sub pagesListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pagesListBox.SelectedIndexChanged
         Dim index As Integer = pagesListBox.SelectedIndex
         If index >= 0 AndAlso Not (Nothing Is document) AndAlso index < document.Pages.Count Then
            selectPage(index)
         End If
      End Sub

      Private Sub viewerPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles viewerPanel.Paint
         e.Graphics.Clear(Color.White)
         If Not (Nothing Is bitmap) Then
            Dim ratio As Single = CType(bitmap.Width, Single) / CType(bitmap.Height, Single)
            Dim displayWidth As Single
            Dim displayHeight As Single
            If ratio * viewerPanel.Height > viewerPanel.Width Then
               displayWidth = viewerPanel.Width - 10
               displayHeight = displayWidth / ratio
            Else
               displayHeight = viewerPanel.Height - 10
               displayWidth = displayHeight * ratio
            End If
            Using thumbnail As Image = bitmap.GetThumbnailImage(CType(displayWidth, Integer), CType(displayHeight, Integer), Nothing, New IntPtr)
               Dim x As Single = (viewerPanel.Width - displayWidth) / 2
               Dim y As Single = (viewerPanel.Height - displayHeight) / 2
               e.Graphics.DrawImage(thumbnail, x, y, displayWidth, displayHeight)
               e.Graphics.DrawRectangle(New Pen(Color.Gray), x, y, displayWidth, displayHeight)
            End Using
         End If
      End Sub

      Private Sub viewerPanel_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewerPanel.Resize
         viewerPanel.Invalidate()
      End Sub

      Private Sub fileClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fileClose.Click
         close()
      End Sub

      Private Sub fileSaveAsTiff_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fileSaveAsTiff.Click
         If Nothing Is document Then Return
         Dim options As ConvertToTiffOptions
         options = New ConvertToTiffOptions(dpi, TiffCompression.CcittG4)
         options.InvertImage = MessageBox.Show("Do you want to invert the image?", "Question", _
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes

         Dim fileName As String = String.Format("{0}.tif", path)
         Using tiffFile As FileStream = New FileStream(fileName, FileMode.Create)
            document.ConvertToTiff(tiffFile, options)
         End Using
         MessageBox.Show("TIFF was created.", fileName)
      End Sub

      Private Sub saveAsBitmap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveAsBitmap.Click
         If Nothing Is bitmap Then Return
         Dim fileName As String = String.Format("{0}_Page_{1}.png", path, selectedPage)
         bitmap.Save(fileName, ImageFormat.Png)
         MessageBox.Show("Bitmap was created.", fileName)
      End Sub

      Private Sub fileSaveAsTiff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsTiff.Click
         If Nothing Is document Then Return
         Dim options As ConvertToTiffOptions
         options = New ConvertToTiffOptions(dpi, TiffCompression.CcittG4)
         options.InvertImage = MessageBox.Show("Do you want to invert the image?", "Question", _
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes

         Dim fileName As String = String.Format("{0}_Page_{1}.tif", path, selectedPage)
         Using tiffFile As FileStream = New FileStream(fileName, FileMode.Create)
            document.Pages(selectedPage).ConvertToTiff(tiffFile, options)
         End Using
         MessageBox.Show("TIFF was created.", fileName)
      End Sub

      Private Sub extractImages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles extractImages.Click
         If Nothing Is document Then Return
         Dim images As ExtractedImageInfo() = document.Pages(selectedPage).ExtractImages()
         Dim imageCnt As Integer = 0
         For Each image As ExtractedImageInfo In images
            imageCnt = imageCnt + 1
            Dim fileName As String = String.Format("{0}_Page_{1}_Image_{2}.png", path, selectedPage, imageCnt)
            image.Bitmap.Save(fileName, ImageFormat.Png)
         Next image
         If imageCnt = 0 Then
            MessageBox.Show("No images found on this page.", _
                            String.Format("{0} Page {1}", path, selectedPage))
         Else
            MessageBox.Show(String.Format("{0} images have been extracted.", imageCnt), String.Format("{0}_Page_{1}_Image_*.png", path, selectedPage))
         End If
      End Sub

      Private Sub dpi48_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpi48.Click
         dpi48.Checked = True
         dpi72.Checked = False
         dpi150.Checked = False
         dpi300.Checked = False
         Me.dpi = 48
         selectPage(selectedPage)
      End Sub

      Private Sub dpi72_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpi72.Click
         dpi48.Checked = False
         dpi72.Checked = True
         dpi150.Checked = False
         dpi300.Checked = False
         Me.dpi = 72
         selectPage(selectedPage)
      End Sub

      Private Sub dpi150_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpi150.Click
         dpi48.Checked = False
         dpi72.Checked = False
         dpi150.Checked = True
         dpi300.Checked = False
         Me.dpi = 150
         selectPage(selectedPage)
      End Sub

      Private Sub dpi300_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dpi300.Click
         dpi48.Checked = False
         dpi72.Checked = False
         dpi150.Checked = False
         dpi300.Checked = True
         Me.dpi = 300
         selectPage(selectedPage)
      End Sub

      Private Sub backgroundColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles backgroundColor.Click
         If Nothing Is document Then Return
         Dim dialog As ColorDialog = New ColorDialog
         dialog.Color = renderSettings.ColorSettings.BackColor
         If dialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            renderSettings.ColorSettings.BackColor = dialog.Color
            selectPage(selectedPage)
         End If
      End Sub

      Private Sub highSpeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles highSpeed.Click
         highSpeed.Checked = Not highSpeed.Checked
         If highSpeed.Checked Then
            renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighSpeed
         Else
            renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality
         End If
         selectPage(selectedPage)
      End Sub

      Private Sub loadColorProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles loadColorProfile.Click
         Dim dialog As OpenFileDialog = New OpenFileDialog
         dialog.Filter = "ICC Color Profile|*.icm|All files|*.*"
         dialog.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.System) + "\spool\drivers\color"
         If Windows.Forms.DialogResult.OK = dialog.ShowDialog() Then
            renderSettings.ColorSettings.OutputColorProfile = New ColorProfile(New FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            selectPage(selectedPage)
         End If
      End Sub

   End Class

End Namespace
