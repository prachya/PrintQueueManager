Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Printing
Imports System.IO
Imports System.Windows.Forms

Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Configuration

Namespace PrintPDF

   Public Class MainForm
      Inherits System.Windows.Forms.Form

      Enum HorizontalAlignmentEnum
         Left = 1
         Center = 2
         Right = 3
      End Enum

      Enum VerticalAlignmentEnum
         Top = 1
         Center = 2
         Bottom = 3
      End Enum

      <STAThread()> _
      Shared Sub Main()
         Application.Run(New MainForm)
      End Sub

#Region " Variables"

      Dim document As Document
      Dim pageBitmap As Bitmap
      Dim printerSetting As PrinterSettings

      Dim paperSizes As ArrayList
      Dim paperSize As PaperSize
      Dim printableArea As RectangleF

      Dim paperSources As ArrayList
        Dim paperSource As PaperSource
        Private components As System.ComponentModel.IContainer
        Friend WithEvents landscape As System.Windows.Forms.CheckBox

        Dim lastPageToPrint As Integer

#End Region

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
        End Sub

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents usePrintableArea As System.Windows.Forms.CheckBox
        Friend WithEvents autoRotate As System.Windows.Forms.CheckBox
        Friend WithEvents scaleToFit As System.Windows.Forms.CheckBox
        Friend WithEvents groupBox3 As System.Windows.Forms.GroupBox
        Friend WithEvents rangeTo As System.Windows.Forms.TextBox
        Friend WithEvents label5 As System.Windows.Forms.Label
        Friend WithEvents label4 As System.Windows.Forms.Label
        Friend WithEvents rangeFrom As System.Windows.Forms.TextBox
        Friend WithEvents printRange As System.Windows.Forms.RadioButton
        Friend WithEvents printAll As System.Windows.Forms.RadioButton
        Friend WithEvents paperSourceList As System.Windows.Forms.ComboBox
        Friend WithEvents groupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents verAlignBottom As System.Windows.Forms.RadioButton
        Friend WithEvents verAlignCenter As System.Windows.Forms.RadioButton
        Friend WithEvents verAlignTop As System.Windows.Forms.RadioButton
        Friend WithEvents groupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents horAlignRight As System.Windows.Forms.RadioButton
        Friend WithEvents horAlignCenter As System.Windows.Forms.RadioButton
        Friend WithEvents horAlignLeft As System.Windows.Forms.RadioButton
        Friend Shadows WithEvents cancelButton As System.Windows.Forms.Button
        Friend WithEvents scaleLabel As System.Windows.Forms.Label
        Friend WithEvents paperSizeLabel As System.Windows.Forms.Label
        Friend WithEvents documentSizeLabel As System.Windows.Forms.Label
        Friend WithEvents previewBox As System.Windows.Forms.PictureBox
        Friend WithEvents printerList As System.Windows.Forms.ComboBox
        Friend WithEvents mainMenu1 As System.Windows.Forms.MainMenu
        Friend WithEvents menuItem1 As System.Windows.Forms.MenuItem
        Friend WithEvents openFileMenu As System.Windows.Forms.MenuItem
        Friend WithEvents label1 As System.Windows.Forms.Label
        Friend WithEvents printButton As System.Windows.Forms.Button
        Friend WithEvents titleLabel As System.Windows.Forms.Label
        Friend WithEvents label3 As System.Windows.Forms.Label
        Friend WithEvents paperSizeList As System.Windows.Forms.ComboBox
        Friend WithEvents label2 As System.Windows.Forms.Label
        Friend WithEvents previewPageIndex As System.Windows.Forms.NumericUpDown
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me.usePrintableArea = New System.Windows.Forms.CheckBox
            Me.autoRotate = New System.Windows.Forms.CheckBox
            Me.scaleToFit = New System.Windows.Forms.CheckBox
            Me.groupBox3 = New System.Windows.Forms.GroupBox
            Me.rangeTo = New System.Windows.Forms.TextBox
            Me.label5 = New System.Windows.Forms.Label
            Me.label4 = New System.Windows.Forms.Label
            Me.rangeFrom = New System.Windows.Forms.TextBox
            Me.printRange = New System.Windows.Forms.RadioButton
            Me.printAll = New System.Windows.Forms.RadioButton
            Me.paperSourceList = New System.Windows.Forms.ComboBox
            Me.groupBox2 = New System.Windows.Forms.GroupBox
            Me.verAlignBottom = New System.Windows.Forms.RadioButton
            Me.verAlignCenter = New System.Windows.Forms.RadioButton
            Me.verAlignTop = New System.Windows.Forms.RadioButton
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.horAlignRight = New System.Windows.Forms.RadioButton
            Me.horAlignCenter = New System.Windows.Forms.RadioButton
            Me.horAlignLeft = New System.Windows.Forms.RadioButton
            Me.cancelButton = New System.Windows.Forms.Button
            Me.scaleLabel = New System.Windows.Forms.Label
            Me.paperSizeLabel = New System.Windows.Forms.Label
            Me.documentSizeLabel = New System.Windows.Forms.Label
            Me.previewBox = New System.Windows.Forms.PictureBox
            Me.printerList = New System.Windows.Forms.ComboBox
            Me.mainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
            Me.menuItem1 = New System.Windows.Forms.MenuItem
            Me.openFileMenu = New System.Windows.Forms.MenuItem
            Me.label1 = New System.Windows.Forms.Label
            Me.printButton = New System.Windows.Forms.Button
            Me.titleLabel = New System.Windows.Forms.Label
            Me.label3 = New System.Windows.Forms.Label
            Me.paperSizeList = New System.Windows.Forms.ComboBox
            Me.label2 = New System.Windows.Forms.Label
            Me.previewPageIndex = New System.Windows.Forms.NumericUpDown
            Me.landscape = New System.Windows.Forms.CheckBox
            Me.groupBox3.SuspendLayout()
            Me.groupBox2.SuspendLayout()
            Me.groupBox1.SuspendLayout()
            CType(Me.previewBox, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.previewPageIndex, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'usePrintableArea
            '
            Me.usePrintableArea.Checked = True
            Me.usePrintableArea.CheckState = System.Windows.Forms.CheckState.Checked
            Me.usePrintableArea.Location = New System.Drawing.Point(14, 184)
            Me.usePrintableArea.Name = "usePrintableArea"
            Me.usePrintableArea.Size = New System.Drawing.Size(128, 16)
            Me.usePrintableArea.TabIndex = 38
            Me.usePrintableArea.Text = "Use printable area"
            '
            'autoRotate
            '
            Me.autoRotate.Checked = True
            Me.autoRotate.CheckState = System.Windows.Forms.CheckState.Checked
            Me.autoRotate.Location = New System.Drawing.Point(142, 160)
            Me.autoRotate.Name = "autoRotate"
            Me.autoRotate.Size = New System.Drawing.Size(128, 16)
            Me.autoRotate.TabIndex = 37
            Me.autoRotate.Text = "Auto-rotate"
            '
            'scaleToFit
            '
            Me.scaleToFit.Checked = True
            Me.scaleToFit.CheckState = System.Windows.Forms.CheckState.Checked
            Me.scaleToFit.Location = New System.Drawing.Point(14, 160)
            Me.scaleToFit.Name = "scaleToFit"
            Me.scaleToFit.Size = New System.Drawing.Size(128, 16)
            Me.scaleToFit.TabIndex = 31
            Me.scaleToFit.Text = "Scale to fit"
            '
            'groupBox3
            '
            Me.groupBox3.Controls.Add(Me.rangeTo)
            Me.groupBox3.Controls.Add(Me.label5)
            Me.groupBox3.Controls.Add(Me.label4)
            Me.groupBox3.Controls.Add(Me.rangeFrom)
            Me.groupBox3.Controls.Add(Me.printRange)
            Me.groupBox3.Controls.Add(Me.printAll)
            Me.groupBox3.Location = New System.Drawing.Point(142, 272)
            Me.groupBox3.Name = "groupBox3"
            Me.groupBox3.Size = New System.Drawing.Size(136, 136)
            Me.groupBox3.TabIndex = 35
            Me.groupBox3.TabStop = False
            Me.groupBox3.Text = "Print selection"
            '
            'rangeTo
            '
            Me.rangeTo.Location = New System.Drawing.Point(64, 96)
            Me.rangeTo.Name = "rangeTo"
            Me.rangeTo.Size = New System.Drawing.Size(48, 20)
            Me.rangeTo.TabIndex = 5
            '
            'label5
            '
            Me.label5.Location = New System.Drawing.Point(8, 96)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(48, 23)
            Me.label5.TabIndex = 4
            Me.label5.Text = "To:"
            Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label4
            '
            Me.label4.Location = New System.Drawing.Point(8, 72)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(48, 23)
            Me.label4.TabIndex = 3
            Me.label4.Text = "From:"
            Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'rangeFrom
            '
            Me.rangeFrom.Location = New System.Drawing.Point(64, 72)
            Me.rangeFrom.Name = "rangeFrom"
            Me.rangeFrom.Size = New System.Drawing.Size(48, 20)
            Me.rangeFrom.TabIndex = 2
            '
            'printRange
            '
            Me.printRange.Location = New System.Drawing.Point(16, 48)
            Me.printRange.Name = "printRange"
            Me.printRange.Size = New System.Drawing.Size(104, 24)
            Me.printRange.TabIndex = 1
            Me.printRange.Text = "Range"
            '
            'printAll
            '
            Me.printAll.Checked = True
            Me.printAll.Location = New System.Drawing.Point(16, 24)
            Me.printAll.Name = "printAll"
            Me.printAll.Size = New System.Drawing.Size(104, 16)
            Me.printAll.TabIndex = 0
            Me.printAll.TabStop = True
            Me.printAll.Text = "All"
            '
            'paperSourceList
            '
            Me.paperSourceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.paperSourceList.Location = New System.Drawing.Point(14, 128)
            Me.paperSourceList.Name = "paperSourceList"
            Me.paperSourceList.Size = New System.Drawing.Size(264, 21)
            Me.paperSourceList.TabIndex = 34
            '
            'groupBox2
            '
            Me.groupBox2.Controls.Add(Me.verAlignBottom)
            Me.groupBox2.Controls.Add(Me.verAlignCenter)
            Me.groupBox2.Controls.Add(Me.verAlignTop)
            Me.groupBox2.Location = New System.Drawing.Point(14, 272)
            Me.groupBox2.Name = "groupBox2"
            Me.groupBox2.Size = New System.Drawing.Size(112, 136)
            Me.groupBox2.TabIndex = 33
            Me.groupBox2.TabStop = False
            Me.groupBox2.Text = "Vertical alignment"
            '
            'verAlignBottom
            '
            Me.verAlignBottom.Location = New System.Drawing.Point(16, 72)
            Me.verAlignBottom.Name = "verAlignBottom"
            Me.verAlignBottom.Size = New System.Drawing.Size(72, 24)
            Me.verAlignBottom.TabIndex = 2
            Me.verAlignBottom.Text = "Bottom"
            '
            'verAlignCenter
            '
            Me.verAlignCenter.Checked = True
            Me.verAlignCenter.Location = New System.Drawing.Point(16, 48)
            Me.verAlignCenter.Name = "verAlignCenter"
            Me.verAlignCenter.Size = New System.Drawing.Size(64, 24)
            Me.verAlignCenter.TabIndex = 1
            Me.verAlignCenter.TabStop = True
            Me.verAlignCenter.Text = "Center"
            '
            'verAlignTop
            '
            Me.verAlignTop.Location = New System.Drawing.Point(16, 24)
            Me.verAlignTop.Name = "verAlignTop"
            Me.verAlignTop.Size = New System.Drawing.Size(64, 24)
            Me.verAlignTop.TabIndex = 0
            Me.verAlignTop.Text = "Top"
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.horAlignRight)
            Me.groupBox1.Controls.Add(Me.horAlignCenter)
            Me.groupBox1.Controls.Add(Me.horAlignLeft)
            Me.groupBox1.Location = New System.Drawing.Point(14, 208)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(264, 56)
            Me.groupBox1.TabIndex = 32
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "Horizontal alignment"
            '
            'horAlignRight
            '
            Me.horAlignRight.Location = New System.Drawing.Point(184, 24)
            Me.horAlignRight.Name = "horAlignRight"
            Me.horAlignRight.Size = New System.Drawing.Size(56, 24)
            Me.horAlignRight.TabIndex = 2
            Me.horAlignRight.Text = "Right"
            '
            'horAlignCenter
            '
            Me.horAlignCenter.Checked = True
            Me.horAlignCenter.Location = New System.Drawing.Point(104, 24)
            Me.horAlignCenter.Name = "horAlignCenter"
            Me.horAlignCenter.Size = New System.Drawing.Size(56, 24)
            Me.horAlignCenter.TabIndex = 1
            Me.horAlignCenter.TabStop = True
            Me.horAlignCenter.Text = "Center"
            '
            'horAlignLeft
            '
            Me.horAlignLeft.Location = New System.Drawing.Point(16, 24)
            Me.horAlignLeft.Name = "horAlignLeft"
            Me.horAlignLeft.Size = New System.Drawing.Size(56, 24)
            Me.horAlignLeft.TabIndex = 0
            Me.horAlignLeft.Text = "Left"
            '
            'cancelButton
            '
            Me.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.cancelButton.Location = New System.Drawing.Point(486, 384)
            Me.cancelButton.Name = "cancelButton"
            Me.cancelButton.Size = New System.Drawing.Size(75, 23)
            Me.cancelButton.TabIndex = 30
            Me.cancelButton.Text = "Cancel"
            '
            'scaleLabel
            '
            Me.scaleLabel.Location = New System.Drawing.Point(334, 344)
            Me.scaleLabel.Name = "scaleLabel"
            Me.scaleLabel.Size = New System.Drawing.Size(224, 23)
            Me.scaleLabel.TabIndex = 29
            Me.scaleLabel.Text = "Scale: ... %"
            '
            'paperSizeLabel
            '
            Me.paperSizeLabel.Location = New System.Drawing.Point(334, 320)
            Me.paperSizeLabel.Name = "paperSizeLabel"
            Me.paperSizeLabel.Size = New System.Drawing.Size(224, 23)
            Me.paperSizeLabel.TabIndex = 28
            Me.paperSizeLabel.Text = "<page w x h in>"
            '
            'documentSizeLabel
            '
            Me.documentSizeLabel.Location = New System.Drawing.Point(334, 296)
            Me.documentSizeLabel.Name = "documentSizeLabel"
            Me.documentSizeLabel.Size = New System.Drawing.Size(224, 23)
            Me.documentSizeLabel.TabIndex = 27
            Me.documentSizeLabel.Text = "Document:"
            '
            'previewBox
            '
            Me.previewBox.BackColor = System.Drawing.SystemColors.Control
            Me.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.previewBox.Location = New System.Drawing.Point(334, 32)
            Me.previewBox.Name = "previewBox"
            Me.previewBox.Size = New System.Drawing.Size(224, 248)
            Me.previewBox.TabIndex = 26
            Me.previewBox.TabStop = False
            '
            'printerList
            '
            Me.printerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.printerList.Location = New System.Drawing.Point(14, 32)
            Me.printerList.Name = "printerList"
            Me.printerList.Size = New System.Drawing.Size(264, 21)
            Me.printerList.TabIndex = 22
            '
            'mainMenu1
            '
            Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem1})
            '
            'menuItem1
            '
            Me.menuItem1.Index = 0
            Me.menuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.openFileMenu})
            Me.menuItem1.Text = "File"
            '
            'openFileMenu
            '
            Me.openFileMenu.Index = 0
            Me.openFileMenu.Text = "Open..."
            '
            'label1
            '
            Me.label1.Location = New System.Drawing.Point(14, 16)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(224, 23)
            Me.label1.TabIndex = 21
            Me.label1.Text = "Select printer"
            '
            'printButton
            '
            Me.printButton.Location = New System.Drawing.Point(398, 384)
            Me.printButton.Name = "printButton"
            Me.printButton.Size = New System.Drawing.Size(75, 23)
            Me.printButton.TabIndex = 20
            Me.printButton.Text = "Print"
            '
            'titleLabel
            '
            Me.titleLabel.Location = New System.Drawing.Point(334, 16)
            Me.titleLabel.Name = "titleLabel"
            Me.titleLabel.Size = New System.Drawing.Size(224, 23)
            Me.titleLabel.TabIndex = 36
            Me.titleLabel.Text = "No document"
            '
            'label3
            '
            Me.label3.Location = New System.Drawing.Point(14, 112)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(200, 23)
            Me.label3.TabIndex = 25
            Me.label3.Text = "Select paper source"
            '
            'paperSizeList
            '
            Me.paperSizeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.paperSizeList.Location = New System.Drawing.Point(14, 80)
            Me.paperSizeList.Name = "paperSizeList"
            Me.paperSizeList.Size = New System.Drawing.Size(264, 21)
            Me.paperSizeList.TabIndex = 24
            '
            'label2
            '
            Me.label2.Location = New System.Drawing.Point(14, 64)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(224, 23)
            Me.label2.TabIndex = 23
            Me.label2.Text = "Select paper size"
            '
            'previewPageIndex
            '
            Me.previewPageIndex.Location = New System.Drawing.Point(336, 384)
            Me.previewPageIndex.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.previewPageIndex.Name = "previewPageIndex"
            Me.previewPageIndex.Size = New System.Drawing.Size(48, 20)
            Me.previewPageIndex.TabIndex = 39
            Me.previewPageIndex.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'landscape
            '
            Me.landscape.AutoSize = True
            Me.landscape.Location = New System.Drawing.Point(142, 182)
            Me.landscape.Name = "landscape"
            Me.landscape.Size = New System.Drawing.Size(79, 17)
            Me.landscape.TabIndex = 40
            Me.landscape.Text = "Landscape"
            Me.landscape.UseVisualStyleBackColor = True
            '
            'MainForm
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(574, 441)
            Me.Controls.Add(Me.landscape)
            Me.Controls.Add(Me.previewPageIndex)
            Me.Controls.Add(Me.autoRotate)
            Me.Controls.Add(Me.scaleToFit)
            Me.Controls.Add(Me.usePrintableArea)
            Me.Controls.Add(Me.groupBox3)
            Me.Controls.Add(Me.paperSourceList)
            Me.Controls.Add(Me.groupBox2)
            Me.Controls.Add(Me.groupBox1)
            Me.Controls.Add(Me.cancelButton)
            Me.Controls.Add(Me.scaleLabel)
            Me.Controls.Add(Me.paperSizeLabel)
            Me.Controls.Add(Me.documentSizeLabel)
            Me.Controls.Add(Me.previewBox)
            Me.Controls.Add(Me.printerList)
            Me.Controls.Add(Me.label1)
            Me.Controls.Add(Me.printButton)
            Me.Controls.Add(Me.titleLabel)
            Me.Controls.Add(Me.label3)
            Me.Controls.Add(Me.paperSizeList)
            Me.Controls.Add(Me.label2)
            Me.Menu = Me.mainMenu1
            Me.Name = "MainForm"
            Me.Text = "Print PDF"
            Me.groupBox3.ResumeLayout(False)
            Me.groupBox3.PerformLayout()
            Me.groupBox2.ResumeLayout(False)
            Me.groupBox1.ResumeLayout(False)
            CType(Me.previewBox, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.previewPageIndex, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Properties"

        ReadOnly Property horizontalAlignment() As HorizontalAlignmentEnum
            Get
                If (horAlignLeft.Checked) Then
                    Return HorizontalAlignmentEnum.Left
                End If
                If (horAlignCenter.Checked) Then
                    Return HorizontalAlignmentEnum.Center
                End If
                Return HorizontalAlignmentEnum.Right
            End Get
        End Property

        ReadOnly Property verticalAlignment() As VerticalAlignmentEnum
            Get
                If (verAlignTop.Checked) Then
                    Return VerticalAlignmentEnum.Top
                End If
                If (verAlignCenter.Checked) Then
                    Return VerticalAlignmentEnum.Center
                End If
                Return VerticalAlignmentEnum.Bottom
            End Get
        End Property

        Shadows ReadOnly Property Scale() As Single
            Get
                If Nothing Is document Then Return 1

                Dim page As Page = document.Pages(PageIndex)

                Dim pdfWidth As Single = CSng(page.Width)
                Dim pdfHeight As Single = CSng(page.Height)
                Dim pdfRatio As Single = pdfWidth / pdfHeight

                Dim paperWidth As Single = (paperSize.Width * 72.0F) / 100.0F
                Dim paperHeight As Single = (paperSize.Height * 72.0F) / 100.0F
                If (landscape.Checked) Then
                    Dim temp As Single = paperHeight
                    paperHeight = paperWidth
                    paperWidth = temp
                End If

                Dim paperRatio As Single = paperWidth / paperHeight

                Dim scaledPdfWidth As Single = pdfWidth
                Dim scaledPdfHeight As Single = pdfHeight

                If autoRotate.Checked Then
                    If ((pdfRatio > 1) And (paperRatio < 1)) Or ((pdfRatio < 1) And (paperRatio > 1)) Then
                        '// need to rotate
                        Dim temp As Single = scaledPdfWidth
                        scaledPdfWidth = scaledPdfHeight
                        scaledPdfHeight = temp
                        pdfRatio = 1 / pdfRatio
                    End If
                End If

                Dim result As Single = 1
                If scaleToFit.Checked Then
                    If pdfRatio > paperRatio Then
                        result = paperWidth / scaledPdfWidth
                    Else
                        result = paperHeight / scaledPdfHeight
                    End If
                End If

                Return result
            End Get
        End Property

        ReadOnly Property PreviewTransformation() As Matrix
            Get
                Dim page As Page = document.Pages(PageIndex)

                Dim pdfWidth As Single = CSng(page.Width)
                Dim pdfHeight As Single = CSng(page.Height)
                Dim pdfRatio As Single = pdfWidth / pdfHeight

                Dim paperWidth As Single = (paperSize.Width * 72.0F) / 100.0F
                Dim paperHeight As Single = (paperSize.Height * 72.0F) / 100.0F
                If (landscape.Checked) Then
                    Dim temp As Single = paperHeight
                    paperHeight = paperWidth
                    paperWidth = temp
                End If

                Dim paperRatio As Single = paperWidth / paperHeight

                Dim scaledPdfWidth As Single = pdfWidth
                Dim scaledPdfHeight As Single = pdfHeight

                If autoRotate.Checked Then
                    If ((pdfRatio > 1) And (paperRatio < 1)) Or ((pdfRatio < 1) And (paperRatio > 1)) Then
                        '// need to rotate
                        Dim temp As Single = scaledPdfWidth
                        scaledPdfWidth = scaledPdfHeight
                        scaledPdfHeight = temp
                        pdfRatio = 1 / pdfRatio
                    End If
                End If

                Dim scale As Single = 1
                If scaleToFit.Checked Then
                    If pdfRatio > paperRatio Then
                        scale = paperWidth / scaledPdfWidth
                    Else
                        scale = paperHeight / scaledPdfHeight
                    End If
                End If

                scaledPdfWidth *= scale
                scaledPdfHeight *= scale

                Dim combinedWidth As Single = Math.Max(scaledPdfWidth, paperWidth)
                Dim combinedHeight As Single = Math.Max(scaledPdfHeight, paperHeight)
                Dim combinedRatio As Single = combinedWidth / combinedHeight

                Dim previewBoxRation As Single = CSng(previewBox.Width / previewBox.Height)
                Dim previewScale As Single

                If combinedRatio > previewBoxRation Then
                    previewScale = (previewBox.Width - 15) / combinedWidth
                Else
                    previewScale = (previewBox.Height - 15) / combinedHeight
                End If

                Dim result As Matrix = New Matrix

                result.Scale(previewScale, previewScale)
                result.Translate((previewBox.Width / previewScale - paperWidth) / 2, _
                                 (previewBox.Height / previewScale - paperHeight) / 2)

                Return result
            End Get
        End Property

        Function GetPrintTransformation(ByVal page As Page, ByVal printableArea As RectangleF) As Matrix

            Dim transformation As Matrix = New Matrix

            Dim pdfWidth As Single = CSng(page.Width)
            Dim pdfHeight As Single = CSng(page.Height)
            Dim pdfRatio As Single = pdfWidth / pdfHeight

            Dim paperWidth As Single = (paperSize.Width * 72.0F) / 100.0F
            Dim paperHeight As Single = (paperSize.Height * 72.0F) / 100.0F
            If (landscape.Checked) Then
                Dim temp As Single = paperHeight
                paperHeight = paperWidth
                paperWidth = temp
            End If

            If usePrintableArea.Checked And printableArea.Width > 0 And printableArea.Height > 0 Then
                paperWidth = printableArea.Width
                paperHeight = printableArea.Height
                transformation.Translate(printableArea.Left, printableArea.Top)
            End If

            Dim paperRatio As Single = paperWidth / paperHeight

            Dim scaledPdfWidth As Single = pdfWidth
            Dim scaledPdfHeight As Single = pdfHeight

            Dim rotate As Boolean = False
            If autoRotate.Checked Then
                If ((pdfRatio > 1) And (paperRatio < 1)) Or ((pdfRatio < 1) And (paperRatio > 1)) Then
                    '// need to rotate
                    rotate = True
                    Dim temp As Single = scaledPdfWidth
                    scaledPdfWidth = scaledPdfHeight
                    scaledPdfHeight = temp
                    pdfRatio = 1 / pdfRatio
                End If
            End If

            Dim scale As Single = 1
            If scaleToFit.Checked Then
                If pdfRatio > paperRatio Then
                    scale = paperWidth / scaledPdfWidth
                Else
                    scale = paperHeight / scaledPdfHeight
                End If
            End If

            scaledPdfWidth *= scale
            scaledPdfHeight *= scale

            Dim dx As Single
            Select Case (horizontalAlignment)
                Case HorizontalAlignmentEnum.Left
                    dx = 0
                Case HorizontalAlignmentEnum.Center
                    dx = (paperWidth - scaledPdfWidth) / 2
                Case HorizontalAlignmentEnum.Right
                    dx = paperWidth - scaledPdfWidth
                Case Else
                    Throw New ArgumentException("Horizontal alignment not set.")
            End Select

            Dim dy As Single
            Select Case (verticalAlignment)
                Case VerticalAlignmentEnum.Top
                    dy = 0
                Case VerticalAlignmentEnum.Center
                    dy = (paperHeight - scaledPdfHeight) / 2
                Case VerticalAlignmentEnum.Bottom
                    dy = paperHeight - scaledPdfHeight
                Case Else
                    Throw New ArgumentException("Vertical alignment not set.")
            End Select

            ' align
            transformation.Translate(dx, dy)

            ' auto-rotate
            If (rotate) Then
                transformation.Rotate(-90)
                transformation.Translate(-scaledPdfHeight, 0)
            End If

            ' scale
            transformation.Scale(scale, scale)

            Return transformation
        End Function

        Property PageIndex() As Integer
            Get
                Return CInt(previewPageIndex.Value - 1)
            End Get
            Set(ByVal Value As Integer)
                previewPageIndex.Value = Value + 1
            End Set
        End Property


#End Region

#Region " Print methods"

        Sub print()
            If (printRange.Checked) Then
                PageIndex = Integer.Parse(rangeFrom.Text) - 1
                lastPageToPrint = Integer.Parse(rangeTo.Text) - 1
            Else
                PageIndex = 0
                lastPageToPrint = document.Pages.Count - 1
            End If

            If PageIndex < document.Pages.Count Then
                Dim printDocument As PrintDocument = New PrintDocument
                printDocument.PrinterSettings = printerSetting
                printDocument.DocumentName = document.DocumentInfo.Title
                printDocument.DefaultPageSettings.PaperSize = paperSize
                printDocument.DefaultPageSettings.PaperSource = paperSource
                printDocument.DefaultPageSettings.Margins.Left = 0
                printDocument.DefaultPageSettings.Margins.Right = 0
                printDocument.DefaultPageSettings.Margins.Bottom = 0
                printDocument.DefaultPageSettings.Margins.Top = 0
                printDocument.OriginAtMargins = True

                ' uncomment to avoid status dialog
                ' printDocument.PrintController = New StandardPrintController

                AddHandler printDocument.PrintPage, AddressOf printPage
                printDocument.Print()
            End If
        End Sub

        Sub printPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
            e.HasMorePages = False

            If PageIndex <= lastPageToPrint Then
                e.Graphics.PageUnit = GraphicsUnit.Point
                Dim page As Page = document.Pages(PageIndex)
                e.Graphics.MultiplyTransform(GetPrintTransformation(page, e.Graphics.VisibleClipBounds))
                page.Draw(e.Graphics)
                If PageIndex < lastPageToPrint Then
                    PageIndex += 1
                    e.HasMorePages = True
                End If
            End If

        End Sub

#End Region

#Region " Fill String Lists"

        Sub populatePrinterList()
            printerList.Items.Clear()
            Dim installedPrinter As String
            For Each installedPrinter In PrinterSettings.InstalledPrinters
                printerList.Items.Add(installedPrinter)
            Next
            printerList.SelectedIndex = printerList.Items.IndexOf((New PrinterSettings).PrinterName)
        End Sub

        Sub populatePaperSizeList()
            Dim format As String = "{0} ({1:f2} x {2:f2} in)", s As String
            Dim paperSize As PaperSize, ps As PaperSize

            paperSizes.Clear()
            paperSizeList.Items.Clear()

            For Each paperSize In printerSetting.PaperSizes
                paperSizes.Add(paperSize)
                s = String.Format(format, paperSize.PaperName, paperSize.Width / 100.0F, paperSize.Height / 100.0F)
                paperSizeList.Items.Add(s)
            Next
            ps = printerSetting.DefaultPageSettings.PaperSize
            s = String.Format(format, ps.PaperName, ps.Width / 100.0F, ps.Height / 100.0F)
            paperSizeList.SelectedIndex = paperSizeList.Items.IndexOf(s)
        End Sub

        Sub populatePaperSourceList()
            Dim format As String = "{0} ({1})", s As String
            Dim paperSource, ps As PaperSource

            paperSources.Clear()
            paperSourceList.Items.Clear()

            For Each paperSource In printerSetting.PaperSources
                paperSources.Add(paperSource)
                s = String.Format(format, paperSource.SourceName, paperSource.Kind)
                paperSourceList.Items.Add(s)
            Next
            ps = printerSetting.DefaultPageSettings.PaperSource
            s = String.Format(format, ps.SourceName, ps.Kind)
            paperSourceList.SelectedIndex = paperSourceList.Items.IndexOf(s)
        End Sub

#End Region

#Region " Update dialog items"

        Sub updateTitleLabel(ByVal fileName As String)
            Dim file As FileInfo = New FileInfo(fileName)
            titleLabel.Text = String.Format("{0}, {1} pages", file.Name, document.Pages.Count)
        End Sub

        Sub updateDocumentSizeLabel()
            If Not (Nothing Is document) Then
                Dim page As Page = document.Pages(PageIndex)
                documentSizeLabel.Text = String.Format("Document: {0:f2} x {1:f2} in", _
                                                        page.Width / 72.0F, page.Height / 72.0F)
            Else
                documentSizeLabel.Text = "Document: "
            End If
        End Sub

        Sub updatePaperSizeLabel()
            paperSizeLabel.Text = String.Format("Paper: {0:f2} x {1:f2} in", _
                                                paperSize.Width / 100.0F, paperSize.Height / 100.0F)
        End Sub

        Sub updateScaleLabel()
            scaleLabel.Text = String.Format("Scale: {0}%", Scale * 100)
        End Sub

        Sub updatePreviewBox()
            If Not (Nothing Is document) Then
                Dim previewBitmap As Bitmap
                previewBitmap = New Bitmap(previewBox.Width, previewBox.Height)
                Dim previewGraphics As Graphics
                previewGraphics = Graphics.FromImage(previewBitmap)
                previewGraphics.Clear(Color.LightGray)

                '// ---------------------
                '// draw paper boundaries
                '// ---------------------
                previewGraphics.MultiplyTransform(PreviewTransformation)

                Dim paperWidth As Single = (paperSize.Width * 72.0F) / 100.0F
                Dim paperHeight As Single = (paperSize.Height * 72.0F) / 100.0F
                If (landscape.Checked) Then
                    Dim temp As Single = paperHeight
                    paperHeight = paperWidth
                    paperWidth = temp
                End If

                Dim darkGrayPen As Pen = New Pen(Color.DarkGray)
                Dim whiteBrush As Brush = New SolidBrush(Color.White)
                previewGraphics.FillRectangle(whiteBrush, 0, 0, paperWidth, paperHeight)
                previewGraphics.DrawRectangle(darkGrayPen, 0, 0, paperWidth, paperHeight)

                '// -------------
                '// draw PDF page
                '// -------------                  
                Dim page As Page = document.Pages(PageIndex)
                previewGraphics.MultiplyTransform(GetPrintTransformation(page, printableArea))

                Dim pdfWidth As Single = CSng(Page.Width)
                Dim pdfHeight As Single = CSng(page.Height)

                previewGraphics.InterpolationMode = InterpolationMode.High
                previewGraphics.DrawImage(pageBitmap, 0, 0, pdfWidth, pdfHeight)

                '// draw PDF page boundaries
                Dim redPen As Pen = New Pen(Color.Red)
                previewGraphics.DrawRectangle(redPen, 0, 0, pdfWidth, pdfHeight)

                previewBox.Image = previewBitmap
            Else
                previewBox.Image = Nothing
            End If
        End Sub

        Sub updatePrintableArea()
            Dim graphics As Graphics = printerSetting.CreateMeasurementGraphics()
            Dim paperWidth As Single = (paperSize.Width * 72.0F) / 100.0F
            Dim paperHeight As Single = (paperSize.Height * 72.0F) / 100.0F

            If (landscape.Checked) Then
                Dim temp As Single = paperHeight
                paperHeight = paperWidth
                paperWidth = temp
            End If


            Dim printableWidth As Single = (graphics.VisibleClipBounds.Width * 72.0F) / 100.0F
            Dim printableHeight As Single = (graphics.VisibleClipBounds.Height * 72.0F) / 100.0F

            printableArea = New RectangleF(0, 0, printableWidth, printableHeight)
            Dim dx As Single = (paperWidth - printableWidth) / 2
            Dim dy As Single = (paperHeight - printableHeight) / 2
            printableArea.Offset(dx, dy)
        End Sub

#End Region

#Region " Events"

        Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            paperSizes = New ArrayList
            paperSources = New ArrayList
            populatePrinterList()
        End Sub

        ' Ask the user to select a file and open it.
        Sub openFileMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles openFileMenu.Click
            Dim dialog As OpenFileDialog = New OpenFileDialog
            dialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
            If Windows.Forms.DialogResult.OK = dialog.ShowDialog() Then
                Using pdfFile As New FileStream(dialog.FileName, FileMode.Open, FileAccess.Read)
                    document = New Document(pdfFile)
                End Using

                Dim page As Page = document.Pages(PageIndex)

                If Not (Nothing Is pageBitmap) Then pageBitmap.Dispose()
                pageBitmap = New Bitmap(CInt(page.Width), CInt(page.Height))

                Using graph As Graphics = Graphics.FromImage(pageBitmap)
                    page.Draw(graph)
                End Using

                updatePreviewBox()
                updateTitleLabel(dialog.FileName)
                updateDocumentSizeLabel()
                updateScaleLabel()
                printerSetting.MaximumPage = document.Pages.Count
            End If
        End Sub

        ' Called whenever another printer is selected
        Private Sub printerList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles printerList.SelectedIndexChanged
            printerSetting = New PrinterSettings
            printerSetting.PrinterName = printerList.Items(printerList.SelectedIndex).ToString()
            If Not (Nothing Is document) Then
                printerSetting.MaximumPage = document.Pages.Count
            End If

            printerSetting.DefaultPageSettings.Landscape = landscape.Checked

            populatePaperSizeList()
            populatePaperSourceList()
        End Sub

        Private Sub cmdPrintPdf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles printButton.Click
            If (Nothing Is document) Then
                MessageBox.Show("You must first open a PDF document.")
                Return
            End If

            ' print asynchronously
            Dim threadStart As System.Threading.ThreadStart = New System.Threading.ThreadStart(AddressOf print)
            threadStart.BeginInvoke(Nothing, Nothing)
        End Sub

        Private Sub cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancelButton.Click
            Close()
        End Sub

        Private Sub paperSizeList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles paperSizeList.SelectedIndexChanged
            paperSize = CType(paperSizes.Item(paperSizeList.SelectedIndex), PaperSize)

            updatePrintableArea()

            updatePaperSizeLabel()
            updatePreviewBox()
            updateScaleLabel()
        End Sub

        Private Sub paperSourceList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles paperSourceList.SelectedIndexChanged
            paperSource = CType(paperSources.Item(paperSourceList.SelectedIndex), PaperSource)
        End Sub

        Private Sub LayoutChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scaleToFit.Click, autoRotate.Click, usePrintableArea.Click
            updatePreviewBox()
            updateScaleLabel()
        End Sub

        Private Sub AlignmentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles horAlignCenter.Click, horAlignLeft.Click, horAlignRight.Click, verAlignBottom.Click, verAlignCenter.Click, verAlignTop.Click
            updatePreviewBox()
        End Sub

        Private Sub previewPageIndex_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles previewPageIndex.ValueChanged
            If Nothing Is document Then
                Return
            End If
            Dim page As Page = document.Pages(PageIndex)

            If Not (Nothing Is pageBitmap) Then
                pageBitmap.Dispose()
            End If

            pageBitmap = New Bitmap(CInt(page.Width), CInt(page.Height))
            Dim graph As Graphics = Graphics.FromImage(pageBitmap)
            page.Draw(graph)

            updatePreviewBox()
        End Sub

        Private Sub range_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rangeFrom.TextChanged, rangeTo.TextChanged
            printRange.Checked = True
        End Sub

        Private Sub landscape_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles landscape.CheckedChanged

            printerSetting.DefaultPageSettings.Landscape = landscape.Checked

            updatePrintableArea()
            updatePreviewBox()
            updateScaleLabel()
        End Sub

#End Region

    End Class

End Namespace