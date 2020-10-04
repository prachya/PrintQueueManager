using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace PDFViewer
{
	public class MainForm : System.Windows.Forms.Form
	{
		private Document document = null;
		private Bitmap bitmap = null;
		private int selectedPage = -1;
		private string path = null;
		private int dpi = 72;
		RenderSettings renderSettings = new RenderSettings();

		private const string CAPTION = "PDFRasterizer.NET Viewer Application";

		private System.Windows.Forms.ListBox pagesListBox;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel viewerPanel;
		private System.Windows.Forms.MenuItem fileOpen;
		private System.Windows.Forms.MenuItem fileClose;
		private System.Windows.Forms.MenuItem dpi48;
		private System.Windows.Forms.MenuItem dpi72;
		private System.Windows.Forms.MenuItem dpi150;
		private System.Windows.Forms.MenuItem dpi300;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem saveAsTiff;
		private System.Windows.Forms.MenuItem pageMenu;
		private System.Windows.Forms.MenuItem extractImages;
		private System.Windows.Forms.MenuItem saveAsBitmap;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem settingsMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem backgroundColor;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem fileSaveAsTiff;
		private MenuItem menuItem4;
		private MenuItem highSpeed;
		private MenuItem loadColorProfile;

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
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.fileOpen = new System.Windows.Forms.MenuItem();
			this.fileClose = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.fileSaveAsTiff = new System.Windows.Forms.MenuItem();
			this.pageMenu = new System.Windows.Forms.MenuItem();
			this.saveAsBitmap = new System.Windows.Forms.MenuItem();
			this.saveAsTiff = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.extractImages = new System.Windows.Forms.MenuItem();
			this.settingsMenu = new System.Windows.Forms.MenuItem();
			this.dpi48 = new System.Windows.Forms.MenuItem();
			this.dpi72 = new System.Windows.Forms.MenuItem();
			this.dpi150 = new System.Windows.Forms.MenuItem();
			this.dpi300 = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.backgroundColor = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.highSpeed = new System.Windows.Forms.MenuItem();
			this.loadColorProfile = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// pagesListBox
			// 
			this.pagesListBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.pagesListBox.IntegralHeight = false;
			this.pagesListBox.Location = new System.Drawing.Point( 0, 0 );
			this.pagesListBox.Name = "pagesListBox";
			this.pagesListBox.Size = new System.Drawing.Size( 120, 433 );
			this.pagesListBox.TabIndex = 0;
			this.pagesListBox.SelectedIndexChanged += new System.EventHandler( this.pagesListBox_SelectedIndexChanged );
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point( 120, 0 );
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size( 3, 433 );
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// viewerPanel
			// 
			this.viewerPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.viewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewerPanel.Location = new System.Drawing.Point( 123, 0 );
			this.viewerPanel.Name = "viewerPanel";
			this.viewerPanel.Size = new System.Drawing.Size( 373, 433 );
			this.viewerPanel.TabIndex = 2;
			this.viewerPanel.Resize += new System.EventHandler( this.viewerPanel_Resize );
			this.viewerPanel.Paint += new System.Windows.Forms.PaintEventHandler( this.viewerPanel_Paint );
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.fileMenu,
            this.pageMenu,
            this.settingsMenu} );
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.fileOpen,
            this.fileClose,
            this.menuItem3,
            this.fileSaveAsTiff} );
			this.fileMenu.Text = "File";
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
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "-";
			// 
			// fileSaveAsTiff
			// 
			this.fileSaveAsTiff.Index = 3;
			this.fileSaveAsTiff.Text = "Save as multi-page TIFF";
			this.fileSaveAsTiff.Click += new System.EventHandler( this.fileSaveAsTiff_Click );
			// 
			// pageMenu
			// 
			this.pageMenu.Index = 1;
			this.pageMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.saveAsBitmap,
            this.saveAsTiff,
            this.menuItem2,
            this.extractImages} );
			this.pageMenu.Text = "Page";
			// 
			// saveAsBitmap
			// 
			this.saveAsBitmap.Index = 0;
			this.saveAsBitmap.Text = "Save as bitmap";
			this.saveAsBitmap.Click += new System.EventHandler( this.saveAsBitmap_Click );
			// 
			// saveAsTiff
			// 
			this.saveAsTiff.Index = 1;
			this.saveAsTiff.Text = "Save as TIFF";
			this.saveAsTiff.Click += new System.EventHandler( this.saveAsTiff_Click );
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 2;
			this.menuItem2.Text = "-";
			// 
			// extractImages
			// 
			this.extractImages.Index = 3;
			this.extractImages.Text = "Extract Images";
			this.extractImages.Click += new System.EventHandler( this.extractImages_Click );
			// 
			// settingsMenu
			// 
			this.settingsMenu.Index = 2;
			this.settingsMenu.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.dpi48,
            this.dpi72,
            this.dpi150,
            this.dpi300,
            this.menuItem1,
            this.backgroundColor,
            this.menuItem4,
            this.highSpeed,
            this.loadColorProfile} );
			this.settingsMenu.Text = "Settings";
			// 
			// dpi48
			// 
			this.dpi48.Index = 0;
			this.dpi48.Text = "48 DPI";
			this.dpi48.Click += new System.EventHandler( this.dpi48_Click );
			// 
			// dpi72
			// 
			this.dpi72.Checked = true;
			this.dpi72.Index = 1;
			this.dpi72.Text = "72 DPI";
			this.dpi72.Click += new System.EventHandler( this.dpi72_Click );
			// 
			// dpi150
			// 
			this.dpi150.Index = 2;
			this.dpi150.Text = "150 DPI";
			this.dpi150.Click += new System.EventHandler( this.dpi150_Click );
			// 
			// dpi300
			// 
			this.dpi300.Index = 3;
			this.dpi300.Text = "300 DPI";
			this.dpi300.Click += new System.EventHandler( this.dpi300_Click );
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 4;
			this.menuItem1.Text = "-";
			// 
			// backgroundColor
			// 
			this.backgroundColor.Index = 5;
			this.backgroundColor.Text = "Background color...";
			this.backgroundColor.Click += new System.EventHandler( this.backgroundColor_Click );
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 6;
			this.menuItem4.Text = "-";
			// 
			// highSpeed
			// 
			this.highSpeed.Checked = true;
			this.highSpeed.Index = 7;
			this.highSpeed.Text = "High Speed";
			this.highSpeed.Click += new System.EventHandler( this.highSpeed_Click );
			// 
			// loadColorProfile
			// 
			this.loadColorProfile.Index = 8;
			this.loadColorProfile.Text = "Load Color Profile...";
			this.loadColorProfile.Click += new System.EventHandler( this.loadColorProfile_Click );
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
			this.ClientSize = new System.Drawing.Size( 496, 433 );
			this.Controls.Add( this.viewerPanel );
			this.Controls.Add( this.splitter1 );
			this.Controls.Add( this.pagesListBox );
			this.Menu = this.mainMenu;
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
			if ( null != bitmap )
			{
				bitmap.Dispose();
				bitmap = null;
				viewerPanel.Invalidate();
			}
			this.pagesListBox.Items.Clear();
			this.Text = CAPTION;
		}

		private void selectPage( int index )
		{
			if ( null == document ) return;

			try
			{
				Cursor = Cursors.WaitCursor;

				selectedPage = index;
				Page page = document.Pages[selectedPage];

				if ( null != bitmap )
				{
					bitmap.Dispose();
				}
				// In case you're missing some small details at the right and/or bottom edges,
				// you might want to increase the bitmap size with on pixel in one or both directions.
				bitmap = new Bitmap(
					( int ) ( page.Width / 72 * dpi ),
					( int ) ( page.Height / 72 * dpi ) );

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
               graphics.ScaleTransform((float)bitmap.Width / (float)page.Width, (float)bitmap.Height / (float)page.Height);

               graphics.SmoothingMode = SmoothingMode.AntiAlias;
               // Makes it probably a bit slower, but gives the best result.
               graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

               page.Draw(graphics, renderSettings);
            }

				viewerPanel.Invalidate();

				this.Text = CAPTION + string.Format( " (page {0} of {1})", selectedPage, path );
			}
			finally
			{
				Cursor = Cursors.Arrow;
			}
		}

		private void fileOpen_Click( object sender, System.EventArgs e )
		{
			close();

			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "PDF Files|*.pdf|All files|*.*";
			if ( DialogResult.OK == dialog.ShowDialog() )
			{
				path = dialog.FileName;

				using ( FileStream file = new FileStream( path, FileMode.Open, FileAccess.Read ) )
				{
					document = new Document( file );

					for ( int i = 0 ; i < document.Pages.Count ; i++ )
					{
						pagesListBox.Items.Add( "Page " + ( i + 1 ) );
					}
				}
				this.pagesListBox.SelectedIndex = 0;
			}
		}

		private void pagesListBox_SelectedIndexChanged( object sender, System.EventArgs e )
		{
			int index = pagesListBox.SelectedIndex;
			if ( index >= 0 && null != document && index < document.Pages.Count )
			{
				selectPage( index );
			}
		}

		private void viewerPanel_Paint( object sender, System.Windows.Forms.PaintEventArgs e )
		{
			e.Graphics.Clear( Color.White );
			if ( null != bitmap )
			{
				float ratio = ( float ) bitmap.Width / ( float ) bitmap.Height;
				float displayWidth, displayHeight;
				if ( ratio * viewerPanel.Height > viewerPanel.Width )
				{
					displayWidth = viewerPanel.Width - 10;
					displayHeight = displayWidth / ratio;
				}
				else
				{
					displayHeight = viewerPanel.Height - 10;
					displayWidth = displayHeight * ratio;
				}

				float x = ( viewerPanel.Width - displayWidth ) / 2;
				float y = ( viewerPanel.Height - displayHeight ) / 2;

				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				e.Graphics.DrawImage( bitmap, x, y, displayWidth, displayHeight );
				e.Graphics.DrawRectangle( new Pen( Color.Gray ), x, y, displayWidth, displayHeight );
			}
		}

		private void viewerPanel_Resize( object sender, System.EventArgs e )
		{
			viewerPanel.Invalidate();
		}

		private void fileClose_Click( object sender, System.EventArgs e )
		{
			close();
		}

		private void fileSaveAsTiff_Click( object sender, System.EventArgs e )
		{
			if ( null != document )
			{
				ConvertToTiffOptions options = new ConvertToTiffOptions( dpi, TiffCompression.CcittG4 );
				options.InvertImage = MessageBox.Show( "Do you want to invert the image?", "Question",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes;
				string fileName = string.Format( "{0}.tif", path );
				using ( FileStream stream = new FileStream( fileName, FileMode.Create ) )
				{
					document.ConvertToTiff( stream, options );
				}
				MessageBox.Show( "TIFF was created.", fileName );
			}
		}

		private void saveAsBitmap_Click( object sender, System.EventArgs e )
		{
			if ( null != bitmap )
			{
				string fileName = string.Format( "{0}_Page_{1}.png", path, selectedPage );
				bitmap.Save( fileName, ImageFormat.Png );
				MessageBox.Show( "Bitmap was created.", fileName );
			}
		}

		private void saveAsTiff_Click( object sender, System.EventArgs e )
		{
			if ( null != document )
			{
				ConvertToTiffOptions options = new ConvertToTiffOptions( dpi, TiffCompression.CcittG4 );
				options.InvertImage = MessageBox.Show( "Do you want to invert the image?", "Question",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes;
				string fileName = string.Format( "{0}_Page_{1}.tif", path, selectedPage );
				using ( FileStream stream = new FileStream( fileName, FileMode.Create ) )
				{
					document.Pages[selectedPage].ConvertToTiff( stream, options );
				}
				MessageBox.Show( "TIFF was created.", fileName );
			}
		}

		private void extractImages_Click( object sender, System.EventArgs e )
		{
			if ( null != document )
			{
				ExtractedImageInfo[] images = document.Pages[selectedPage].ExtractImages();
				int imageCnt = 0;
				foreach ( ExtractedImageInfo image in images )
				{
					string fileName = string.Format( "{0}_Page_{1}_Image_{2}.png", path, selectedPage, ++imageCnt );
					image.Bitmap.Save( fileName, ImageFormat.Png );
				}
				if ( 0 == imageCnt )
					MessageBox.Show( "No images found on this page.",
										  String.Format( "{0} Page {1}", path, selectedPage ) );
				else
					MessageBox.Show( String.Format( "{0} images have been extracted.", imageCnt ),
										  String.Format( "{0}_Page_{1}_Image_*.png", path, selectedPage ) );
			}
		}

		private void dpi48_Click( object sender, System.EventArgs e )
		{
			dpi48.Checked = true;
			dpi72.Checked = false;
			dpi150.Checked = false;
			dpi300.Checked = false;
			this.dpi = 48;
			selectPage( selectedPage );
		}

		private void dpi72_Click( object sender, System.EventArgs e )
		{
			dpi48.Checked = false;
			dpi72.Checked = true;
			dpi150.Checked = false;
			dpi300.Checked = false;
			this.dpi = 72;
			selectPage( selectedPage );
		}

		private void dpi150_Click( object sender, System.EventArgs e )
		{
			dpi48.Checked = false;
			dpi72.Checked = false;
			dpi150.Checked = true;
			dpi300.Checked = false;
			this.dpi = 150;
			selectPage( selectedPage );
		}

		private void dpi300_Click( object sender, System.EventArgs e )
		{
			dpi48.Checked = false;
			dpi72.Checked = false;
			dpi150.Checked = false;
			dpi300.Checked = true;
			this.dpi = 300;
			selectPage( selectedPage );
		}

		private void backgroundColor_Click( object sender, System.EventArgs e )
		{
			if ( null != document )
			{
				ColorDialog dialog = new ColorDialog();
				dialog.Color = renderSettings.ColorSettings.BackColor;
				if ( dialog.ShowDialog() == DialogResult.OK )
				{
					renderSettings.ColorSettings.BackColor = dialog.Color;
					selectPage( selectedPage );
				}
			}
		}

		private void highSpeed_Click( object sender, EventArgs e )
		{
			highSpeed.Checked = !highSpeed.Checked;
			if ( highSpeed.Checked )
			{
				renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighSpeed;
			}
			else
			{
				renderSettings.ColorSettings.TransformationMode = ColorTransformationMode.HighQuality;
			}
			selectPage( selectedPage );
		}

		private void loadColorProfile_Click( object sender, EventArgs e )
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "ICC Color Profile|*.icm|All files|*.*";
			dialog.InitialDirectory = Environment.GetFolderPath( System.Environment.SpecialFolder.System ) + @"\spool\drivers\color";
			if ( DialogResult.OK == dialog.ShowDialog() )
			{
				renderSettings.ColorSettings.OutputColorProfile = new ColorProfile( new FileStream( dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) );
				selectPage( selectedPage );
			}
		}
	}
}