// ---------------------------------------
// Print code sample PDFRasterizer.NET 1.0
//
// Copyright TallComponents BV 2002-2005
// ---------------------------------------

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

using TallComponents.PDF.Rasterizer;
using TallComponents.PDF.Rasterizer.Configuration;

namespace PrintPDF
{
   enum HorizontalAlignment
   {
      Left,
      Center,
      Right
   }

   enum VerticalAlignment
   {
      Top, 
      Center,
      Bottom
   }

	public class MainForm : System.Windows.Forms.Form
	{

      #region Variables

      Document document = null;
      Bitmap pageBitmap = null;
      PrinterSettings printerSettings = null;

      ArrayList paperSizes = new ArrayList();
      PaperSize paperSize;
      RectangleF printableArea;

      int currentPageToPrint;
      int firstPageToPrint;
      int lastPageToPrint;

      ArrayList paperSources = new ArrayList();
      PaperSource paperSource;

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox printerList;
      private System.Windows.Forms.MainMenu mainMenu1;
      private System.Windows.Forms.MenuItem menuItem1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox paperSizeList;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.PictureBox previewBox;
      private System.Windows.Forms.Label documentSizeLabel;
      private System.Windows.Forms.Label scaleLabel;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.RadioButton horAlignLeft;
      private System.Windows.Forms.RadioButton horAlignCenter;
      private System.Windows.Forms.RadioButton horAlignRight;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.RadioButton verAlignTop;
      private System.Windows.Forms.RadioButton verAlignCenter;
      private System.Windows.Forms.RadioButton verAlignBottom;
      private System.Windows.Forms.ComboBox paperSourceList;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label titleLabel;
      private System.Windows.Forms.Label paperSizeLabel;
      private System.Windows.Forms.CheckBox scaleToFit;
      private System.Windows.Forms.TextBox rangeTo;
      private System.Windows.Forms.TextBox rangeFrom;
      private System.Windows.Forms.RadioButton printRange;
      private System.Windows.Forms.RadioButton printAll;
      private System.Windows.Forms.CheckBox autoRotate;
      private System.Windows.Forms.MenuItem openFileMenu;
      private System.Windows.Forms.Button printButton;
      private System.Windows.Forms.Button cancelButton;
      private System.Windows.Forms.CheckBox usePrintableArea;
      private System.ComponentModel.IContainer components;
      private CheckBox landscape;
      private System.Windows.Forms.NumericUpDown previewPageIndex;

      #endregion Variables

      [STAThread]
      static void Main() 
      {
         Application.Run( new MainForm() );
      }

      #region Initialization and cleanup methods

      public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
         InitializeComponent();

			TextRenderSettings.CMapFolder = new DirectoryInfo( @"..\..\..\..\..\Support\CMaps\" ).FullName;
      }

