using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Fonts;
using TallComponents.PDF.Rasterizer.Configuration;

namespace FontMapping
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
      #region Variables

      private Document document = null;
      private Bitmap bmp1 = null;
      private Bitmap bmp2 = null;
      private RenderSettings renderSettings1 = null;
      private RenderSettings renderSettings2 = null;
      private System.Windows.Forms.Panel view1;
      private System.Windows.Forms.Panel view2;
      private System.Windows.Forms.Splitter splitter;

      #endregion Variables

      #region Constructor and destructor

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			TextRenderSettings.CMapFolder = new DirectoryInfo( @"..\..\..\..\..\Support\CMaps\" ).FullName;

         using ( FileStream file = new FileStream( @"..\..\PDF\FontDemo.PDF", FileMode.Open, FileAccess.Read ) )
         {
            document = new Document( file );
         }

			AddFontsToMapping();

			RepaintPage();
      }

      #endregion Constructor and destructor

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.view1 = new System.Windows.Forms.Panel();
         this.splitter = new System.Windows.Forms.Splitter();
         this.view2 = new System.Windows.Forms.Panel();
         this.SuspendLayout();
         // 
         // view1
         // 
         this.view1.Dock = System.Windows.Forms.DockStyle.Top;
         this.view1.Location = new System.Drawing.Point( 0, 0 );
         this.view1.Name = "view1";
         this.view1.Size = new System.Drawing.Size( 712, 257 );
         this.view1.TabIndex = 2;
         this.view1.Paint += new System.Windows.Forms.PaintEventHandler( this.view1_Paint );
         // 
         // splitter
         // 
         this.splitter.BackColor = System.Drawing.Color.Black;
         this.splitter.Dock = System.Windows.Forms.DockStyle.Top;
         this.splitter.Location = new System.Drawing.Point( 0, 257 );
         this.splitter.Name = "splitter";
         this.splitter.Size = new System.Drawing.Size( 712, 4 );
         this.splitter.TabIndex = 3;
         this.splitter.TabStop = false;
         this.splitter.Move += new System.EventHandler( this.splitter_Move );
         // 
         // view2
         // 
         this.view2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.view2.Location = new System.Drawing.Point( 0, 261 );
         this.view2.Name = "view2";
         this.view2.Size = new System.Drawing.Size( 712, 257 );
         this.view2.TabIndex = 4;
         this.view2.Paint += new System.Windows.Forms.PaintEventHandler( this.view2_Paint );
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
         this.ClientSize = new System.Drawing.Size( 712, 518 );
         this.Controls.Add( this.view2 );
         this.Controls.Add( this.splitter );
         this.Controls.Add( this.view1 );
         this.Name = "MainForm";
         this.Text = "OS Font usage example";
         this.SizeChanged += new System.EventHandler( this.MainForm_SizeChanged );
         this.ResumeLayout( false );

      }
		#endregion

      #region Painting

      private void RepaintPage()
      {
         if ( null != bmp1 )
            bmp1.Dispose();
         if ( null != bmp2 )
            bmp2.Dispose();

         Page page = document.Pages[0];
         // Get a bitmap 4 times the size of the page. This is 288 DPI, almost 300
         bmp1 = new Bitmap( (int) page.Width * 4, (int) page.Height * 4 );
         using ( Graphics graphics = Graphics.FromImage( bmp1 ) )
         {
            graphics.ScaleTransform( 4.0f, 4.0f );
            page.Draw( graphics, renderSettings1  );
         }
         bmp2 = new Bitmap( (int) page.Width * 4, (int) page.Height * 4 );
         using ( Graphics graphics = Graphics.FromImage( bmp2 ) )
         {
            graphics.ScaleTransform( 4.0f, 4.0f );
            page.Draw( graphics, renderSettings2 );
         }
         view1.Invalidate();
         view2.Invalidate();
      }

      private void PaintPanel( Bitmap bmp, Panel view, Graphics graphics )
      {
         graphics.Clear( Color.White );
         if ( null != bmp )
         {
            float ratio = (float) bmp.Width / (float) bmp.Height;
            float displayWidth, displayHeight;
            if ( ratio * view.Height > view.Width )
            {
               displayWidth = view.Width - 10;
               displayHeight = displayWidth / ratio;
            }
            else
            {
               displayHeight = view.Height - 10;
               displayWidth = displayHeight * ratio;
            }

            float x = ( view.Width - displayWidth ) / 2;
            float y = ( view.Height - displayHeight ) / 2;

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.DrawImage( bmp, x, y, displayWidth, displayHeight );
            graphics.DrawRectangle( new Pen( Color.Gray ), x, y, displayWidth, displayHeight );
         }
      }

      #endregion Painting

		#region Events

      private void MainForm_SizeChanged(object sender, System.EventArgs e)
      {
         view1.Height = ( ClientSize.Height - splitter.Height ) >> 1;
         view1.Invalidate();
         view2.Invalidate();
      }

      private void view1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
      {
         PaintPanel( bmp1, view1, e.Graphics );
      }

      private void view2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
      {
         PaintPanel( bmp2, view2, e.Graphics );
      }

      private void splitter_Move(object sender, System.EventArgs e)
      {
         view1.Invalidate();
         view2.Invalidate();
      }

      #endregion Events

      private void AddFontsToMapping()
      {
         // This document (FontDemo.pdf) has two fonts. You can verify this by loading it into
         // Acrobat Reader and pressing <CTRL-D>. If you select the Fonts tab, you will see all
         // the fonts used in this document.
         // The Actual Font entry tells you what font Adobe uses to draw the text.
         // With PDFRasterizer.NET, you specify the font substitution as below.
         // There are two mappings, because we have two views, just to be able to see the difference.
         // The program renders the same PDF twice, but with different Font substitutions.
         // Have a look at the method RepaintPage() to see how these settings are passed to the Page
			// If you use this when printing documents, the data file send to the printer will become much smaller.

         renderSettings1 = new RenderSettings();
         renderSettings1.TextSettings.ResolveFont += new ResolveFontEventHandler(TextSettings1_ResolveFont);

			// Here we get all the fonts in the document and set the same substitute font.
         renderSettings2 = new RenderSettings();
         renderSettings2.TextSettings.ResolveFont += new ResolveFontEventHandler(TextSettings2_ResolveFont);
		}

      void TextSettings1_ResolveFont(TextRenderSettings sender, ResolveFontEventArgs args)
      {
         // Map all non-system fonts to the system font arial

         if (args.FontLocation != FontLocation.System)
         {
            args.FontRenderMode = FontRenderMode.RenderAsFont;

            if (args.PdfFontName.EndsWith("Bold"))
            {
               args.Bold = true;
            }

            args.SystemFontName = "Arial";
         }
      }

      void TextSettings2_ResolveFont(TextRenderSettings sender, ResolveFontEventArgs args)
      {
         // Map all non-system fonts to the system font Arial narrow.

         if (args.FontLocation != FontLocation.System)
         {
            args.FontRenderMode = FontRenderMode.RenderAsFont;
            args.SystemFontName = "Arial Narrow";
         }
      }

      [STAThread]
      static void Main() 
      {
         Application.Run(new MainForm());
      }

   }
}
