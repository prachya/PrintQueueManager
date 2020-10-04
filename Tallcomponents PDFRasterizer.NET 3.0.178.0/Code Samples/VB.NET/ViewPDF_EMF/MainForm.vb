Imports System
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Windows.Forms
Imports System.Runtime.InteropServices ' for Marshal.Copy

Imports TallComponents.PDF.Rasterizer
Imports TallComponents.PDF.Rasterizer.Configuration

Namespace PDFViewerEMF

   Public Class MainForm
      Inherits System.Windows.Forms.Form
      Private document As Document = Nothing
      Private metafiles As Metafile()
      Private selectedPage As Integer = -1
      Private path As String = Nothing

      Private Const CAPTION As String = "PDFRasterizer.NET Viewer Application"

      Private WithEvents pagesListBox As System.Windows.Forms.ListBox
      Private splitter1 As System.Windows.Forms.Splitter
      Private WithEvents viewerPanel As System.Windows.Forms.Panel
      Private mainMenu1 As System.Windows.Forms.MainMenu
      Private menuItem1 As System.Windows.Forms.MenuItem
      Private WithEvents fileOpen As System.Windows.Forms.MenuItem
      Private WithEvents fileClose As System.Windows.Forms.MenuItem

      Public Sub New()
         InitializeComponent()
         Me.Text = CAPTION
         TextRenderSettings.CMapFolder = New DirectoryInfo("..\..\..\..\..\Support\CMaps\").FullName
      End Sub

      Private Sub InitializeComponent()
         Me.pagesListBox = New System.Windows.Forms.ListBox
         Me.splitter1 = New System.Windows.Forms.Splitter
         Me.viewerPanel = New System.Windows.Forms.Panel
         Me.mainMenu1 = New System.Windows.Forms.MainMenu
         Me.menuItem1 = New System.Windows.Forms.MenuItem
         Me.fileOpen = New System.Windows.Forms.MenuItem
         Me.fileClose = New System.Windows.Forms.MenuItem
         Me.SuspendLayout()
         '
         'pagesListBox
         '
         Me.pagesListBox.Dock = System.Windows.Forms.DockStyle.Left
         Me.pagesListBox.IntegralHeight = False
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
         Me.mainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem1})
         '
         'menuItem1
         '
         Me.menuItem1.Index = 0
         Me.menuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.fileOpen, Me.fileClose})
         Me.menuItem1.Text = "File"
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
         'MainForm
         '
         Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
         Me.ClientSize = New System.Drawing.Size(536, 406)
         Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.viewerPanel, Me.splitter1, Me.pagesListBox})
         Me.Menu = Me.mainMenu1
         Me.Name = "MainForm"
         Me.Text = "PDFViewer Demo Application"
         Me.ResumeLayout(False)

      End Sub

      <STAThread()> _
      Shared Sub Main()
         Application.Run(New MainForm)
      End Sub

      Private Overloads Sub closeDoc()
         document = Nothing

         If Not metafiles Is Nothing Then
            Dim metafile As Metafile
            For Each metafile In metafiles
               If Not metafile Is Nothing Then metafile.Dispose()
            Next
         End If

         viewerPanel.Invalidate()
         Me.pagesListBox.Items.Clear()
         Me.Text = CAPTION
      End Sub

      Private Function createMetafile(ByVal page As Page) As Metafile

         Dim metafile As Metafile
         Using graph As Graphics = Me.CreateGraphics()
            Dim hdc As System.IntPtr = graph.GetHdc()
            metafile = New Metafile(hdc, New System.Drawing.RectangleF(0, 0, CSng(page.Width), CSng(page.Height)), MetafileFrameUnit.Point)
            graph.ReleaseHdc(hdc)
         End Using

         Using metafileGraphics As Graphics = Graphics.FromImage(metafile)
            metafileGraphics.SmoothingMode = SmoothingMode.AntiAlias
            page.Draw(metafileGraphics)
         End Using

         Return metafile
      End Function

      Private Function MetafileCallback(ByVal recordType As EmfPlusRecordType, ByVal flags As Integer, ByVal dataSize As Integer, ByVal data As IntPtr, ByVal callbackData As PlayRecordCallback) As Boolean
         Dim dataArray(dataSize) As Byte
         If Not data.Equals(IntPtr.Zero) Then
            ' Copy the unmanaged record to a managed byte buffer 
            ' that can be used by PlayRecord.
            Marshal.Copy(data, dataArray, 0, dataSize)
         End If

         metafiles(selectedPage).PlayRecord(recordType, flags, dataSize, dataArray)

         Return True
      End Function

      Private Sub viewerPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles viewerPanel.Paint
         If document Is Nothing Then Return

         e.Graphics.Clear(Color.White)

         If metafiles(selectedPage) Is Nothing Then   ' lazy evaluation
            metafiles(selectedPage) = createMetafile(document.Pages(selectedPage))
         End If

         Dim page As Page = document.Pages(selectedPage)
         Dim scale As Double = Math.Min(viewerPanel.Width / page.Width, viewerPanel.Height / page.Height)

         e.Graphics.ScaleTransform(CSng(scale), CSng(scale))
         e.Graphics.EnumerateMetafile(metafiles(selectedPage), New Point(0, 0), AddressOf MetafileCallback)
      End Sub

      Private Sub viewerPanel_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewerPanel.Resize
         viewerPanel.Invalidate()
      End Sub

      Private Sub fileOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fileOpen.Click
         closeDoc()

         Dim dialog As OpenFileDialog = New OpenFileDialog
         If Windows.Forms.DialogResult.OK = dialog.ShowDialog Then
            path = dialog.FileName

            Using pdfFile As FileStream = New FileStream(path, FileMode.Open, FileAccess.Read)
               document = New Document(pdfFile)

               ReDim metafiles(document.Pages.Count) ' metafile cache

               Dim i As Integer = 0
               For i = 1 To document.Pages.Count
                  pagesListBox.Items.Add("Page " & i)
               Next i

               Me.pagesListBox.SelectedIndex = 0
            End Using
         End If
      End Sub

      Private Sub fileClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fileClose.Click
         closeDoc()
      End Sub

      Private Sub pagesListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pagesListBox.SelectedIndexChanged
         Dim index As Integer = pagesListBox.SelectedIndex
         If index >= 0 AndAlso Not (Nothing Is document) AndAlso index < document.Pages.Count Then
            selectedPage = index
         End If
         viewerPanel.Invalidate()
      End Sub
   End Class
End Namespace