      #endregion Initialization and cleanup methods

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.components = new System.ComponentModel.Container();
         this.printButton = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.printerList = new System.Windows.Forms.ComboBox();
         this.mainMenu1 = new System.Windows.Forms.MainMenu();
         this.menuItem1 = new System.Windows.Forms.MenuItem();
         this.openFileMenu = new System.Windows.Forms.MenuItem();
         this.label2 = new System.Windows.Forms.Label();
         this.paperSizeList = new System.Windows.Forms.ComboBox();
         this.label3 = new System.Windows.Forms.Label();
         this.previewBox = new System.Windows.Forms.PictureBox();
         this.documentSizeLabel = new System.Windows.Forms.Label();
         this.paperSizeLabel = new System.Windows.Forms.Label();
         this.scaleLabel = new System.Windows.Forms.Label();
         this.cancelButton = new System.Windows.Forms.Button();
         this.scaleToFit = new System.Windows.Forms.CheckBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.horAlignRight = new System.Windows.Forms.RadioButton();
         this.horAlignCenter = new System.Windows.Forms.RadioButton();
         this.horAlignLeft = new System.Windows.Forms.RadioButton();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.verAlignBottom = new System.Windows.Forms.RadioButton();
         this.verAlignCenter = new System.Windows.Forms.RadioButton();
         this.verAlignTop = new System.Windows.Forms.RadioButton();
         this.paperSourceList = new System.Windows.Forms.ComboBox();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.rangeTo = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.rangeFrom = new System.Windows.Forms.TextBox();
         this.printRange = new System.Windows.Forms.RadioButton();
         this.printAll = new System.Windows.Forms.RadioButton();
         this.titleLabel = new System.Windows.Forms.Label();
         this.autoRotate = new System.Windows.Forms.CheckBox();
         this.usePrintableArea = new System.Windows.Forms.CheckBox();
         this.previewPageIndex = new System.Windows.Forms.NumericUpDown();
         this.landscape = new System.Windows.Forms.CheckBox();
         ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.groupBox3.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.previewPageIndex)).BeginInit();
         this.SuspendLayout();
         // 
         // printButton
         // 
         this.printButton.Location = new System.Drawing.Point(400, 384);
         this.printButton.Name = "printButton";
         this.printButton.Size = new System.Drawing.Size(75, 23);
         this.printButton.TabIndex = 0;
         this.printButton.Text = "Print";
         this.printButton.Click += new System.EventHandler(this.cmdPrintPdf_Click);
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(16, 16);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(224, 23);
         this.label1.TabIndex = 1;
         this.label1.Text = "Select printer";
         // 
         // printerList
         // 
         this.printerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.printerList.Location = new System.Drawing.Point(16, 32);
         this.printerList.Name = "printerList";
         this.printerList.Size = new System.Drawing.Size(264, 21);
         this.printerList.TabIndex = 2;
         this.printerList.SelectedIndexChanged += new System.EventHandler(this.printerList_SelectedIndexChanged);
         // 
         // mainMenu1
         // 
         this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
         // 
         // menuItem1
         // 
         this.menuItem1.Index = 0;
         this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openFileMenu});
         this.menuItem1.Text = "File";
         // 
         // openFileMenu
         // 
         this.openFileMenu.Index = 0;
         this.openFileMenu.Text = "Open...";
         this.openFileMenu.Click += new System.EventHandler(this.openFileMenu_Click);
         // 
         // label2
         // 
         this.label2.Location = new System.Drawing.Point(16, 64);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(224, 23);
         this.label2.TabIndex = 3;
         this.label2.Text = "Select paper size";
         // 
         // paperSizeList
         // 
         this.paperSizeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.paperSizeList.Location = new System.Drawing.Point(16, 80);
         this.paperSizeList.Name = "paperSizeList";
         this.paperSizeList.Size = new System.Drawing.Size(264, 21);
         this.paperSizeList.TabIndex = 4;
         this.paperSizeList.SelectedIndexChanged += new System.EventHandler(this.paperSizeList_SelectedIndexChanged);
         // 
         // label3
         // 
         this.label3.Location = new System.Drawing.Point(16, 112);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(200, 23);
         this.label3.TabIndex = 5;
         this.label3.Text = "Select paper source";
         // 
         // previewBox
         // 
         this.previewBox.BackColor = System.Drawing.SystemColors.Control;
         this.previewBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.previewBox.Location = new System.Drawing.Point(336, 32);
         this.previewBox.Name = "previewBox";
         this.previewBox.Size = new System.Drawing.Size(224, 248);
         this.previewBox.TabIndex = 7;
         this.previewBox.TabStop = false;
         // 
         // documentSizeLabel
         // 
         this.documentSizeLabel.Location = new System.Drawing.Point(336, 296);
         this.documentSizeLabel.Name = "documentSizeLabel";
         this.documentSizeLabel.Size = new System.Drawing.Size(224, 23);
         this.documentSizeLabel.TabIndex = 8;
         this.documentSizeLabel.Text = "Document:";
         // 
         // paperSizeLabel
         // 
         this.paperSizeLabel.Location = new System.Drawing.Point(336, 320);
         this.paperSizeLabel.Name = "paperSizeLabel";
         this.paperSizeLabel.Size = new System.Drawing.Size(224, 23);
         this.paperSizeLabel.TabIndex = 9;
         this.paperSizeLabel.Text = "<page w x h in>";
         // 
         // scaleLabel
         // 
         this.scaleLabel.Location = new System.Drawing.Point(336, 344);
         this.scaleLabel.Name = "scaleLabel";
         this.scaleLabel.Size = new System.Drawing.Size(224, 23);
         this.scaleLabel.TabIndex = 10;
         this.scaleLabel.Text = "Scale: ... %";
         // 
         // cancelButton
         // 
         this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelButton.Location = new System.Drawing.Point(488, 384);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(75, 23);
         this.cancelButton.TabIndex = 11;
         this.cancelButton.Text = "Cancel";
         this.cancelButton.Click += new System.EventHandler(this.cancel_Click);
         // 
         // scaleToFit
         // 
         this.scaleToFit.Checked = true;
         this.scaleToFit.CheckState = System.Windows.Forms.CheckState.Checked;
         this.scaleToFit.Location = new System.Drawing.Point(16, 160);
         this.scaleToFit.Name = "scaleToFit";
         this.scaleToFit.Size = new System.Drawing.Size(128, 16);
         this.scaleToFit.TabIndex = 12;
         this.scaleToFit.Text = "Scale to fit";
         this.scaleToFit.CheckedChanged += new System.EventHandler(this.LayoutChanged);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.horAlignRight);
         this.groupBox1.Controls.Add(this.horAlignCenter);
         this.groupBox1.Controls.Add(this.horAlignLeft);
         this.groupBox1.Location = new System.Drawing.Point(16, 208);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(264, 56);
         this.groupBox1.TabIndex = 13;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Horizontal alignment";
         // 
         // horAlignRight
         // 
         this.horAlignRight.Location = new System.Drawing.Point(184, 24);
         this.horAlignRight.Name = "horAlignRight";
         this.horAlignRight.Size = new System.Drawing.Size(56, 24);
         this.horAlignRight.TabIndex = 2;
         this.horAlignRight.Text = "Right";
         this.horAlignRight.CheckedChanged += new System.EventHandler(this.AlignmentChanged);
         // 
         // horAlignCenter
         // 
         this.horAlignCenter.Checked = true;
         this.horAlignCenter.Location = new System.Drawing.Point(104, 24);
         this.horAlignCenter.Name = "horAlignCenter";
         this.horAlignCenter.Size = new System.Drawing.Size(56, 24);
         this.horAlignCenter.TabIndex = 1;
         this.horAlignCenter.TabStop = true;
         this.horAlignCenter.Text = "Center";
         this.horAlignCenter.CheckedChanged += new System.EventHandler(this.AlignmentChanged);
         // 
         // horAlignLeft
         // 
         this.horAlignLeft.Location = new System.Drawing.Point(16, 24);
         this.horAlignLeft.Name = "horAlignLeft";
         this.horAlignLeft.Size = new System.Drawing.Size(56, 24);
         this.horAlignLeft.TabIndex = 0;
         this.horAlignLeft.Text = "Left";
         this.horAlignLeft.CheckedChanged += new System.EventHandler(this.AlignmentChanged);
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.verAlignBottom);
         this.groupBox2.Controls.Add(this.verAlignCenter);
         this.groupBox2.Controls.Add(this.verAlignTop);
         this.groupBox2.Location = new System.Drawing.Point(16, 272);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(112, 136);
         this.groupBox2.TabIndex = 14;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Vertical alignment";
         // 
         // verAlignBottom
         // 
         this.verAlignBottom.Location = new System.Drawing.Point(16, 72);
         this.verAlignBottom.Name = "verAlignBottom";
         this.verAlignBottom.Size = new System.Drawing.Size(72, 24);
         this.verAlignBottom.TabIndex = 2;
         this.verAlignBottom.Text = "Bottom";
         this.verAlignBottom.CheckedChanged += new System.EventHandler(this.AlignmentChanged);
         // 
         // verAlignCenter
         // 
         this.verAlignCenter.Checked = true;
         this.verAlignCenter.Location = new System.Drawing.Point(16, 48);
         this.verAlignCenter.Name = "verAlignCenter";
         this.verAlignCenter.Size = new System.Drawing.Size(64, 24);
         this.verAlignCenter.TabIndex = 1;
         this.verAlignCenter.TabStop = true;
         this.verAlignCenter.Text = "Center";
         this.verAlignCenter.CheckedChanged += new System.EventHandler(this.AlignmentChanged);
         // 
         // verAlignTop
         // 
         this.verAlignTop.Location = new System.Drawing.Point(16, 24);
         this.verAlignTop.Name = "verAlignTop";
         this.verAlignTop.Size = new System.Drawing.Size(64, 24);
         this.verAlignTop.TabIndex = 0;
         this.verAlignTop.Text = "Top";
         this.verAlignTop.CheckedChanged += new System.EventHandler(this.AlignmentChanged);
         // 
         // paperSourceList
         // 
         this.paperSourceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.paperSourceList.Location = new System.Drawing.Point(16, 128);
         this.paperSourceList.Name = "paperSourceList";
         this.paperSourceList.Size = new System.Drawing.Size(264, 21);
         this.paperSourceList.TabIndex = 15;
         this.paperSourceList.SelectedIndexChanged += new System.EventHandler(this.paperSourceList_SelectedIndexChanged);
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.rangeTo);
         this.groupBox3.Controls.Add(this.label5);
         this.groupBox3.Controls.Add(this.label4);
         this.groupBox3.Controls.Add(this.rangeFrom);
         this.groupBox3.Controls.Add(this.printRange);
         this.groupBox3.Controls.Add(this.printAll);
         this.groupBox3.Location = new System.Drawing.Point(144, 272);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(136, 136);
         this.groupBox3.TabIndex = 16;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Print selection";
         // 
         // rangeTo
         // 
         this.rangeTo.Location = new System.Drawing.Point(64, 96);
         this.rangeTo.Name = "rangeTo";
         this.rangeTo.Size = new System.Drawing.Size(48, 20);
         this.rangeTo.TabIndex = 5;
         this.rangeTo.TextChanged += new System.EventHandler(this.range_TextChanged);
         // 
         // label5
         // 
         this.label5.Location = new System.Drawing.Point(8, 96);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(48, 23);
         this.label5.TabIndex = 4;
         this.label5.Text = "To:";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label4
         // 
         this.label4.Location = new System.Drawing.Point(8, 72);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(48, 23);
         this.label4.TabIndex = 3;
         this.label4.Text = "From:";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // rangeFrom
         // 
         this.rangeFrom.Location = new System.Drawing.Point(64, 72);
         this.rangeFrom.Name = "rangeFrom";
         this.rangeFrom.Size = new System.Drawing.Size(48, 20);
         this.rangeFrom.TabIndex = 2;
         this.rangeFrom.TextChanged += new System.EventHandler(this.range_TextChanged);
         // 
         // printRange
         // 
         this.printRange.Location = new System.Drawing.Point(16, 48);
         this.printRange.Name = "printRange";
         this.printRange.Size = new System.Drawing.Size(104, 24);
         this.printRange.TabIndex = 1;
         this.printRange.Text = "Range";
         // 
         // printAll
         // 
         this.printAll.Checked = true;
         this.printAll.Location = new System.Drawing.Point(16, 24);
         this.printAll.Name = "printAll";
         this.printAll.Size = new System.Drawing.Size(104, 16);
         this.printAll.TabIndex = 0;
         this.printAll.TabStop = true;
         this.printAll.Text = "All";
         // 
         // titleLabel
         // 
         this.titleLabel.Location = new System.Drawing.Point(336, 16);
         this.titleLabel.Name = "titleLabel";
         this.titleLabel.Size = new System.Drawing.Size(224, 23);
         this.titleLabel.TabIndex = 17;
         this.titleLabel.Text = "No document";
         // 
         // autoRotate
         // 
         this.autoRotate.Checked = true;
         this.autoRotate.CheckState = System.Windows.Forms.CheckState.Checked;
         this.autoRotate.Location = new System.Drawing.Point(144, 160);
         this.autoRotate.Name = "autoRotate";
         this.autoRotate.Size = new System.Drawing.Size(128, 16);
         this.autoRotate.TabIndex = 18;
         this.autoRotate.Text = "Auto-rotate";
         this.autoRotate.CheckedChanged += new System.EventHandler(this.LayoutChanged);
         // 
         // usePrintableArea
         // 
         this.usePrintableArea.Checked = true;
         this.usePrintableArea.CheckState = System.Windows.Forms.CheckState.Checked;
         this.usePrintableArea.Location = new System.Drawing.Point(16, 184);
         this.usePrintableArea.Name = "usePrintableArea";
         this.usePrintableArea.Size = new System.Drawing.Size(128, 16);
         this.usePrintableArea.TabIndex = 19;
         this.usePrintableArea.Text = "Use printable area";
         this.usePrintableArea.CheckedChanged += new System.EventHandler(this.LayoutChanged);
         // 
         // previewPageIndex
         // 
         this.previewPageIndex.Location = new System.Drawing.Point(336, 384);
         this.previewPageIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.previewPageIndex.Name = "previewPageIndex";
         this.previewPageIndex.Size = new System.Drawing.Size(48, 20);
         this.previewPageIndex.TabIndex = 20;
         this.previewPageIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.previewPageIndex.ValueChanged += new System.EventHandler(this.previewPageIndex_ValueChanged);
         // 
         // landscape
         // 
         this.landscape.Location = new System.Drawing.Point(144, 183);
         this.landscape.Name = "landscape";
         this.landscape.Size = new System.Drawing.Size(79, 17);
         this.landscape.TabIndex = 21;
         this.landscape.Text = "Landscape";
         this.landscape.CheckedChanged += new System.EventHandler(this.landscape_CheckedChanged);
         // 
         // MainForm
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(576, 443);
         this.Controls.Add(this.landscape);
         this.Controls.Add(this.previewPageIndex);
         this.Controls.Add(this.usePrintableArea);
         this.Controls.Add(this.autoRotate);
         this.Controls.Add(this.scaleToFit);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.paperSourceList);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.cancelButton);
         this.Controls.Add(this.scaleLabel);
         this.Controls.Add(this.paperSizeLabel);
         this.Controls.Add(this.documentSizeLabel);
         this.Controls.Add(this.previewBox);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.paperSizeList);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.printerList);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.printButton);
         this.Controls.Add(this.titleLabel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.Menu = this.mainMenu1;
         this.Name = "MainForm";
         this.Text = "Print PDF";
         this.Load += new System.EventHandler(this.MainForm_Load);
         ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.groupBox2.ResumeLayout(false);
         this.groupBox3.ResumeLayout(false);
         this.groupBox3.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.previewPageIndex)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }
		#endregion

      #region Properties

      HorizontalAlignment HorizontalAlignment
      {
         get
         {
            if ( this.horAlignLeft.Checked )
               return HorizontalAlignment.Left;
            if ( this.horAlignCenter.Checked )
               return HorizontalAlignment.Center;
            return HorizontalAlignment.Right;
         }
      }

      VerticalAlignment VerticalAlignment
      {
         get
         {
            if ( this.verAlignTop.Checked )
               return VerticalAlignment.Top;
            if ( this.verAlignCenter.Checked )
               return VerticalAlignment.Center;
            return VerticalAlignment.Bottom;
         }
      }

      new float Scale
      {
         get
         {
            if ( null == document ) return 1;

            Page page = document.Pages[PageIndex];
            
            float pdfWidth = (float) page.Width;
            float pdfHeight = (float) page.Height;
            float pdfRatio = pdfWidth / pdfHeight;

            float paperWidth = ( (float) paperSize.Width * 72f ) / 100f;
            float paperHeight = ( (float) paperSize.Height * 72f ) / 100f;

            if (landscape.Checked)
            {
               float temp = paperHeight;
               paperHeight = paperWidth;
               paperWidth = temp;
            }

            float paperRatio = paperWidth / paperHeight;
 
            float scaledPdfWidth = pdfWidth;
            float scaledPdfHeight = pdfHeight;

            if ( this.autoRotate.Checked )
            {
               if ( ( pdfRatio > 1 && paperRatio < 1 ) || ( pdfRatio < 1 && paperRatio > 1 ) )
               {
                  // need to rotate
                  float temp = scaledPdfWidth;
                  scaledPdfWidth = scaledPdfHeight;
                  scaledPdfHeight = temp;
                  pdfRatio = 1 / pdfRatio;
               }
            }

            float scale = 1;
            if ( this.scaleToFit.Checked )
            {
               if ( pdfRatio > paperRatio )
                  scale = paperWidth / scaledPdfWidth;
               else
                  scale = paperHeight / scaledPdfHeight;
            }

            return scale;
         }
      }

      Matrix PreviewTransformation
      {
         get
         {
            Page page = document.Pages[PageIndex];
            
            float pdfWidth = (float) page.Width;
            float pdfHeight = (float) page.Height;
            float pdfRatio = pdfWidth / pdfHeight;

            float paperWidth = ( (float) paperSize.Width * 72f ) / 100f;
            float paperHeight = ( (float) paperSize.Height * 72f ) / 100f;

            if (landscape.Checked)
            {
               float temp = paperHeight;
               paperHeight = paperWidth;
               paperWidth = temp;
            }

            float paperRatio = paperWidth / paperHeight;
 
            float scaledPdfWidth = pdfWidth;
            float scaledPdfHeight = pdfHeight;

            if ( this.autoRotate.Checked )
            {
               if ( ( pdfRatio > 1 && paperRatio < 1 ) || ( pdfRatio < 1 && paperRatio > 1 ) )
               {
                  // need to rotate
                  float temp = scaledPdfWidth;
                  scaledPdfWidth = scaledPdfHeight;
                  scaledPdfHeight = temp;
                  pdfRatio = 1 / pdfRatio;
               }
            }

            float scale = 1;
            if ( this.scaleToFit.Checked )
            {
               if ( pdfRatio > paperRatio )
                  scale = paperWidth / scaledPdfWidth;
               else
                  scale = paperHeight / scaledPdfHeight;
            }

            scaledPdfWidth *= scale;
            scaledPdfHeight *= scale;

            float combinedWidth = Math.Max( scaledPdfWidth, paperWidth );
            float combinedHeight = Math.Max( scaledPdfHeight, paperHeight );
            float combinedRatio = combinedWidth / combinedHeight;

            float previewBoxRation = (float) previewBox.Width / (float) previewBox.Height;
            float previewScale;

            if ( combinedRatio > previewBoxRation )
               previewScale = (float) ( previewBox.Width - 15 ) / combinedWidth;
            else
               previewScale = (float) ( previewBox.Height - 15 ) / combinedHeight;

            Matrix previewTransformation = new Matrix();
            
            previewTransformation.Scale( previewScale, previewScale );
            previewTransformation.Translate( 
               ( previewBox.Width/previewScale - paperWidth ) / 2,
               ( previewBox.Height/previewScale - paperHeight ) / 2 );

            return previewTransformation;
         }
      }

      /// <summary>
      /// Compute the proper transformation for printing a page.
      /// </summary>
      /// <param name="page">The page that needs to be printed.</param>
      /// <param name="printableArea">The printable area of the printer.</param>
      /// <returns>The page transformation.</returns>
      /// <remarks>The printable area is the area after the landscape flag has been
      /// taken into account. For example, if letter is specified, and landscape has been set,
      /// printableArea.Width > printableArea.Height.
      /// </remarks>
      Matrix GetPrintTransformation(Page page, RectangleF printableArea)
      {
         Matrix transformation = new Matrix();

         float pdfWidth = (float)page.Width;
         float pdfHeight = (float)page.Height;
         float pdfRatio = pdfWidth / pdfHeight;

         float paperWidth = ((float)paperSize.Width * 72f) / 100f;
         float paperHeight = ((float)paperSize.Height * 72f) / 100f;

         if (landscape.Checked)
         {
            float temp = paperHeight;
            paperHeight = paperWidth;
            paperWidth = temp;
         }

         // It is possible that a driver returns bogus data for the printable area. If that is
         // obviously the case, we are going to ignore the usePrintableArea settings (otherwise
         // we get invalid transforms).

         if (usePrintableArea.Checked && printableArea.Width > 0 && printableArea.Height > 0)
         {
            paperWidth = printableArea.Width;
            paperHeight = printableArea.Height;
            transformation.Translate(printableArea.Left, printableArea.Top);
         }

         float paperRatio = paperWidth / paperHeight;

         float scaledPdfWidth = pdfWidth;
         float scaledPdfHeight = pdfHeight;

         bool rotate = false;
         if (this.autoRotate.Checked)
         {
            if ((pdfRatio > 1 && paperRatio < 1) || (pdfRatio < 1 && paperRatio > 1))
            {
               // need to rotate
               rotate = true;
               float temp = scaledPdfWidth;
               scaledPdfWidth = scaledPdfHeight;
               scaledPdfHeight = temp;
               pdfRatio = 1 / pdfRatio;
            }
         }

         float scale = 1;
         if (this.scaleToFit.Checked)
         {
            if (pdfRatio > paperRatio)
               scale = paperWidth / scaledPdfWidth;
            else
               scale = paperHeight / scaledPdfHeight;
         }

         scaledPdfWidth *= scale;
         scaledPdfHeight *= scale;

         float dx;
         switch (HorizontalAlignment)
         {
            case HorizontalAlignment.Left: dx = 0;
               break;
            case HorizontalAlignment.Center: dx = (paperWidth - scaledPdfWidth) / 2;
               break;
            case HorizontalAlignment.Right: dx = paperWidth - scaledPdfWidth;
               break;
            default: throw new ArgumentException("Horizontal alignment not set.");
         }

         float dy;
         switch (VerticalAlignment)
         {
            case VerticalAlignment.Top: dy = 0;
               break;
            case VerticalAlignment.Center: dy = (paperHeight - scaledPdfHeight) / 2;
               break;
            case VerticalAlignment.Bottom: dy = paperHeight - scaledPdfHeight;
               break;
            default: throw new ArgumentException("Vertical alignment not set.");
         }

         // align
         transformation.Translate(dx, dy);

         // auto-rotate
         if (rotate)
         {
            transformation.Rotate(-90);
            transformation.Translate(-scaledPdfHeight, 0);
         }

         // scale
         transformation.Scale(scale, scale);

         return transformation;
      }

      int PageIndex
      { 
         get 
         { 
            return ( int ) ( previewPageIndex.Value - 1 );
         }
         set
         {
            previewPageIndex.Value = value + 1;
         }
      }

      #endregion Properties

      #region Print methods

      void print()
      {
         currentPageToPrint = firstPageToPrint;

         if (currentPageToPrint < document.Pages.Count)
         {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings = this.printerSettings;
            printDocument.DocumentName = document.DocumentInfo.Title;
            printDocument.DefaultPageSettings.PaperSize = this.paperSize;
            printDocument.DefaultPageSettings.PaperSource = this.paperSource;
            printDocument.DefaultPageSettings.Margins.Left = 0;
            printDocument.DefaultPageSettings.Margins.Right = 0;
            printDocument.DefaultPageSettings.Margins.Bottom = 0;
            printDocument.DefaultPageSettings.Margins.Top = 0;
            printDocument.OriginAtMargins = true;
            
            // uncomment to avoid status dialog
            // printDocument.PrintController = new StandardPrintController();

            printDocument.PrintPage += new PrintPageEventHandler( printPage );
            printDocument.Print();
         }
		}

      private void printPage(object sender, PrintPageEventArgs e)
      {
         e.HasMorePages = false;
         if ( currentPageToPrint <= lastPageToPrint )
         {
            e.Graphics.PageUnit = GraphicsUnit.Point;
            Page page = document.Pages[ currentPageToPrint ];

            e.Graphics.MultiplyTransform( GetPrintTransformation(page, e.Graphics.VisibleClipBounds) );
            page.Draw( e.Graphics );
            if ( currentPageToPrint < lastPageToPrint )
            {
               currentPageToPrint++;
               e.HasMorePages = true;
            }
         }
      }

      #endregion Print methods

      #region Fill String Lists

      void populatePrinterList()
      {
         printerList.Items.Clear();

         foreach ( string installedPrinter in PrinterSettings.InstalledPrinters )
            printerList.Items.Add( installedPrinter );
         printerList.SelectedIndex = printerList.Items.IndexOf( ( new PrinterSettings() ).PrinterName );
      }

      void populatePaperSizeList()
      {
         string format = "{0} ({1:f2} x {2:f2} in)", s;
         paperSizes.Clear();
         paperSizeList.Items.Clear();

         foreach ( PaperSize paperSize in printerSettings.PaperSizes )
         {
            paperSizes.Add( paperSize );
            s = string.Format( format, paperSize.PaperName, (float) paperSize.Width / 100f, (float) paperSize.Height / 100f );
            paperSizeList.Items.Add( s );
         }
         PaperSize ps = printerSettings.DefaultPageSettings.PaperSize;
         s = string.Format( format, ps.PaperName, (float) ps.Width / 100f, (float) ps.Height / 100f );
         paperSizeList.SelectedIndex = paperSizeList.Items.IndexOf( s );
      }

      void populatePaperSourceList()
      {
         string format = "{0} ({1})", s;
         paperSources.Clear();
         paperSourceList.Items.Clear();

         foreach ( PaperSource paperSource in printerSettings.PaperSources )
         {
            paperSources.Add( paperSource );
            s = string.Format( format, paperSource.SourceName, paperSource.Kind );
            paperSourceList.Items.Add( s );
         }
         PaperSource ps = printerSettings.DefaultPageSettings.PaperSource;
         s = string.Format( format, ps.SourceName, ps.Kind );
         paperSourceList.SelectedIndex = paperSourceList.Items.IndexOf( s );
      }

      #endregion Fill String Lists

      #region Update dialog items

      void updateTitleLabel( string fileName )
      {
         FileInfo file = new FileInfo( fileName );
         titleLabel.Text = string.Format( "{0}, {1} pages", file.Name, document.Pages.Count );
      }

      void updateDocumentSizeLabel()
      {
         if ( null != document )
         {
            Page page = document.Pages[PageIndex];
            documentSizeLabel.Text = string.Format( "Document: {0:f2} x {1:f2} in", 
               (float) page.Width / 72f, 
               (float) page.Height / 72f ); 
         }
         else documentSizeLabel.Text = "Document: ";
      }

      void updatePaperSizeLabel()
      {
         paperSizeLabel.Text = string.Format( "Paper: {0:f2} x {1:f2} in", 
            (float) paperSize.Width / 100f, 
            (float) paperSize.Height / 100f ); 
      }

      void updateScaleLabel()
      {
         scaleLabel.Text = string.Format( "Scale: {0}%", (int) ( Scale * 100 ) );
      }

      void updatePreviewBox()
      {
         if ( null != document )
         {
            Bitmap previewBitmap = new Bitmap( previewBox.Width, previewBox.Height );
            using ( Graphics previewGraphics = Graphics.FromImage( previewBitmap ) )
            {
               previewGraphics.Clear( Color.LightGray );

               // ---------------------
               // draw paper boundaries
               // ---------------------
               previewGraphics.MultiplyTransform( PreviewTransformation );

               float paperWidth = ( (float) paperSize.Width * 72f ) / 100f;
               float paperHeight = ( (float) paperSize.Height * 72f ) / 100f;

               if (landscape.Checked)
               {
                  float temp = paperHeight;
                  paperHeight = paperWidth;
                  paperWidth = temp;
               }
               
               using ( Pen darkGrayPen = new Pen( Color.DarkGray ) )
               using ( Brush whiteBrush = new SolidBrush( Color.White ) )
               {
                  previewGraphics.FillRectangle( 
                     whiteBrush, 
                     0, 0,
                     paperWidth, paperHeight );
                  previewGraphics.DrawRectangle( 
                     darkGrayPen, 
                     0, 0,
                     paperWidth, paperHeight );
               }
                  
               // -------------
               // draw PDF page
               // -------------                  
               Page page = document.Pages[PageIndex];

               previewGraphics.MultiplyTransform( GetPrintTransformation(page, printableArea) );

               float pdfWidth = (float) page.Width;
               float pdfHeight = (float) page.Height;
               
               previewGraphics.InterpolationMode = InterpolationMode.High;
               previewGraphics.DrawImage( 
                  pageBitmap, 
                  0, 0,
                  pdfWidth, pdfHeight );
            
               // draw PDF page boundaries
               using ( Pen redPen = new Pen( Color.Red ) )
               {
                  previewGraphics.DrawRectangle( 
                     redPen, 
                     0, 0,
                     pdfWidth, pdfHeight );
               }

               previewBox.Image = previewBitmap;
            }
         }
         else
            previewBox.Image = null;
      }

      void updatePrintableArea()
      {
         using ( Graphics graphics = printerSettings.CreateMeasurementGraphics() )
         {
            float paperWidth = ( paperSize.Width * 72f ) / 100f;
            float paperHeight = ( paperSize.Height * 72f ) / 100f;

            if (landscape.Checked)
            {
               float temp = paperHeight;
               paperHeight = paperWidth;
               paperWidth = temp;
            }

            float printableWidth = ( graphics.VisibleClipBounds.Width * 72f ) / 100f;
            float printableHeight = ( graphics.VisibleClipBounds.Height * 72f ) / 100f;

            printableArea = new RectangleF( 0, 0, printableWidth, printableHeight );
            float dx = ( paperWidth - printableWidth ) / 2;
            float dy = ( paperHeight - printableHeight ) / 2;
            printableArea.Offset( dx, dy ); 
         }
      }

      #endregion Update dialog items

      #region Events

      // Load the printer list as soon as the mainform is created
      private void MainForm_Load(object sender, System.EventArgs e)
      {
         populatePrinterList();
      }

      // Ask the user to select a file and open it.
      private void openFileMenu_Click(object sender, System.EventArgs e)
      {
         OpenFileDialog dialog = new OpenFileDialog();
         dialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
         if ( DialogResult.OK == dialog.ShowDialog() )
         {
            using ( FileStream file = new FileStream( dialog.FileName, FileMode.Open, FileAccess.Read ) )
            {
               document = new Document( file );

               previewPageIndex.Maximum = document.Pages.Count;
               if ( 1 != previewPageIndex.Value )
                  previewPageIndex.Value = 1;
               else
                  previewPageIndex_ValueChanged(sender, e);

               updatePreviewBox();
               updateTitleLabel( dialog.FileName );
               updateDocumentSizeLabel();
               updateScaleLabel();
               printerSettings.MaximumPage = document.Pages.Count;
            }
         }
      }

      // Called whenever another printer is selected
      private void printerList_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         printerSettings = new PrinterSettings();
         printerSettings.PrinterName = printerList.Items[ printerList.SelectedIndex ] as string;
         if (null != document)
         {
            printerSettings.MaximumPage = document.Pages.Count;
         }

         printerSettings.DefaultPageSettings.Landscape = landscape.Checked;

         populatePaperSizeList();
         populatePaperSourceList();
      }

      private void cmdPrintPdf_Click( object sender, System.EventArgs e )
      {
         if ( null == document )
         {
            MessageBox.Show( "You must first open a PDF document." );
            return;
         }

         if (printRange.Checked)
         {
            firstPageToPrint = int.Parse(rangeFrom.Text) - 1;
            lastPageToPrint = int.Parse(rangeTo.Text) - 1;
         }
         else
         {
            firstPageToPrint = 0;
            lastPageToPrint = document.Pages.Count - 1;
         }

         PageIndex = firstPageToPrint;

         // print asynchronously
         System.Threading.ThreadStart threadStart = new System.Threading.ThreadStart( print );
         threadStart.BeginInvoke( null, null );
      }

      private void cancel_Click(object sender, System.EventArgs e)
      {
         this.Close();
      }

      private void paperSizeList_SelectedIndexChanged(object sender, System.EventArgs e)
      {
         paperSize = paperSizes[ paperSizeList.SelectedIndex ] as PaperSize;
         
         updatePrintableArea();

         updatePaperSizeLabel();
         updatePreviewBox();
         updateScaleLabel();
      }

      private void paperSourceList_SelectedIndexChanged(object sender, System.EventArgs e)
      {
          paperSource = paperSources[ paperSourceList.SelectedIndex ] as PaperSource;
      }

      private void LayoutChanged(object sender, System.EventArgs e)
      {
         updatePreviewBox();
         updateScaleLabel();
      }

      private void AlignmentChanged(object sender, System.EventArgs e)
      {
         updatePreviewBox();
      }

      private void range_TextChanged(object sender, System.EventArgs e)
      {
         printRange.Checked = true;
      }

      private void previewPageIndex_ValueChanged(object sender, System.EventArgs e)
      {
         if ( null != document )
         {
            Page page = document.Pages[PageIndex];

            if ( null != pageBitmap )
               pageBitmap.Dispose();

            pageBitmap = new Bitmap( (int) page.Width, (int) page.Height );
            using ( Graphics graphics = Graphics.FromImage( pageBitmap ) )
            {
               page.Draw( graphics );
            }

            updatePreviewBox();
         }
      }

      private void landscape_CheckedChanged(object sender, EventArgs e)
      {
         printerSettings.DefaultPageSettings.Landscape = landscape.Checked;

         updatePrintableArea();
         updatePreviewBox();
         updateScaleLabel();
      }

      #endregion Events

   }
}
