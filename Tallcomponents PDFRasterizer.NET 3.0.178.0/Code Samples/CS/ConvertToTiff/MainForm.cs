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
      private System.Windows.Forms.RadioButton radioButton3;
      private System.Windows.Forms.Button selectPdfDocument;
      private System.Windows.Forms.GroupBox compressionGroup;
      private System.Windows.Forms.RadioButton compressionCcittG4;
      private System.Windows.Forms.RadioButton compressionCcittG3;
      private System.Windows.Forms.RadioButton compressionNone;
      private System.Windows.Forms.RadioButton compressionLzw;
      private System.Windows.Forms.GroupBox dpiGroup;
      private System.Windows.Forms.RadioButton dpi300;
      private System.Windows.Forms.RadioButton dpi150;
      private System.Windows.Forms.RadioButton dpi72;
      private System.ComponentModel.IContainer components;
      private RadioButton compressionPackBits;
      private RadioButton dpi600;
      private RadioButton dpi96;
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
         this.components = new System.ComponentModel.Container();
         this.mainMenu1 = new System.Windows.Forms.MainMenu();
         this.selectPdfDocument = new System.Windows.Forms.Button();
         this.compressionGroup = new System.Windows.Forms.GroupBox();
         this.compressionCcittG4 = new System.Windows.Forms.RadioButton();
         this.radioButton3 = new System.Windows.Forms.RadioButton();
         this.compressionCcittG3 = new System.Windows.Forms.RadioButton();
         this.compressionNone = new System.Windows.Forms.RadioButton();
         this.compressionLzw = new System.Windows.Forms.RadioButton();
         this.dpiGroup = new System.Windows.Forms.GroupBox();
         this.dpi300 = new System.Windows.Forms.RadioButton();
         this.dpi150 = new System.Windows.Forms.RadioButton();
         this.dpi72 = new System.Windows.Forms.RadioButton();
         this.saveAsImage = new System.Windows.Forms.Button();
         this.compressionPackBits = new System.Windows.Forms.RadioButton();
         this.dpi600 = new System.Windows.Forms.RadioButton();
         this.dpi96 = new System.Windows.Forms.RadioButton();
         this.compressionGroup.SuspendLayout();
         this.dpiGroup.SuspendLayout();
         this.SuspendLayout();
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
         // compressionGroup
         // 
         this.compressionGroup.Controls.Add(this.compressionPackBits);
         this.compressionGroup.Controls.Add(this.compressionCcittG4);
         this.compressionGroup.Controls.Add(this.radioButton3);
         this.compressionGroup.Controls.Add(this.compressionCcittG3);
         this.compressionGroup.Controls.Add(this.compressionNone);
         this.compressionGroup.Controls.Add(this.compressionLzw);
         this.compressionGroup.Enabled = false;
         this.compressionGroup.Location = new System.Drawing.Point(8, 52);
         this.compressionGroup.Name = "compressionGroup";
         this.compressionGroup.Size = new System.Drawing.Size(128, 155);
         this.compressionGroup.TabIndex = 3;
         this.compressionGroup.TabStop = false;
         this.compressionGroup.Text = "Image Compression";
         // 
         // compressionCcittG4
         // 
         this.compressionCcittG4.Location = new System.Drawing.Point(16, 72);
         this.compressionCcittG4.Name = "compressionCcittG4";
         this.compressionCcittG4.Size = new System.Drawing.Size(104, 24);
         this.compressionCcittG4.TabIndex = 7;
         this.compressionCcittG4.Text = "CCITT G4";
         // 
         // radioButton3
         // 
         this.radioButton3.Location = new System.Drawing.Point(16, 72);
         this.radioButton3.Name = "radioButton3";
         this.radioButton3.Size = new System.Drawing.Size(104, 24);
         this.radioButton3.TabIndex = 6;
         this.radioButton3.Text = "radioButton3";
         // 
         // compressionCcittG3
         // 
         this.compressionCcittG3.Location = new System.Drawing.Point(16, 48);
         this.compressionCcittG3.Name = "compressionCcittG3";
         this.compressionCcittG3.Size = new System.Drawing.Size(104, 24);
         this.compressionCcittG3.TabIndex = 5;
         this.compressionCcittG3.Text = "CCITT G3";
         // 
         // compressionNone
         // 
         this.compressionNone.Checked = true;
         this.compressionNone.Location = new System.Drawing.Point(16, 24);
         this.compressionNone.Name = "compressionNone";
         this.compressionNone.Size = new System.Drawing.Size(104, 24);
         this.compressionNone.TabIndex = 4;
         this.compressionNone.TabStop = true;
         this.compressionNone.Text = "None";
         // 
         // compressionLzw
         // 
         this.compressionLzw.Location = new System.Drawing.Point(16, 96);
         this.compressionLzw.Name = "compressionLzw";
         this.compressionLzw.Size = new System.Drawing.Size(104, 24);
         this.compressionLzw.TabIndex = 8;
         this.compressionLzw.Text = "LZW";
         // 
         // dpiGroup
         // 
         this.dpiGroup.Controls.Add(this.dpi96);
         this.dpiGroup.Controls.Add(this.dpi600);
         this.dpiGroup.Controls.Add(this.dpi300);
         this.dpiGroup.Controls.Add(this.dpi150);
         this.dpiGroup.Controls.Add(this.dpi72);
         this.dpiGroup.Enabled = false;
         this.dpiGroup.Location = new System.Drawing.Point(152, 52);
         this.dpiGroup.Name = "dpiGroup";
         this.dpiGroup.Size = new System.Drawing.Size(128, 155);
         this.dpiGroup.TabIndex = 4;
         this.dpiGroup.TabStop = false;
         this.dpiGroup.Text = "Output resolution";
         // 
         // dpi300
         // 
         this.dpi300.Location = new System.Drawing.Point(16, 93);
         this.dpi300.Name = "dpi300";
         this.dpi300.Size = new System.Drawing.Size(96, 24);
         this.dpi300.TabIndex = 2;
         this.dpi300.Text = "300 DPI";
         // 
         // dpi150
         // 
         this.dpi150.Location = new System.Drawing.Point(16, 69);
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
         this.dpi72.Text = "72 DPI";
         // 
         // saveAsImage
         // 
         this.saveAsImage.Enabled = false;
         this.saveAsImage.Location = new System.Drawing.Point(8, 213);
         this.saveAsImage.Name = "saveAsImage";
         this.saveAsImage.Size = new System.Drawing.Size(272, 23);
         this.saveAsImage.TabIndex = 5;
         this.saveAsImage.Text = "Save as image...";
         this.saveAsImage.Click += new System.EventHandler(this.saveAsImage_Click);
         // 
         // compressionPackBits
         // 
         this.compressionPackBits.Location = new System.Drawing.Point(16, 120);
         this.compressionPackBits.Name = "compressionPackBits";
         this.compressionPackBits.Size = new System.Drawing.Size(104, 24);
         this.compressionPackBits.TabIndex = 9;
         this.compressionPackBits.Text = "PackBits";
         // 
         // dpi600
         // 
         this.dpi600.Location = new System.Drawing.Point(16, 117);
         this.dpi600.Name = "dpi600";
         this.dpi600.Size = new System.Drawing.Size(96, 24);
         this.dpi600.TabIndex = 3;
         this.dpi600.Text = "600 DPI";
         // 
         // dpi96
         // 
         this.dpi96.Location = new System.Drawing.Point(16, 47);
         this.dpi96.Name = "dpi96";
         this.dpi96.Size = new System.Drawing.Size(96, 24);
         this.dpi96.TabIndex = 4;
         this.dpi96.Text = "96 DPI";
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(290, 246);
         this.Controls.Add(this.saveAsImage);
         this.Controls.Add(this.dpiGroup);
         this.Controls.Add(this.compressionGroup);
         this.Controls.Add(this.selectPdfDocument);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.Menu = this.mainMenu1;
         this.Name = "MainForm";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = "Save PDF as image";
         this.compressionGroup.ResumeLayout(false);
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
               document = new Document( file );
            }

            compressionGroup.Enabled = true;
            dpiGroup.Enabled = true;
            saveAsImage.Enabled = true;
         }
      }

      private void saveAsImage_Click(object sender, System.EventArgs e)
      {
         SaveFileDialog dialog = new SaveFileDialog();
         dialog.Filter = "TIFF files (*.tiff)|*.tiff";
         if ( DialogResult.OK == dialog.ShowDialog() )
         {
            ConvertToTiffOptions tiffOptions = new ConvertToTiffOptions( getDPI(), getCompression() );

            using (FileStream file = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
            {
               document.ConvertToTiff(file, tiffOptions);
            }

            MessageBox.Show(this, "Done!", "TIFF Created");
         }
      }

      int getDPI()
      {
         if (dpi72.Checked) return 72;
         if (dpi96.Checked) return 96;
         if (dpi150.Checked) return 150;
         if (dpi300.Checked) return 300;
         if (dpi600.Checked) return 600;
         throw new Exception("Unsupported DPI selected.");
      }

      TiffCompression getCompression()
      {
         if (compressionNone.Checked) return TiffCompression.None;
         if (compressionCcittG3.Checked) return TiffCompression.CcittG3;
         if (compressionCcittG4.Checked) return TiffCompression.CcittG4;
         if (compressionLzw.Checked) return TiffCompression.Lzw;
         if (compressionPackBits.Checked) return TiffCompression.PackBits;
         throw new Exception("Unsupported Compression selected.");
      }

      Document document = null;
   }
}
