using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices; // for Marshal.Copy

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace PDFViewerEMF
{
	public class MainForm : System.Windows.Forms.Form
	{
		private Document document = null;
		private Metafile[] metafiles = null;
		private int selectedPage = -1;
		private string path = null;

		private const string CAPTION = "PDFRasterizer.NET Viewer Application";

		private System.Windows.Forms.ListBox pagesListBox;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel viewerPanel;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem fileOpen;
		private System.Windows.Forms.MenuItem fileClose;

		public MainForm()
		{
			InitializeComponent();
			this.Text = CAPTION;

			TextRenderSettings.CMapFolder = new DirectoryInfo( @"..\..\..\..\..\Support\CMaps\" ).FullName;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pagesListBox = new System.Windows.Forms.ListBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.viewerPanel = new System.Windows.Forms.Panel();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.fileOpen = new System.Windows.Forms.MenuItem();
			this.fileClose = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// pagesListBox
			// 
			this.pagesListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.pagesListBox.IntegralHeight = false;
			this.pagesListBox.Name = "pagesListBox";
			this.pagesListBox.Size = new System.Drawing.Size( 120, 266 );
			this.pagesListBox.TabIndex = 0;
			this.pagesListBox.SelectedIndexChanged += new System.EventHandler( this.pagesListBox_SelectedIndexChanged );
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 120, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 266 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// viewerPanel
			// 
			this.viewerPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.viewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewerPanel.Location = new System.Drawing.Point( 123, 0 );
			this.viewerPanel.Name = "viewerPanel";
			this.viewerPanel.Size = new System.Drawing.Size( 169, 266 );
			this.viewerPanel.TabIndex = 2;
			this.viewerPanel.Resize += new System.EventHandler( this.viewerPanel_Resize );
			this.viewerPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.viewerPanel_Paint );
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
                                                                                  this.menuItem1} );
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
                                                                                  this.fileOpen,
                                                                                  this.fileClose} );
			this.menuItem1.Text = "File";
			// 
			// fileOpen
			// 
			this.fileOpen.Index = 0;
			this.fileOpen.Text = "Open...";
			this.fileOpen.Click += new System.EventHandler( this.fileOpen_Click );
			// 
			// fileClose
			// 
			this.fileClose.Index = 1;
			this.fileClose.Text = "Close";
			this.fileClose.Click += new System.EventHandler( this.fileClose_Click );
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 292, 266 );
			this.Controls.AddRange( new System.Windows.Forms.Control[] {
                                                                      this.viewerPanel,
                                                                      this.splitter1,
                                                                      this.pagesListBox} );
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "PDFViewer Demo Application";
			this.ResumeLayout( false );

		}
		#endregion


		[STAThread]
		static void Main()
		{
			Application.Run( new MainForm() );
		}

		private void close()
		{
			document = null;
			if ( null != metafiles )
			{
				foreach ( Metafile metafile in metafiles )
				{
					if ( null != metafile ) metafile.Dispose();
				}
				metafiles = null;
			}
			viewerPanel.Invalidate();
			this.pagesListBox.Items.Clear();
			this.Text = CAPTION;
		}

		private Metafile createMetafile( Page page )
		{
			Metafile metafile = null;

			using ( Graphics graphics = this.CreateGraphics() )
			{
				System.IntPtr hdc = graphics.GetHdc();
            metafile = new Metafile(hdc, new System.Drawing.RectangleF(0, 0, (float)page.Width, (float)page.Height), MetafileFrameUnit.Point);
				graphics.ReleaseHdc( hdc );
			}

			using ( Graphics metafileGraphics = Graphics.FromImage( metafile ) )
			{
				metafileGraphics.SmoothingMode = SmoothingMode.AntiAlias;
				page.Draw( metafileGraphics );
			}

			return metafile;
		}

		private bool MetafileCallback(
			EmfPlusRecordType recordType,
			int flags,
			int dataSize,
			IntPtr data,
			PlayRecordCallback callbackData )
		{
			byte[] dataArray = null;
			if ( data != IntPtr.Zero )
			{
				// Copy the unmanaged record to a managed byte buffer 
				// that can be used by PlayRecord.
				dataArray = new byte[dataSize];
				Marshal.Copy( data, dataArray, 0, dataSize );
			}

			metafiles[selectedPage].PlayRecord( recordType, flags, dataSize, dataArray );

			return true;
		}

		private void viewerPanel_Paint( object sender, System.Windows.Forms.PaintEventArgs e )
		{
			if ( null == document ) return;

			e.Graphics.Clear( Color.White );

			if ( null == metafiles[selectedPage] ) // lazy evaluation
			{
				metafiles[selectedPage] = createMetafile( document.Pages[selectedPage] );
			}

			if ( null != metafiles[selectedPage] )
			{
				Page page = document.Pages[selectedPage];

				float scale = ( float ) Math.Min( viewerPanel.Width / page.Width, viewerPanel.Height / page.Height );

				e.Graphics.ScaleTransform( scale, scale );
				e.Graphics.EnumerateMetafile( metafiles[selectedPage], new Point( 0, 0 ), new Graphics.EnumerateMetafileProc( MetafileCallback ) );
			}
		}

		private void viewerPanel_Resize( object sender, System.EventArgs e )
		{
			viewerPanel.Invalidate();
		}

		private void fileOpen_Click( object sender, System.EventArgs e )
		{
			close();

			OpenFileDialog dialog = new OpenFileDialog();
			if ( DialogResult.OK == dialog.ShowDialog() )
			{
				path = dialog.FileName;

				using ( FileStream file = new FileStream( path, FileMode.Open, FileAccess.Read ) )
				{
					document = new Document( file );
					metafiles = new Metafile[document.Pages.Count]; // metafile cache

					for ( int i = 0 ; i < document.Pages.Count ; i++ )
					{
						pagesListBox.Items.Add( "Page " + ( i + 1 ) );
					}
				}

				this.pagesListBox.SelectedIndex = 0; // initial selection
			}
		}

		private void fileClose_Click( object sender, System.EventArgs e )
		{
			close();
		}

		private void pagesListBox_SelectedIndexChanged( object sender, System.EventArgs e )
		{
			int index = pagesListBox.SelectedIndex;
			if ( index >= 0 && null != document && index < document.Pages.Count )
			{
				selectedPage = index;
			}
			viewerPanel.Invalidate();
		}
	}
}
