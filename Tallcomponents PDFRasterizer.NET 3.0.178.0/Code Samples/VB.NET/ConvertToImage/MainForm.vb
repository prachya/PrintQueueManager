Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Configuration


Public Class MainForm
   Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   Friend WithEvents selectPdfDocument As System.Windows.Forms.Button
   Friend WithEvents mainMenu1 As System.Windows.Forms.MainMenu
   Friend WithEvents label1 As System.Windows.Forms.Label
   Friend WithEvents dpiGroup As System.Windows.Forms.GroupBox
   Friend WithEvents dpi300 As System.Windows.Forms.RadioButton
   Friend WithEvents dpi150 As System.Windows.Forms.RadioButton
   Friend WithEvents dpi72 As System.Windows.Forms.RadioButton
   Friend WithEvents saveAsImage As System.Windows.Forms.Button
   Friend WithEvents formatGroup As System.Windows.Forms.GroupBox
   Friend WithEvents tiffFormat As System.Windows.Forms.RadioButton
   Friend WithEvents radioButton3 As System.Windows.Forms.RadioButton
   Friend WithEvents pngFormat As System.Windows.Forms.RadioButton
   Friend WithEvents bmpFormat As System.Windows.Forms.RadioButton
   Friend WithEvents jpegFormat As System.Windows.Forms.RadioButton
   Friend WithEvents selectedPage As System.Windows.Forms.ComboBox
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.selectPdfDocument = New System.Windows.Forms.Button
      Me.mainMenu1 = New System.Windows.Forms.MainMenu
      Me.label1 = New System.Windows.Forms.Label
      Me.dpiGroup = New System.Windows.Forms.GroupBox
      Me.dpi300 = New System.Windows.Forms.RadioButton
      Me.dpi150 = New System.Windows.Forms.RadioButton
      Me.dpi72 = New System.Windows.Forms.RadioButton
      Me.saveAsImage = New System.Windows.Forms.Button
      Me.formatGroup = New System.Windows.Forms.GroupBox
      Me.tiffFormat = New System.Windows.Forms.RadioButton
      Me.radioButton3 = New System.Windows.Forms.RadioButton
      Me.pngFormat = New System.Windows.Forms.RadioButton
      Me.bmpFormat = New System.Windows.Forms.RadioButton
      Me.jpegFormat = New System.Windows.Forms.RadioButton
      Me.selectedPage = New System.Windows.Forms.ComboBox
      Me.dpiGroup.SuspendLayout()
      Me.formatGroup.SuspendLayout()
      Me.SuspendLayout()
      '
      'selectPdfDocument
      '
      Me.selectPdfDocument.Location = New System.Drawing.Point(8, 8)
      Me.selectPdfDocument.Name = "selectPdfDocument"
      Me.selectPdfDocument.Size = New System.Drawing.Size(272, 23)
      Me.selectPdfDocument.TabIndex = 8
      Me.selectPdfDocument.Text = "Select PDF document..."
      '
      'label1
      '
      Me.label1.Location = New System.Drawing.Point(8, 48)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(264, 23)
      Me.label1.TabIndex = 6
      Me.label1.Text = "Select page"
      '
      'dpiGroup
      '
      Me.dpiGroup.Controls.AddRange(New System.Windows.Forms.Control() {Me.dpi300, Me.dpi150, Me.dpi72})
      Me.dpiGroup.Enabled = False
      Me.dpiGroup.Location = New System.Drawing.Point(152, 104)
      Me.dpiGroup.Name = "dpiGroup"
      Me.dpiGroup.Size = New System.Drawing.Size(128, 136)
      Me.dpiGroup.TabIndex = 10
      Me.dpiGroup.TabStop = False
      Me.dpiGroup.Text = "Output resolution"
      '
      'dpi300
      '
      Me.dpi300.Location = New System.Drawing.Point(16, 72)
      Me.dpi300.Name = "dpi300"
      Me.dpi300.Size = New System.Drawing.Size(96, 24)
      Me.dpi300.TabIndex = 2
      Me.dpi300.Text = "300 DPI"
      '
      'dpi150
      '
      Me.dpi150.Location = New System.Drawing.Point(16, 48)
      Me.dpi150.Name = "dpi150"
      Me.dpi150.Size = New System.Drawing.Size(96, 24)
      Me.dpi150.TabIndex = 1
      Me.dpi150.Text = "150 DPI"
      '
      'dpi72
      '
      Me.dpi72.Checked = True
      Me.dpi72.Location = New System.Drawing.Point(16, 24)
      Me.dpi72.Name = "dpi72"
      Me.dpi72.Size = New System.Drawing.Size(96, 24)
      Me.dpi72.TabIndex = 0
      Me.dpi72.TabStop = True
      Me.dpi72.Text = "72 DPI"
      '
      'saveAsImage
      '
      Me.saveAsImage.Enabled = False
      Me.saveAsImage.Location = New System.Drawing.Point(8, 256)
      Me.saveAsImage.Name = "saveAsImage"
      Me.saveAsImage.Size = New System.Drawing.Size(272, 23)
      Me.saveAsImage.TabIndex = 11
      Me.saveAsImage.Text = "Save as image..."
      '
      'formatGroup
      '
      Me.formatGroup.Controls.AddRange(New System.Windows.Forms.Control() {Me.tiffFormat, Me.radioButton3, Me.pngFormat, Me.bmpFormat, Me.jpegFormat})
      Me.formatGroup.Enabled = False
      Me.formatGroup.Location = New System.Drawing.Point(8, 104)
      Me.formatGroup.Name = "formatGroup"
      Me.formatGroup.Size = New System.Drawing.Size(128, 136)
      Me.formatGroup.TabIndex = 9
      Me.formatGroup.TabStop = False
      Me.formatGroup.Text = "Image format"
      '
      'tiffFormat
      '
      Me.tiffFormat.Location = New System.Drawing.Point(16, 72)
      Me.tiffFormat.Name = "tiffFormat"
      Me.tiffFormat.TabIndex = 7
      Me.tiffFormat.Text = "TIFF"
      '
      'radioButton3
      '
      Me.radioButton3.Location = New System.Drawing.Point(16, 72)
      Me.radioButton3.Name = "radioButton3"
      Me.radioButton3.TabIndex = 6
      Me.radioButton3.Text = "radioButton3"
      '
      'pngFormat
      '
      Me.pngFormat.Location = New System.Drawing.Point(16, 48)
      Me.pngFormat.Name = "pngFormat"
      Me.pngFormat.TabIndex = 5
      Me.pngFormat.Text = "PNG"
      '
      'bmpFormat
      '
      Me.bmpFormat.Checked = True
      Me.bmpFormat.Location = New System.Drawing.Point(16, 24)
      Me.bmpFormat.Name = "bmpFormat"
      Me.bmpFormat.TabIndex = 4
      Me.bmpFormat.TabStop = True
      Me.bmpFormat.Text = "BMP"
      '
      'jpegFormat
      '
      Me.jpegFormat.Location = New System.Drawing.Point(16, 96)
      Me.jpegFormat.Name = "jpegFormat"
      Me.jpegFormat.TabIndex = 8
      Me.jpegFormat.Text = "JPEG"
      '
      'selectedPage
      '
      Me.selectedPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
      Me.selectedPage.Enabled = False
      Me.selectedPage.Location = New System.Drawing.Point(8, 64)
      Me.selectedPage.Name = "selectedPage"
      Me.selectedPage.Size = New System.Drawing.Size(272, 21)
      Me.selectedPage.TabIndex = 7
      '
      'MainForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(288, 286)
      Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.dpiGroup, Me.saveAsImage, Me.formatGroup, Me.selectedPage, Me.selectPdfDocument, Me.label1})
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
      Me.MaximizeBox = False
      Me.Menu = Me.mainMenu1
      Me.Name = "MainForm"
      Me.Text = "Form1"
      Me.dpiGroup.ResumeLayout(False)
      Me.formatGroup.ResumeLayout(False)
      Me.ResumeLayout(False)

   End Sub

