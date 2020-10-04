Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Configuration

''' <summary>
''' Summary description for MainForm.
''' </summary>
Public Class MainForm
	Inherits System.Windows.Forms.Form
	Private mainMenu1 As System.Windows.Forms.MainMenu
	Private radioButton3 As System.Windows.Forms.RadioButton
	Private selectPdfDocument As System.Windows.Forms.Button
	Private compressionGroup As System.Windows.Forms.GroupBox
	Private compressionCcittG4 As System.Windows.Forms.RadioButton
	Private compressionCcittG3 As System.Windows.Forms.RadioButton
	Private compressionNone As System.Windows.Forms.RadioButton
	Private compressionLzw As System.Windows.Forms.RadioButton
	Private dpiGroup As System.Windows.Forms.GroupBox
	Private dpi300 As System.Windows.Forms.RadioButton
	Private dpi150 As System.Windows.Forms.RadioButton
	Private dpi72 As System.Windows.Forms.RadioButton
	Private components As System.ComponentModel.IContainer
	Private compressionPackBits As RadioButton
	Private dpi600 As RadioButton
	Private dpi96 As RadioButton
	Private saveAsImage As System.Windows.Forms.Button

	Public Sub New()
		'
		' Required for Windows Form Designer support
		'
		InitializeComponent()

		TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
	End Sub

	#Region "Windows Form Designer generated code"
	''' <summary>
	''' Required method for Designer support - do not modify
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Me.mainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
		Me.selectPdfDocument = New System.Windows.Forms.Button()
		Me.compressionGroup = New System.Windows.Forms.GroupBox()
		Me.compressionCcittG4 = New System.Windows.Forms.RadioButton()
		Me.radioButton3 = New System.Windows.Forms.RadioButton()
		Me.compressionCcittG3 = New System.Windows.Forms.RadioButton()
		Me.compressionNone = New System.Windows.Forms.RadioButton()
		Me.compressionLzw = New System.Windows.Forms.RadioButton()
		Me.dpiGroup = New System.Windows.Forms.GroupBox()
		Me.dpi300 = New System.Windows.Forms.RadioButton()
		Me.dpi150 = New System.Windows.Forms.RadioButton()
		Me.dpi72 = New System.Windows.Forms.RadioButton()
		Me.saveAsImage = New System.Windows.Forms.Button()
		Me.compressionPackBits = New System.Windows.Forms.RadioButton()
		Me.dpi600 = New System.Windows.Forms.RadioButton()
		Me.dpi96 = New System.Windows.Forms.RadioButton()
		Me.compressionGroup.SuspendLayout()
		Me.dpiGroup.SuspendLayout()
		Me.SuspendLayout()
		' 
		' selectPdfDocument
		' 
		Me.selectPdfDocument.Location = New System.Drawing.Point(8, 16)
		Me.selectPdfDocument.Name = "selectPdfDocument"
		Me.selectPdfDocument.Size = New System.Drawing.Size(272, 23)
		Me.selectPdfDocument.TabIndex = 2
		Me.selectPdfDocument.Text = "Select PDF document..."
		AddHandler Me.selectPdfDocument.Click, New System.EventHandler(AddressOf Me.selectPdfDocument_Click)
		' 
		' compressionGroup
		' 
		Me.compressionGroup.Controls.Add(Me.compressionPackBits)
		Me.compressionGroup.Controls.Add(Me.compressionCcittG4)
		Me.compressionGroup.Controls.Add(Me.radioButton3)
		Me.compressionGroup.Controls.Add(Me.compressionCcittG3)
		Me.compressionGroup.Controls.Add(Me.compressionNone)
		Me.compressionGroup.Controls.Add(Me.compressionLzw)
		Me.compressionGroup.Enabled = False
		Me.compressionGroup.Location = New System.Drawing.Point(8, 52)
		Me.compressionGroup.Name = "compressionGroup"
		Me.compressionGroup.Size = New System.Drawing.Size(128, 155)
		Me.compressionGroup.TabIndex = 3
		Me.compressionGroup.TabStop = False
		Me.compressionGroup.Text = "Image Compression"
		' 
		' compressionCcittG4
		' 
		Me.compressionCcittG4.Location = New System.Drawing.Point(16, 72)
		Me.compressionCcittG4.Name = "compressionCcittG4"
		Me.compressionCcittG4.Size = New System.Drawing.Size(104, 24)
		Me.compressionCcittG4.TabIndex = 7
		Me.compressionCcittG4.Text = "CCITT G4"
		' 
		' radioButton3
		' 
		Me.radioButton3.Location = New System.Drawing.Point(16, 72)
		Me.radioButton3.Name = "radioButton3"
		Me.radioButton3.Size = New System.Drawing.Size(104, 24)
		Me.radioButton3.TabIndex = 6
		Me.radioButton3.Text = "radioButton3"
		' 
		' compressionCcittG3
		' 
		Me.compressionCcittG3.Location = New System.Drawing.Point(16, 48)
		Me.compressionCcittG3.Name = "compressionCcittG3"
		Me.compressionCcittG3.Size = New System.Drawing.Size(104, 24)
		Me.compressionCcittG3.TabIndex = 5
		Me.compressionCcittG3.Text = "CCITT G3"
		' 
		' compressionNone
		' 
		Me.compressionNone.Checked = True
		Me.compressionNone.Location = New System.Drawing.Point(16, 24)
		Me.compressionNone.Name = "compressionNone"
		Me.compressionNone.Size = New System.Drawing.Size(104, 24)
		Me.compressionNone.TabIndex = 4
		Me.compressionNone.TabStop = True
		Me.compressionNone.Text = "None"
		' 
		' compressionLzw
		' 
		Me.compressionLzw.Location = New System.Drawing.Point(16, 96)
		Me.compressionLzw.Name = "compressionLzw"
		Me.compressionLzw.Size = New System.Drawing.Size(104, 24)
		Me.compressionLzw.TabIndex = 8
		Me.compressionLzw.Text = "LZW"
		' 
		' dpiGroup
		' 
		Me.dpiGroup.Controls.Add(Me.dpi96)
		Me.dpiGroup.Controls.Add(Me.dpi600)
		Me.dpiGroup.Controls.Add(Me.dpi300)
		Me.dpiGroup.Controls.Add(Me.dpi150)
		Me.dpiGroup.Controls.Add(Me.dpi72)
		Me.dpiGroup.Enabled = False
		Me.dpiGroup.Location = New System.Drawing.Point(152, 52)
		Me.dpiGroup.Name = "dpiGroup"
		Me.dpiGroup.Size = New System.Drawing.Size(128, 155)
		Me.dpiGroup.TabIndex = 4
		Me.dpiGroup.TabStop = False
		Me.dpiGroup.Text = "Output resolution"
		' 
		' dpi300
		' 
		Me.dpi300.Location = New System.Drawing.Point(16, 93)
		Me.dpi300.Name = "dpi300"
		Me.dpi300.Size = New System.Drawing.Size(96, 24)
		Me.dpi300.TabIndex = 2
		Me.dpi300.Text = "300 DPI"
		' 
		' dpi150
		' 
		Me.dpi150.Location = New System.Drawing.Point(16, 69)
		Me.dpi150.Name = "dpi150"
		Me.dpi150.Size = New System.Drawing.Size(96, 24)
		Me.dpi150.TabIndex = 1
		Me.dpi150.Text = "150 DPI"
		' 
		' dpi72
		' 
		Me.dpi72.Checked = True
		Me.dpi72.Location = New System.Drawing.Point(16, 24)
		Me.dpi72.Name = "dpi72"
		Me.dpi72.Size = New System.Drawing.Size(96, 24)
		Me.dpi72.TabIndex = 0
		Me.dpi72.Text = "72 DPI"
		' 
		' saveAsImage
		' 
		Me.saveAsImage.Enabled = False
		Me.saveAsImage.Location = New System.Drawing.Point(8, 213)
		Me.saveAsImage.Name = "saveAsImage"
		Me.saveAsImage.Size = New System.Drawing.Size(272, 23)
		Me.saveAsImage.TabIndex = 5
		Me.saveAsImage.Text = "Save as image..."
		AddHandler Me.saveAsImage.Click, New System.EventHandler(AddressOf Me.saveAsImage_Click)
		' 
		' compressionPackBits
		' 
		Me.compressionPackBits.Location = New System.Drawing.Point(16, 120)
		Me.compressionPackBits.Name = "compressionPackBits"
		Me.compressionPackBits.Size = New System.Drawing.Size(104, 24)
		Me.compressionPackBits.TabIndex = 9
		Me.compressionPackBits.Text = "PackBits"
		' 
		' dpi600
		' 
		Me.dpi600.Location = New System.Drawing.Point(16, 117)
		Me.dpi600.Name = "dpi600"
		Me.dpi600.Size = New System.Drawing.Size(96, 24)
		Me.dpi600.TabIndex = 3
		Me.dpi600.Text = "600 DPI"
		' 
		' dpi96
		' 
		Me.dpi96.Location = New System.Drawing.Point(16, 47)
		Me.dpi96.Name = "dpi96"
		Me.dpi96.Size = New System.Drawing.Size(96, 24)
		Me.dpi96.TabIndex = 4
		Me.dpi96.Text = "96 DPI"
		' 
		' MainForm
		' 
		Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
		Me.ClientSize = New System.Drawing.Size(290, 246)
		Me.Controls.Add(Me.saveAsImage)
		Me.Controls.Add(Me.dpiGroup)
		Me.Controls.Add(Me.compressionGroup)
		Me.Controls.Add(Me.selectPdfDocument)
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.MaximizeBox = False
		Me.Menu = Me.mainMenu1
		Me.Name = "MainForm"
		Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
		Me.Text = "Save PDF as image"
		Me.compressionGroup.ResumeLayout(False)
		Me.dpiGroup.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	#End Region

	''' <summary>
	''' The main entry point for the application.
	''' </summary>
    Friend Shared Sub Main()
        Application.Run(New MainForm())
    End Sub

	Private Sub selectPdfDocument_Click(sender As Object, e As System.EventArgs)
		Dim dialog As New OpenFileDialog()
		dialog.DefaultExt = ".pdf"
        If Windows.Forms.DialogResult.OK = dialog.ShowDialog() Then
            Dim path As String = dialog.FileName
            Using file As New FileStream(path, FileMode.Open, FileAccess.Read)
                document = New Document(file)
            End Using

            compressionGroup.Enabled = True
            dpiGroup.Enabled = True
            saveAsImage.Enabled = True
        End If
	End Sub

	Private Sub saveAsImage_Click(sender As Object, e As System.EventArgs)
		Dim dialog As New SaveFileDialog()
		dialog.Filter = "TIFF files (*.tiff)|*.tiff"
        If Windows.Forms.DialogResult.OK = dialog.ShowDialog() Then
            Dim tiffOptions As New ConvertToTiffOptions(getDPI(), getCompression())

            Using file As New FileStream(dialog.FileName, FileMode.Create, FileAccess.Write)
                document.ConvertToTiff(file, tiffOptions)
            End Using

            MessageBox.Show(Me, "Done!", "TIFF Created")
        End If
	End Sub

	Private Function getDPI() As Integer
		If dpi72.Checked Then
			Return 72
		End If
		If dpi96.Checked Then
			Return 96
		End If
		If dpi150.Checked Then
			Return 150
		End If
		If dpi300.Checked Then
			Return 300
		End If
		If dpi600.Checked Then
			Return 600
		End If
        Throw New Exception("Unsupported DPI selected.")
	End Function

	Private Function getCompression() As TiffCompression
		If compressionNone.Checked Then
			Return TiffCompression.None
		End If
		If compressionCcittG3.Checked Then
			Return TiffCompression.CcittG3
		End If
		If compressionCcittG4.Checked Then
			Return TiffCompression.CcittG4
		End If
		If compressionLzw.Checked Then
			Return TiffCompression.Lzw
		End If
		If compressionPackBits.Checked Then
			Return TiffCompression.PackBits
		End If
		Throw New Exception("Unsupported Compression selected.")
	End Function

	Private document As Document = Nothing
End Class
