Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
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

      AddFontsToMapping()

      Using pdfFile As New FileStream("..\PDF\FontDemo.PDF", FileMode.Open, FileAccess.Read)
         document = New Document(pdfFile)
      End Using

      RepaintPage()
   End Sub

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
      Me.Text = "Font mapping example"
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region " Variables"

   Private document As Document = Nothing
   Private bmp1 As Bitmap = Nothing
   Private bmp2 As Bitmap = Nothing
   Private renderSettings1 As RenderSettings = Nothing
   Private renderSettings2 As RenderSettings = Nothing

#End Region

#Region " Painting"

   Private Sub RepaintPage()
      If Not (Nothing Is bmp1) Then
         bmp1.Dispose()
      End If
      If Not (Nothing Is bmp2) Then
         bmp2.Dispose()
      End If

      Dim page As Page = document.Pages(0)

      ' Get a bitmap 4 time the size of the page. This is 288 DPI, almost 300
      bmp1 = New Bitmap(CInt(page.Width * 4), CInt(page.Height * 4))
      Using graph As Graphics = Graphics.FromImage(bmp1)
         graph.ScaleTransform(4.0, 4.0)
         page.Draw(graph, renderSettings1)
      End Using

      bmp2 = New Bitmap(CInt(page.Width * 4), CInt(page.Height * 4))
      Using graph As Graphics = Graphics.FromImage(bmp2)
         graph.ScaleTransform(4.0, 4.0)
         page.Draw(graph, renderSettings2)
      End Using

      view1.Invalidate()
      view2.Invalidate()
   End Sub

   Private Sub PaintPanel(ByVal bmp As Bitmap, ByVal view As Panel, ByVal graphics As Graphics)
      graphics.Clear(Color.White)
      If Nothing Is bmp Then
         Return
      End If
      Dim ratio As Single = CSng(bmp.Width / bmp.Height)
      Dim displayWidth, displayHeight As Single
      If (ratio * view.Height > view.Width) Then
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
   End Sub

#End Region

#Region " Events"

   Private Sub view1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles view1.Paint
      PaintPanel(bmp1, view1, e.Graphics)
   End Sub

   Private Sub view2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles view2.Paint
      PaintPanel(bmp2, view2, e.Graphics)
   End Sub

   Private Sub MainForm_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
      view1.Height = (ClientSize.Height - splitter.Height) >> 1
      view1.Invalidate()
      view2.Invalidate()
   End Sub

   Private Sub splitter_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles splitter.SplitterMoved
      view1.Invalidate()
      view2.Invalidate()
   End Sub

#End Region

   Private Sub AddFontsToMapping()
      ' This document (FontDenmo.pdf) has two fonts. You can verify this by loading it into
      ' Acrobat Reader and pressing <CTRL-D>. If you select the Fonts tab, you will see all
      ' the fonts used in this document.
      ' The Actual Font entry tells you what font Adobe uses to draw the text.
      ' With PDFRasterizer.NET, you specify the font substitution as below.
      ' There are two mappings, because we have two views, just to be able to see the difference.
      ' The program renders the same PDF twice, but with different Font substitutions.
      ' Have a look at the method RepaintPage() to see how these settings are passed to the Page

      renderSettings1 = New RenderSettings
      renderSettings1.TextSettings.FontSubstitutionMap.Add("TallComponents Demo Font", "ariali.ttf")
      renderSettings1.TextSettings.FontSubstitutionMap.Add("TallComponents Demo Font Bold", "arialbi.ttf")

      renderSettings2 = New RenderSettings
      renderSettings2.TextSettings.FontSubstitutionMap.Add("TallComponents Demo Font", "comic.ttf")
      renderSettings2.TextSettings.FontSubstitutionMap.Add("TallComponents Demo Font Bold", "comicbd.ttf")
   End Sub

End Class
