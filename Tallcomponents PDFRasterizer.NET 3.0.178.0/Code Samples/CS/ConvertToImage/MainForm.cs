using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace ConvertToImage_cs
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
      private System.Windows.Forms.MainMenu mainMenu1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.RadioButton radioButton3;
      private System.Windows.Forms.ComboBox selectedPage;
      private System.Windows.Forms.Button selectPdfDocument;
      private System.Windows.Forms.GroupBox formatGroup;
      private System.Windows.Forms.RadioButton tiffFormat;
      private System.Windows.Forms.RadioButton pngFormat;
      private System.Windows.Forms.RadioButton bmpFormat;
      private System.Windows.Forms.RadioButton jpegFormat;
      private System.Windows.Forms.GroupBox dpiGroup;
      private System.Windows.Forms.RadioButton dpi300;
      private System.Windows.Forms.RadioButton dpi150;
      private System.Windows.Forms.RadioButton dpi72;
      private System.Windows.Forms.Button saveAsImage;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
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
         this.mainMenu1 = new System.Windows.Forms.MainMenu();
         this.label1 = new System.Windows.Forms.Label();
         this.selectedPage = new System.Windows.Forms.ComboBox();
         this.selectPdfDocument = new System.Windows.Forms.Button();
         this.formatGroup = new System.Windows.Forms.GroupBox();
         this.tiffFormat = new System.Windows.Forms.RadioButton();
         this.radioButton3 = new System.Windows.Forms.RadioButton();
         this.pngFormat = new System.Windows.Forms.RadioButton();
         this.bmpFormat = new System.Windows.Forms.RadioButton();
         this.jpegFormat = new System.Windows.Forms.RadioButton();
         this.dpiGroup = new System.Windows.Forms.GroupBox();
         this.dpi300 = new System.Windows.Forms.RadioButton();
         this.dpi150 = new System.Windows.Forms.RadioButton();
         this.dpi72 = new System.Windows.Forms.RadioButton();
         this.saveAsImage = new System.Windows.Forms.Button();
         this.formatGroup.SuspendLayout();
         this.dpiGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(8, 56);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(264, 23);
         this.label1.TabIndex = 0;
         this.label1.Text = "Select page";
         // 
         // selectedPage
         // 
         this.selectedPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.selectedPage.Enabled = false;
         this.selectedPage.Location = new System.Drawing.Point(8, 72);
         this.selectedPage.Name = "selectedPage";
         this.selectedPage.Size = new System.Drawing.Size(272, 21);
         this.selectedPage.TabIndex = 1;
         // 
         // selectPdfDocument
         // 
         this.selectPdfDocument.Location = new System.Drawing.Point(8, 16);
         this.selectPdfDocument.Name = "selectPdfDocument";
         this.selectPdfDocument.Size = new System.Drawing.Size(272, 23);
         this.selectPdfDocument.TabIndex = 2;
         this.selectPdfDocument.Text = "Select PDF document...";
         this.selectPdfDocument.Click += new System.EventHandler(this.selectPdfDocument_Click);
         // 
         // formatGroup
         // 
         this.formatGroup.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                  this.tiffFormat,
                                                                                  this.radioButton3,
                                                                                  this.pngFormat,
                                                                                  this.bmpFormat,
                                                                                  this.jpegFormat});
         this.formatGroup.Enabled = false;
         this.formatGroup.Location = new System.Drawing.Point(8, 112);
         this.formatGroup.Name = "formatGroup";
         this.formatGroup.Size = new System.Drawing.Size(128, 136);
         this.formatGroup.TabIndex = 3;
         this.formatGroup.TabStop = false;
         this.formatGroup.Text = "Image format";
         // 
         // tiffFormat
         // 
         this.tiffFormat.Location = new System.Drawing.Point(16, 72);
         this.tiffFormat.Name = "tiffFormat";
         this.tiffFormat.TabIndex = 7;
         this.tiffFormat.Text = "TIFF";
         // 
         // radioButton3
         // 
         this.radioButton3.Location = new System.Drawing.Point(16, 72);
         this.radioButton3.Name = "radioButton3";
         this.radioButton3.TabIndex = 6;
         this.radioButton3.Text = "radioButton3";
         // 
         // pngFormat
         // 
         this.pngFormat.Location = new System.Drawing.Point(16, 48);
         this.pngFormat.Name = "pngFormat";
         this.pngFormat.TabIndex = 5;
         this.pngFormat.Text = "PNG";
         // 
         // bmpFormat
         // 
         this.bmpFormat.Checked = true;
         this.bmpFormat.Location = new System.Drawing.Point(16, 24);
         this.bmpFormat.Name = "bmpFormat";
         this.bmpFormat.TabIndex = 4;
         this.bmpFormat.TabStop = true;
         this.bmpFormat.Text = "BMP";
         // 
         // jpegFormat
         // 
         this.jpegFormat.Location = new System.Drawing.Point(16, 96);
         this.jpegFormat.Name = "jpegFormat";
         this.jpegFormat.TabIndex = 8;
         this.jpegFormat.Text = "JPEG";
         // 
         // dpiGroup
         // 
         this.dpiGroup.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                               this.dpi300,
                                                                               this.dpi150,
                                                                               this.dpi72});
         this.dpiGroup.Enabled = false;
         this.dpiGroup.Location = new System.Drawing.Point(152, 112);
         this.dpiGroup.Name = "dpiGroup";
         this.dpiGroup.Size = new System.Drawing.Size(128, 136);
         this.dpiGroup.TabIndex = 4;
         this.dpiGroup.TabStop = false;
         this.dpiGroup.Text = "Output resolution";
         // 
         // dpi300
         // 
         this.dpi300.Location = new System.Drawing.Point(16, 72);
         this.dpi300.Name = "dpi300";
         this.dpi300.Size = new System.Drawing.Size(96, 24);
         this.dpi300.TabIndex = 2;
         this.dpi300.Text = "300 DPI";
         // 
         // dpi150
         // 
         this.dpi150.Location = new System.Drawing.Point(16, 48);
         this.dpi150.Name = "dpi150";
         this.dpi150.Size = new System.Drawing.Size(96, 24);
         this.dpi150.TabIndex = 1;
         this.dpi150.Text = "150 DPI";
         // 
         // dpi72
         // 
         this.dpi72.Checked = true;
         this.dpi72.Location = new System.Drawing.Point(16, 24);
         this.dpi72.Name = "dpi72";
         this.dpi72.Size = new System.Drawing.Size(96, 24);
         this.dpi72.TabIndex = 0;
         this.dpi72.TabStop = true;
         this.dpi72.Text = "72 DPI";
         // 
         // saveAsImage
         // 
         this.saveAsImage.Enabled = false;
         this.saveAsImage.Location = new System.Drawing.Point(8, 264);
         this.saveAsImage.Name = "saveAsImage";
         this.saveAsImage.Size = new System.Drawing.Size(272, 23);
         this.saveAsImage.TabIndex = 5;
         this.saveAsImage.Text = "Save as image...";
         this.saveAsImage.Click += new System.EventHandler(this.saveAsImage_Click);
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(290, 294);
         this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                      this.saveAsImage,
                                                                      this.dpiGroup,
                                                                      this.formatGroup,
                                                                      this.selectPdfDocument,
                                                                      this.selectedPage,
                                                                      this.label1});
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.Menu = this.mainMenu1;
         this.Name = "MainForm";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = "Save PDF as image";
         this.formatGroup.ResumeLayout(false);
         this.dpiGroup.ResumeLayout(false);
         this.ResumeLayout(false);

      }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

      private void selectPdfDocument_Click(object sender, System.EventArgs e)
      {
         OpenFileDialog dialog = new OpenFileDialog();
         dialog.DefaultExt = ".pdf";
         if ( DialogResult.OK == dialog.ShowDialog() )
         {
            string path = dialog.FileName;
            using ( FileStream file = new FileStream( path, FileMode.Open, FileAccess.Read ) )
            {
               byte[] buffer = new byte[ file.Length ];
               file.Read( buffer, 0, buffer.Length );
               document = new Document( new BinaryReader( new MemoryStream( buffer ) ) );

               selectedPage.Items.Clear();
               for ( int i=1; i<=document.Pages.Count; i++ ) selectedPage.Items.Add( i );
               selectedPage.SelectedIndex = 0;
            }
         }

         updateControls();
      }

      void updateControls()
      {
         selectedPage.Enabled = formatGroup.Enabled = dpiGroup.Enabled = saveAsImage.Enabled = ( null != document && document.Pages.Count > 0 );
      }

      private void saveAsImage_Click(object sender, System.EventArgs e)
      {
         SaveFileDialog dialog = new SaveFileDialog();
         dialog.Filter = getImageFilter();
         if ( DialogResult.OK == dialog.ShowDialog() )
         {
            Page page = document.Pages[ selectedPage.SelectedIndex ];
            int dpi = getDPI();
            float scale = (float) dpi / (float) 72;
            using ( Bitmap bitmap = new Bitmap( (int) ( scale * page.Width ), (int) ( scale * page.Height ) ) )
            {
               using (Graphics graphics = Graphics.FromImage( bitmap ))
               {
                  graphics.SmoothingMode = SmoothingMode.AntiAlias;
                  graphics.ScaleTransform( scale, scale ); 
                  graphics.Clear( Color.White );
                  page.Draw(graphics);
               }
               bitmap.Save( dialog.FileName, getImageFormat() );
            }
         }
      }

      int getDPI()
      {
         if ( dpi72.Checked ) return 72;
         if ( dpi150.Checked ) return 150;
         return 300;
      }

      ImageFormat getImageFormat()
      {
         if ( bmpFormat.Checked ) return ImageFormat.Bmp;
         if ( pngFormat.Checked ) return ImageFormat.Png;
         if ( tiffFormat.Checked ) return ImageFormat.Tiff;
         return ImageFormat.Jpeg;
      }

      string getImageFilter()
      {
         if ( bmpFormat.Checked ) return "BMP files (*.bmp)|*.bmp";
         if ( pngFormat.Checked ) return "PNG files (*.png)|*.png";
         if ( tiffFormat.Checked ) return "TIFF files (*.tiff)|*.tiff";
         return "JPEG files (*.jpg)|*.jpg";
      }

      Document document = null;
   }
}