#End Region

   Private Sub selectPdfDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles selectPdfDocument.Click
      Dim dialog As New OpenFileDialog()
      dialog.DefaultExt = ".pdf"
      If (Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then

         Dim path As String = dialog.FileName
         Using pdfFile As New FileStream(path, FileMode.Open, FileAccess.Read)
            document = New Document(pdfFile)

            selectedPage.Items.Clear()
            Dim i As Integer
            For i = 1 To document.Pages.Count
               selectedPage.Items.Add(i)
            Next i
            selectedPage.SelectedIndex = 0
         End Using
      End If

      updateControls()
   End Sub

   Private Sub updateControls()
      selectedPage.Enabled = ((Not (document Is Nothing)) And document.Pages.Count > 0)
      formatGroup.Enabled = ((Not (document Is Nothing)) And document.Pages.Count > 0)
      dpiGroup.Enabled = ((Not (document Is Nothing)) And document.Pages.Count > 0)
      saveAsImage.Enabled = ((Not (document Is Nothing)) And document.Pages.Count > 0)
   End Sub

   Private Sub saveAsImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles saveAsImage.Click
      Dim dialog As New SaveFileDialog()
      dialog.Filter = getImageFilter()
      If (Windows.Forms.DialogResult.OK = dialog.ShowDialog()) Then
         Dim page As Page = document.Pages(selectedPage.SelectedIndex)
         Dim dpi As Integer = getDPI()
         Dim scale As Double = dpi / 72
        
         Using bitmap As New Bitmap(CInt(scale * page.Width), CInt(scale * page.Height))
            Using graph As Graphics = Graphics.FromImage(bitmap)
               graph.SmoothingMode = SmoothingMode.AntiAlias
               graph.ScaleTransform(CSng(scale), CSng(scale))
               graph.Clear(Color.White)
               page.Draw(graph)
            End Using
            bitmap.Save(dialog.FileName, getImageFormat())
         End Using

      End If
   End Sub

   Private Function getImageFilter() As String
      If (bmpFormat.Checked) Then
         getImageFilter = "BMP files (*.bmp)|*.bmp"
      ElseIf (pngFormat.Checked) Then
         getImageFilter = "PNG files (*.png)|*.png"
      ElseIf (tiffFormat.Checked) Then
         getImageFilter = "TIFF files (*.tiff)|*.tiff"
      Else
         getImageFilter = "JPEG files (*.jpg)|*.jpg"
      End If
   End Function

   Private Function getDPI() As Integer
      If (dpi72.Checked) Then
         getDPI = 72
      ElseIf (dpi150.Checked) Then
         getDPI = 150
      Else
         getDPI = 300
      End If
   End Function

   Private Function getImageFormat() As ImageFormat
      If (bmpFormat.Checked) Then
         getImageFormat = ImageFormat.Bmp
      ElseIf (pngFormat.Checked) Then
         getImageFormat = ImageFormat.Png
      ElseIf (tiffFormat.Checked) Then
         getImageFormat = ImageFormat.Tiff
      Else
         getImageFormat = ImageFormat.Jpeg
      End If
   End Function

   Dim document As Document
End Class
