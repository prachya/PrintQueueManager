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
		Private selectedPage As Integer = -1
		Private path As String = Nothing
		Private dpi As Integer = 72
		Private renderSettings As New RenderSettings()
		Private drawJob As DrawJob = Nothing

		Private Const CAPTION As String = "PDFRasterizer.NET Viewer Application"

      Private WithEvents pagesListBox As System.Windows.Forms.ListBox
		Private splitter1 As System.Windows.Forms.Splitter
      Private WithEvents viewerPanel As DoubleBufferPanel
      Private WithEvents fileOpen As System.Windows.Forms.MenuItem
      Private WithEvents fileClose As System.Windows.Forms.MenuItem
      Private WithEvents dpi48 As System.Windows.Forms.MenuItem
      Private WithEvents dpi72 As System.Windows.Forms.MenuItem
      Private WithEvents dpi150 As System.Windows.Forms.MenuItem
      Private WithEvents dpi300 As System.Windows.Forms.MenuItem
		Private mainMenu As System.Windows.Forms.MainMenu
      Private WithEvents fileMenu As System.Windows.Forms.MenuItem
      Private WithEvents saveAsTiff As System.Windows.Forms.MenuItem
      Private WithEvents pageMenu As System.Windows.Forms.MenuItem
      Private WithEvents extractImages As System.Windows.Forms.MenuItem
      Private WithEvents saveAsBitmap As System.Windows.Forms.MenuItem
      Private WithEvents menuItem2 As System.Windows.Forms.MenuItem
      Private WithEvents settingsMenu As System.Windows.Forms.MenuItem
      Private WithEvents menuItem1 As System.Windows.Forms.MenuItem
      Private WithEvents backgroundColor As System.Windows.Forms.MenuItem
      Private WithEvents menuItem3 As System.Windows.Forms.MenuItem
      Private WithEvents fileSaveAsTiff As System.Windows.Forms.MenuItem
      Private WithEvents menuItem4 As MenuItem
      Private WithEvents highSpeed As MenuItem
      Private components As System.ComponentModel.IContainer
      Friend WithEvents saveAs4Bitmaps As System.Windows.Forms.MenuItem
      Private WithEvents loadColorProfile As MenuItem

      ''' <summary>
      ''' Summary description for DoubleBufferPanel.
      ''' </summary>
      Friend Class DoubleBufferPanel
         Inherits Panel
         Friend Sub New()
            SetStyle(ControlStyles.DoubleBuffer, True)
            SetStyle(ControlStyles.UserPaint, True)
            SetStyle(ControlStyles.AllPaintingInWmPaint, True)
            SetStyle(ControlStyles.Selectable, True)
         End Sub
      End Class
      Public Sub New()
         InitializeComponent()

         TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
         Me.Text = CAPTION
      End Sub

      Protected Overloads Overrides Sub OnLoad(ByVal e As EventArgs)
         MyBase.OnLoad(e)
      End Sub

      Protected Overloads Overrides Sub OnClosing(ByVal e As System.ComponentModel.CancelEventArgs)
         closeDoc()
         ' abort rendering.
         MyBase.OnClosing(e)
      End Sub

#Region "Windows Form Designer generated code"
      ''' <summary>
      ''' Required method for Designer support - do not modify
      ''' the contents of this method with the code editor.
      ''' </summary>
      Private Sub InitializeComponent()
         Me.viewerPanel = New DoubleBufferPanel
         Me.components = New System.ComponentModel.Container
         Me.pagesListBox = New System.Windows.Forms.ListBox
         Me.splitter1 = New System.Windows.Forms.Splitter
         Me.mainMenu = New System.Windows.Forms.MainMenu(Me.components)
         Me.fileMenu = New System.Windows.Forms.MenuItem
         Me.fileOpen = New System.Windows.Forms.MenuItem
         Me.fileClose = New System.Windows.Forms.MenuItem
         Me.menuItem3 = New System.Windows.Forms.MenuItem
         Me.fileSaveAsTiff = New System.Windows.Forms.MenuItem
         Me.pageMenu = New System.Windows.Forms.MenuItem
         Me.saveAsBitmap = New System.Windows.Forms.MenuItem
         Me.saveAsTiff = New System.Windows.Forms.MenuItem
         Me.menuItem2 = New System.Windows.Forms.MenuItem
         Me.extractImages = New System.Windows.Forms.MenuItem
         Me.settingsMenu = New System.Windows.Forms.MenuItem
         Me.dpi48 = New System.Windows.Forms.MenuItem
         Me.dpi72 = New System.Windows.Forms.MenuItem
         Me.dpi150 = New System.Windows.Forms.MenuItem
         Me.dpi300 = New System.Windows.Forms.MenuItem
         Me.menuItem1 = New System.Windows.Forms.MenuItem
         Me.backgroundColor = New System.Windows.Forms.MenuItem
         Me.menuItem4 = New System.Windows.Forms.MenuItem
         Me.highSpeed = New System.Windows.Forms.MenuItem
         Me.loadColorProfile = New System.Windows.Forms.MenuItem
         Me.saveAs4Bitmaps = New System.Windows.Forms.MenuItem
         Me.SuspendLayout()
         '
         'pagesListBox
         '
         Me.pagesListBox.Dock = System.Windows.Forms.DockStyle.Left
         Me.pagesListBox.IntegralHeight = False
         Me.pagesListBox.Location = New System.Drawing.Point(0, 0)
         Me.pagesListBox.Name = "pagesListBox"
         Me.pagesListBox.Size = New System.Drawing.Size(120, 433)
         Me.pagesListBox.TabIndex = 0
         '
         'splitter1
         '
         Me.splitter1.Location = New System.Drawing.Point(120, 0)
         Me.splitter1.Name = "splitter1"
         Me.splitter1.Size = New System.Drawing.Size(3, 433)
         Me.splitter1.TabIndex = 1
         Me.splitter1.TabStop = False
         '
         'viewerPanel
         '
         Me.viewerPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
         Me.viewerPanel.Dock = System.Windows.Forms.DockStyle.Fill
         Me.viewerPanel.Location = New System.Drawing.Point(123, 0)
         Me.viewerPanel.Name = "viewerPanel"
         Me.viewerPanel.Size = New System.Drawing.Size(373, 433)
         Me.viewerPanel.TabIndex = 2
         '
         'mainMenu
         '
         Me.mainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileMenu, Me.pageMenu, Me.settingsMenu})
         '
         'fileMenu
         '
         Me.fileMenu.Index = 0
         Me.fileMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileOpen, Me.fileClose, Me.menuItem3, Me.fileSaveAsTiff})
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
         'menuItem3
         '
         Me.menuItem3.Index = 2
         Me.menuItem3.Text = "-"
         '
         'fileSaveAsTiff
         '
         Me.fileSaveAsTiff.Index = 3
         Me.fileSaveAsTiff.Text = "Save as multi-page TIFF"
         '
         'pageMenu
         '
         Me.pageMenu.Index = 1
         Me.pageMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.saveAsBitmap, Me.saveAs4Bitmaps, Me.saveAsTiff, Me.menuItem2, Me.extractImages})
         Me.pageMenu.Text = "Page"
         '
         'saveAsBitmap
         '
         Me.saveAsBitmap.Index = 0
         Me.saveAsBitmap.Text = "Save as bitmap"
         '
         'saveAsTiff
         '
         Me.saveAsTiff.Index = 2
         Me.saveAsTiff.Text = "Save as TIFF"
         '
         'menuItem2
         '
         Me.menuItem2.Index = 3
         Me.menuItem2.Text = "-"
         '
         'extractImages
         '
         Me.extractImages.Index = 4
         Me.extractImages.Text = "Extract Images"
         '
         'settingsMenu
         '
         Me.settingsMenu.Index = 2
         Me.settingsMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.dpi48, Me.dpi72, Me.dpi150, Me.dpi300, Me.menuItem1, Me.backgroundColor, Me.menuItem4, Me.highSpeed, Me.loadColorProfile})
         Me.settingsMenu.Text = "Settings"
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
         'menuItem1
         '
         Me.menuItem1.Index = 4
         Me.menuItem1.Text = "-"
         '
         'backgroundColor
         '
         Me.backgroundColor.Index = 5
         Me.backgroundColor.Text = "Background color..."
         '
         'menuItem4
         '
         Me.menuItem4.Index = 6
         Me.menuItem4.Text = "-"
         '
         'highSpeed
         '
         Me.highSpeed.Checked = True
         Me.highSpeed.Index = 7
         Me.highSpeed.Text = "High Speed"
         '
         'loadColorProfile
         '
         Me.loadColorProfile.Index = 8
         Me.loadColorProfile.Text = "Load Color Profile..."
         '
         'saveAs4Bitmaps
         '
         Me.saveAs4Bitmaps.Index = 1
         Me.saveAs4Bitmaps.Text = "Save as 4 sub-bitmaps"
         '
         'MainForm
         '
         Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
         Me.ClientSize = New System.Drawing.Size(496, 433)
         Me.Controls.Add(Me.viewerPanel)
         Me.Controls.Add(Me.splitter1)
         Me.Controls.Add(Me.pagesListBox)
         Me.Menu = Me.mainMenu
         Me.Name = "MainForm"
         Me.Text = "PDFViewer Demo Application"
         Me.ResumeLayout(False)

      End Sub
#End Region


      <STAThread()> _
      Friend Shared Sub Main()
         Application.Run(New MainForm())
      End Sub

      Private Sub closeDoc()
         document = Nothing

         If drawJob IsNot Nothing Then
            drawJob.Dispose()
            drawJob = Nothing
         End If

         Me.pagesListBox.Items.Clear()
         Me.Text = CAPTION

         viewerPanel.Invalidate()
      End Sub

      Private Sub selectPage(ByVal index As Integer)
         If document Is Nothing Then
            Return
         End If

         Try
            Cursor = Cursors.WaitCursor

            selectedPage = index
            Dim page As Page = document.Pages(selectedPage)

            If drawJob IsNot Nothing Then
               drawJob.Dispose()
            End If
            drawJob = New DrawJob(page)
            AddHandler drawJob.Updated, New UpdatedEventHandler(AddressOf drawJob_Updated)

            ' In case you're missing some small details at the right and/or bottom edges,
            ' you might want to increase the bitmap size with on pixel in one or both directions.
            Dim mybitmap As New Bitmap(CInt(Math.Truncate(page.Width / 72 * dpi)), CInt(Math.Truncate(page.Height / 72 * dpi)))

            Dim scale As Single = CSng(dpi / 72.0F)

            Dim transformation As New Matrix()
            transformation.Scale(scale, scale)

            ' start the drawig job.
            drawJob.Start(mybitmap, renderSettings, PageBoundary.[Default], transformation)

            viewerPanel.Invalidate()

            Me.Text = CAPTION & String.Format(" (page {0} of {1})", selectedPage, path)
         Finally
            Cursor = Cursors.Arrow
         End Try
      End Sub

      Private Sub drawJob_Updated(ByVal sender As DrawJob, ByVal e As UpdatedEventArgs)
         viewerPanel.Invalidate()
      End Sub

      Private Sub fileOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fileOpen.Click
         closeDoc()

         Dim dialog As New OpenFileDialog()
         dialog.Filter = "PDF Files|*.pdf|All files|*.*"
         If Windows.Forms.DialogResult.OK = dialog.ShowDialog() Then
            path = dialog.FileName

            Using file As New FileStream(path, FileMode.Open, FileAccess.Read)
               Dim buffer As Byte() = New Byte(CInt(file.Length - 1)) {}
               file.Read(buffer, 0, buffer.Length)
               document = New Document(New BinaryReader(New MemoryStream(buffer)))

               For i As Integer = 0 To document.Pages.Count - 1
                  pagesListBox.Items.Add("Page " & (i + 1))
               Next
            End Using
            Me.pagesListBox.SelectedIndex = 0
         End If
      End Sub

      Private Sub pagesListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles pagesListBox.SelectedIndexChanged
         Dim index As Integer = pagesListBox.SelectedIndex
         If index >= 0 AndAlso document IsNot Nothing AndAlso index < document.Pages.Count Then
            selectPage(index)
         End If
      End Sub

      Private Sub viewerPanel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles viewerPanel.Paint
         e.Graphics.Clear(Color.White)

         If drawJob IsNot Nothing Then
            Using bitmap As System.Drawing.Bitmap = drawJob.GetBitmap()
               If bitmap IsNot Nothing Then
                  Dim ratio As Single = CSng(bitmap.Width) / CSng(bitmap.Height)
                  Dim displayWidth As Single, displayHeight As Single
                  If ratio * viewerPanel.Height > viewerPanel.Width Then
                     displayWidth = viewerPanel.Width - 10
                     displayHeight = displayWidth / ratio
                  Else
                     displayHeight = viewerPanel.Height - 10
                     displayWidth = displayHeight * ratio
                  End If

                  Dim x As Single = (viewerPanel.Width - displayWidth) / 2
                  Dim y As Single = (viewerPanel.Height - displayHeight) / 2

                  e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
                  e.Graphics.DrawImage(bitmap, x, y, displayWidth, displayHeight)
                  e.Graphics.DrawRectangle(New Pen(Color.Gray), x, y, displayWidth, displayHeight)
               End If
            End Using
         End If
      End Sub

      Private Sub viewerPanel_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles viewerPanel.Resize
         viewerPanel.Invalidate()
      End Sub

      Private Sub fileClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fileClose.Click
         Close()
      End Sub

      Private Sub fileSaveAsTiff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fileSaveAsTiff.Click
         If document IsNot Nothing Then
            Dim options As New ConvertToTiffOptions(dpi, TiffCompression.CcittG4)
            options.InvertImage = MessageBox.Show("Do you want to invert the image?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
            Dim fileName As String = String.Format("{0}.tif", path)
            Using stream As New FileStream(fileName, FileMode.Create)
               document.ConvertToTiff(stream, options)
            End Using
            MessageBox.Show("TIFF was created.", fileName)
         End If
      End Sub

      Private Sub saveAsBitmap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles saveAsBitmap.Click
         If drawJob IsNot Nothing Then
            Dim bitmap As System.Drawing.Bitmap = drawJob.GetBitmap()

            If bitmap IsNot Nothing Then
               Dim fileName As String = String.Format("{0}_Page_{1}.png", path, selectedPage)
               bitmap.Save(fileName, ImageFormat.Png)
               MessageBox.Show("Bitmap was created.", fileName)
            End If
         End If
      End Sub

      Private Sub saveAsTiff_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles saveAsTiff.Click
         If document IsNot Nothing Then
            Dim options As New ConvertToTiffOptions(dpi, TiffCompression.CcittG4)
            options.InvertImage = MessageBox.Show("Do you want to invert the image?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes
            Dim fileName As String = String.Format("{0}_Page_{1}.tif", path, selectedPage)
            Using stream As New FileStream(fileName, FileMode.Create)
               document.Pages(selectedPage).ConvertToTiff(stream, options)
            End Using
            MessageBox.Show("TIFF was created.", fileName)
         End If
      End Sub

      Private Sub extractImages_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles extractImages.Click
         If document IsNot Nothing Then
            Dim images As ExtractedImageInfo() = document.Pages(selectedPage).ExtractImages()
            Dim imageCnt As Integer = 0
            For Each image As ExtractedImageInfo In images
               Dim fileName As String = String.Format("{0}_Page_{1}_Image_{2}.png", path, selectedPage, System.Threading.Interlocked.Increment(imageCnt))
               image.Bitmap.Save(fileName, ImageFormat.Png)
            Next
            If 0 = imageCnt Then
               MessageBox.Show("No images found on this page.", [String].Format("{0} Page {1}", path, selectedPage))
            Else
               MessageBox.Show([String].Format("{0} images have been extracted.", imageCnt), [String].Format("{0}_Page_{1}_Image_*.png", path, selectedPage))
            End If
         End If
      End Sub

      Private Sub dpi48_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpi48.Click
         dpi48.Checked = True
         dpi72.Checked = False
         dpi150.Checked = False
         dpi300.Checked = False
         Me.dpi = 48
         selectPage(selectedPage)
      End Sub

      Private Sub dpi72_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpi72.Click
         dpi48.Checked = False
         dpi72.Checked = True
         dpi150.Checked = False
         dpi300.Checked = False
         Me.dpi = 72
         selectPage(selectedPage)
      End Sub

      Private Sub dpi150_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpi150.Click
         dpi48.Checked = False
         dpi72.Checked = False
         dpi150.Checked = True
         dpi300.Checked = False
         Me.dpi = 150
         selectPage(selectedPage)
      End Sub

      Private Sub dpi300_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dpi300.Click
         dpi48.Checked = False
         dpi72.Checked = False
         dpi150.Checked = False
         dpi300.Checked = True
         Me.dpi = 300
         selectPage(selectedPage)
      End Sub

      Private Sub backgroundColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles backgroundColor.Click
         If document IsNot Nothing Then
            Dim dialog As New ColorDialog()
            dialog.Color = renderSettings.ColorSettings.BackColor
            If dialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
               renderSettings.ColorSettings.BackColor = dialog.Color
               selectPage(selectedPage)
            End If
         End If
      End Sub

      Private Sub highSpeed_Click(ByVal sender As Object, ByVal e As EventArgs) Handles highSpeed.Click
         highSpeed.Checked = Not highSpeed.Checked
         If highSpeed.Checked Then
            renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighSpeed
         Else
            renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality
         End If
         selectPage(selectedPage)
      End Sub

      Private Sub loadColorProfile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles loadColorProfile.Click
         Dim dialog As New OpenFileDialog()
         dialog.Filter = "ICC Color Profile|*.icm|All files|*.*"
         dialog.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.System) & "\spool\drivers\color"
         If Windows.Forms.DialogResult.OK = dialog.ShowDialog() Then
            renderSettings.ColorSettings.OutputColorProfile = New ColorProfile(New FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            selectPage(selectedPage)
         End If
      End Sub

      Private Sub saveAs4Bitmaps_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveAs4Bitmaps.Click

         ' we are going to render four separate bitmaps for the current page.

         Dim page As Page = document.Pages(selectedPage)
         Dim scale As Single = CSng(dpi) / 72

         ' Generate 4 tiles for the page. Tile size is in points, not pixels.
         Dim tileSizeX As Single = CSng(page.Width) / 2
         Dim tileSizeY As Single = CSng(page.Height) / 2

         ' subtract 0.001 to deal with rounding errors.
         For xPos As Single = 0 To CSng(page.Width - 0.001) Step tileSizeX

            For yPos As Single = 0 To CSng(page.Height - 0.001) Step tileSizeY

               Dim fileName As String = String.Format("{0}_Page_{1}_{2}_{3}.png", path, selectedPage, xPos, yPos)

               Dim bitmapSizeX As Integer = CInt( (scale * (Math.Min(tileSizeX, page.Width - xPos))))
               Dim bitmapSizeY As Integer = CInt((scale * (Math.Min(tileSizeY, page.Height - yPos))))

               Using bitmap As New Bitmap(bitmapSizeX, bitmapSizeY)

                  ' Using a drawjob is much faster if many tiles are generated, because
                  ' different draw jobs share an intermediate representation for a single
                  ' page.

                  Dim drawJob As DrawJob = New DrawJob(page)

                  Dim transformation As Matrix = New Matrix()
                  transformation.Scale(scale, scale)
                  transformation.Translate(-xPos, -yPos)

                  ' start the drawig job.
                  drawJob.Start(bitmap, New RenderSettings(), PageBoundary.Default, transformation)
                  drawJob.Wait()

                  bitmap.Save(fileName, ImageFormat.Png)
               End Using
            Next
         Next
         MessageBox.Show("Bitmaps have been created.")
      End Sub

   End Class
End Namespace
