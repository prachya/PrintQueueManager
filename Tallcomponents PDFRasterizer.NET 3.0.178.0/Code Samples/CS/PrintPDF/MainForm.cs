using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace PrintDemo
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
      private System.Windows.Forms.ToolBarButton toolBarButton3;
      private System.Windows.Forms.ToolBarButton toolBarButton5;
      private System.Windows.Forms.ListBox pagesList;
      private System.Windows.Forms.ToolBar toolBar;
      private System.Windows.Forms.MenuItem openFile;
      private System.Windows.Forms.MenuItem fileMenu;
      private System.Windows.Forms.MainMenu mainMenu;
      private System.Windows.Forms.MenuItem menuItem1;
      private System.Windows.Forms.MenuItem autoPositionMenuItem;
      private System.Windows.Forms.ImageList imageList;

		public MainForm()
		{
			InitializeComponent();

			TextRenderSettings.CMapFolder = new DirectoryInfo( @"..\..\..\..\..\Support\CMaps\" ).FullName;
      }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
         this.mainMenu = new System.Windows.Forms.MainMenu();
         this.fileMenu = new System.Windows.Forms.MenuItem();
         this.openFile = new System.Windows.Forms.MenuItem();
         this.menuItem1 = new System.Windows.Forms.MenuItem();
         this.autoPositionMenuItem = new System.Windows.Forms.MenuItem();
         this.imageList = new System.Windows.Forms.ImageList();
         this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
         this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
         this.toolBar = new System.Windows.Forms.ToolBar();
         this.pagesList = new System.Windows.Forms.ListBox();
         this.SuspendLayout();
         // 
         // mainMenu
         // 
         this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                 this.fileMenu,
                                                                                 this.menuItem1});
         // 
         // fileMenu
         // 
         this.fileMenu.Index = 0;
         this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                 this.openFile});
         this.fileMenu.Text = "File";
         // 
         // openFile
         // 
         this.openFile.Index = 0;
         this.openFile.Text = "Open...";
         this.openFile.Click += new System.EventHandler(this.openFile_Click);
         // 
         // menuItem1
         // 
         this.menuItem1.Index = 1;
         this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                  this.autoPositionMenuItem});
         this.menuItem1.Text = "Options";
         // 
         // autoPositionMenuItem
         // 
         this.autoPositionMenuItem.Checked = true;
         this.autoPositionMenuItem.Index = 0;
         this.autoPositionMenuItem.Text = "Auto-scale and center";
         this.autoPositionMenuItem.Click += new System.EventHandler(this.autoPositionMenuItem_Click);
         // 
         // imageList
         // 
         this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
         this.imageList.ImageSize = new System.Drawing.Size(16, 16);
         this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
         this.imageList.TransparentColor = System.Drawing.Color.Red;
         // 
         // toolBarButton3
         // 
         this.toolBarButton3.ImageIndex = 0;
         this.toolBarButton3.Tag = "print";
         this.toolBarButton3.ToolTipText = "Print selection";
         // 
         // toolBarButton5
         // 
         this.toolBarButton5.ImageIndex = 1;
         this.toolBarButton5.Tag = "preview";
         this.toolBarButton5.ToolTipText = "Preview selection";
         // 
         // toolBar
         // 
         this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
         this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                   this.toolBarButton3,
                                                                                   this.toolBarButton5});
         this.toolBar.DropDownArrows = true;
         this.toolBar.ImageList = this.imageList;
         this.toolBar.Name = "toolBar";
         this.toolBar.ShowToolTips = true;
         this.toolBar.Size = new System.Drawing.Size(292, 25);
         this.toolBar.TabIndex = 0;
         this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
         // 
         // pagesList
         // 
         this.pagesList.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pagesList.Location = new System.Drawing.Point(0, 25);
         this.pagesList.Name = "pagesList";
         this.pagesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
         this.pagesList.Size = new System.Drawing.Size(292, 238);
         this.pagesList.TabIndex = 1;
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                      this.pagesList,
                                                                      this.toolBar});
         this.Menu = this.mainMenu;
         this.Name = "MainForm";
         this.Text = "PDFRasterizer.NET Print Demo";
         this.ResumeLayout(false);

      }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run( new MainForm() );
		}

      private void openFile_Click(object sender, System.EventArgs e)
      {
         OpenFileDialog dialog = new OpenFileDialog();
         if ( dialog.ShowDialog() == DialogResult.OK )
         {
            using ( FileStream file = new FileStream( dialog.FileName, FileMode.Open, FileAccess.Read ))
            {
               document = new Document( file );
               
               pagesList.Items.Clear();
               for ( int i=0; i<document.Pages.Count; i++ )
               {
                  pagesList.Items.Add( "page " + i.ToString() );
               }
               
               // select first page
               if ( pagesList.Items.Count > 0 ) pagesList.SelectedIndex = 0;
            }  
         }
      }

      void print()
      {
         if ( null == document || pagesList.SelectedIndices.Count == 0 ) return;

         selectedPages = pagesList.SelectedIndices.GetEnumerator();
         selectedPages.Reset();

         if ( selectedPages.MoveNext() )
         {
            PrintDocument printDocument = new PrintDocument();
            printDocument.DocumentName = document.DocumentInfo.Title;

            printDocument.DefaultPageSettings.Margins.Left = 0;
            printDocument.DefaultPageSettings.Margins.Top = 0;
            printDocument.OriginAtMargins = true;

            PageSetupDialog setupDialog = new PageSetupDialog();
            setupDialog.Document = printDocument;
         
            if ( DialogResult.OK == setupDialog.ShowDialog() )
            {
               printDocument.DefaultPageSettings = setupDialog.PageSettings;
               printDocument.PrinterSettings = setupDialog.PrinterSettings;

               printDocument.PrintPage += new PrintPageEventHandler( printPage );
               printDocument.Print();
            }
         }
      }

      void printPreview()
      {
         if ( null == document || pagesList.SelectedIndices.Count == 0 ) return;

         selectedPages = pagesList.SelectedIndices.GetEnumerator();
         selectedPages.Reset();

         if ( selectedPages.MoveNext() )
         {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler( printPage );

            PrintPreviewDialog previewDialog = new PrintPreviewDialog();
            previewDialog.Document = printDocument;
            previewDialog.ShowDialog();
         }
      }

      void printPage( object sender, PrintPageEventArgs e )
      {
         e.Graphics.PageUnit = GraphicsUnit.Point;
         
         if ( null != selectedPages.Current )
         {
            int pageIndex = (int) selectedPages.Current;
            Page page = document.Pages[ pageIndex++ ];
            
            if ( this.autoPositionMenuItem.Checked )
            {
               autoPosition( 
                  e.Graphics,
                  ( e.PageSettings.PaperSize.Width * 72 ) / 100, ( e.PageSettings.PaperSize.Height * 72 ) / 100,
                  // In .NET 2.0, you can use the printable area instead of the papersize.
                  // ( e.PageSettings.PrintableArea.Width * 72 ) / 100, ( e.PageSettings.PrintableArea.Height * 72 ) / 100,
                  page.Width, page.Height );
            }

            page.Draw( e.Graphics );
         }

         e.HasMorePages = selectedPages.MoveNext();
      }

      void autoPosition( Graphics graphics, double printerWidth, double printerHeight, double pdfWidth, double pdfHeight )
      {
         double scaleX = printerWidth / pdfWidth;
         double scaleY = printerHeight / pdfHeight;
         double scale = Math.Min( scaleX, scaleY );

         double scaledPdfWidth = scale * pdfWidth;
         double scaledPdfHeight = scale * pdfHeight;
         
         double dx = (printerWidth - scaledPdfWidth ) / 2;
         double dy = (printerHeight - scaledPdfHeight ) / 2;

         graphics.TranslateTransform( (float)dx, (float)dy );
         graphics.ScaleTransform((float)scale, (float)scale);
      }

      private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
      {
         switch ( e.Button.Tag.ToString() )
         {
            case "print":
               if ( null != document ) print();
               break;
            case "preview":
               if ( null != document ) printPreview();
               break;
         }
      }

      private void autoPositionMenuItem_Click(object sender, System.EventArgs e)
      {
         autoPositionMenuItem.Checked = !autoPositionMenuItem.Checked;
      }

      IEnumerator selectedPages;
      Document document;
	}
}
