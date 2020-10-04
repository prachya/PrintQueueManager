Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Windows.Forms

Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Fonts
Imports TallComponents.PDF.Rasterizer.Configuration

Public Class MainForm
   Inherits System.Windows.Forms.Form

#Region " Variables "

   Private document As Document = Nothing
   Private bmp1 As Bitmap = Nothing
   Private bmp2 As Bitmap = Nothing
   Private renderSettings1 As RenderSettings = Nothing
   Private renderSettings2 As RenderSettings = Nothing

#End Region

#Region " Constructor and destructor "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName

      Using pdfFile As New FileStream("..\PDF\FontDemo.PDF", FileMode.Open, FileAccess.Read)
         document = New Document(pdfFile)
      End Using

      AddFontsToMapping()

      RepaintPage()

   End Sub

   'Form overrides dispose to clean up the component list.
   Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
         If Not (components Is Nothing) Then
            components.Dispose()
         End If
      End If
      MyBase.Dispose(disposing)
   End Sub

#End Region

#Region " Windows Form Designer generated code "

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   Friend WithEvents view1 As System.Windows.Forms.Panel
   Friend WithEvents splitter As System.Windows.Forms.Splitter
   Friend WithEvents view2 As System.Windows.Forms.Panel
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.view1 = New System.Windows.Forms.Panel
      Me.splitter = New System.Windows.Forms.Splitter
      Me.view2 = New System.Windows.Forms.Panel
      Me.SuspendLayout()
      '
      'view1
      '
      Me.view1.Dock = System.Windows.Forms.DockStyle.Top
      Me.view1.Location = New System.Drawing.Point(0, 0)
      Me.view1.Name = "view1"
      Me.view1.Size = New System.Drawing.Size(712, 257)
      Me.view1.TabIndex = 3
      '
      'splitter
      '
      Me.splitter.BackColor = System.Drawing.Color.Black
      Me.splitter.Dock = System.Windows.Forms.DockStyle.Top
      Me.splitter.Location = New System.Drawing.Point(0, 257)
      Me.splitter.Name = "splitter"
      Me.splitter.Size = New System.Drawing.Size(712, 4)
      Me.splitter.TabIndex = 4
      Me.splitter.TabStop = False
      '
      'view2
      '
      Me.view2.Dock = System.Windows.Forms.DockStyle.Fill
      Me.view2.Location = New System.Drawing.Point(0, 261)
      Me.view2.Name = "view2"
      Me.view2.Size = New System.Drawing.Size(712, 257)
      Me.view2.TabIndex = 5
      '
      'MainForm
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(712, 518)
      Me.Controls.Add(Me.view2)
      Me.Controls.Add(Me.splitter)
      Me.Controls.Add(Me.view1)
      Me.Name = "MainForm"
      Me.Text = "OS Font usage example"
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region " Painting "

   Private Sub RepaintPage()
      If Not Nothing Is bmp1 Then
         bmp1.Dispose()
      End If
      If Not Nothing Is bmp2 Then
         bmp2.Dispose()
      End If

      Dim page As Page = document.Pages(0)
      ' Get a bitmap 4 times the size of the page. This is 288 DPI, almost 300
      bmp1 = New Bitmap(CInt(page.Width * 4), CInt(page.Height * 4))
      Using graphics As Graphics = graphics.FromImage(bmp1)
         graphics.ScaleTransform(4.0F, 4.0F)
         page.Draw(graphics, renderSettings1)
      End Using

      bmp2 = New Bitmap(CInt(page.Width * 4), CInt(page.Height * 4))
      Using graphics As Graphics = graphics.FromImage(bmp2)
         graphics.ScaleTransform(4.0F, 4.0F)
         page.Draw(graphics, renderSettings2)
      End Using
      view1.Invalidate()
      view2.Invalidate()
   End Sub

   Private Sub PaintPanel(ByVal bmp As Bitmap, ByVal view As Panel, ByVal graphics As Graphics)
      graphics.Clear(Color.White)
      If Not Nothing Is bmp Then
         Dim ratio As Single = bmp.Width / bmp.Height
         Dim displayWidth As Single, displayHeight As Single
         If ratio * view.Height > view.Width Then
            displayWidth = view.Width - 10
            displayHeight = displayWidth / ratio
         Else
            displayHeight = view.Height - 10
            displayWidth = displayHeight * ratio
         End If

         Dim x As Single = (view.Width - displayWidth) / 2
         Dim y As Single = (view.Height - displayHeight) / 2

         graphics.SmoothingMode = SmoothingMode.HighQuality
         graphics.InterpolationMode = InterpolationMode.High
         graphics.DrawImage(bmp, x, y, displayWidth, displayHeight)
         graphics.DrawRectangle(New Pen(Color.Gray), x, y, displayWidth, displayHeight)
      End If
   End Sub

#End Region

#Region " Events "

   Private Sub MainForm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
      view1.Height = (ClientSize.Height - splitter.Height) >> 1
      view1.Invalidate()
      view2.Invalidate()
   End Sub

   Private Sub view1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles view1.Paint
      PaintPanel(bmp1, view1, e.Graphics)
   End Sub

   Private Sub view2_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles view2.Paint
      PaintPanel(bmp2, view2, e.Graphics)
   End Sub

   Private Sub splitter_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles splitter.Move
      view1.Invalidate()
      view2.Invalidate()
   End Sub

#End Region

   Private Sub AddFontsToMapping()
      ' This document (FontDemo.pdf) has two fonts. You can verify this by loading it into
      ' Acrobat Reader and pressing <CTRL-D>. If you select the Fonts tab, you will see all
      ' the fonts used in this document.
      ' The Actual Font entry tells you what font Adobe uses to draw the text.
      ' With PDFRasterizer.NET, you specify the font substitution as below.
      ' There are two mappings, because we have two views, just to be able to see the difference.
      ' The program renders the same PDF twice, but with different Font substitutions.
      ' Have a look at the method RepaintPage() to see how these settings are passed to the Page
      ' If you use this when printing documents, the data file send to the printer will become much smaller.

      renderSettings1 = New RenderSettings
      AddHandler renderSettings1.TextSettings.ResolveFont, AddressOf Me.renderSettings1_ResolveFont

      ' Here we get all the fonts in the document and set the same substitute font.
      renderSettings2 = New RenderSettings
      AddHandler renderSettings2.TextSettings.ResolveFont, AddressOf Me.renderSettings2_ResolveFont

   End Sub

   Private Sub renderSettings1_ResolveFont(ByVal sender As TextRenderSettings, ByVal args As ResolveFontEventArgs)

      ' Map all non-system fonts to the system font arial

      If (Not args.FontLocation = FontLocation.System) Then
         args.FontRenderMode = FontRenderMode.RenderAsFont

         If (args.PdfFontName.EndsWith("Bold")) Then
            args.Bold = True
         End If

         args.SystemFontName = "Arial"
      End If
   End Sub

   Private Sub renderSettings2_ResolveFont(ByVal sender As TextRenderSettings, ByVal args As ResolveFontEventArgs)
      'Map all non-system fonts to the system font Arial narrow.

      If (Not args.FontLocation = FontLocation.System) Then
         args.FontRenderMode = FontRenderMode.RenderAsFont
         args.SystemFontName = "Arial Narrow"
      End If

   End Sub

End Class
