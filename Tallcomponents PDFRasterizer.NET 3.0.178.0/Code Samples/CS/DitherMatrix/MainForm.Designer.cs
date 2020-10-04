namespace DitherMatrix_vs2010
{
   partial class MainForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.selectPDF = new System.Windows.Forms.Button();
         this.pictureBox = new System.Windows.Forms.PictureBox();
         this.selectedPage = new System.Windows.Forms.ComboBox();
         this.selectedMatrix = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.convertButton = new System.Windows.Forms.Button();
         this.matrixView1 = new DitherMatrix_vs2010.MatrixView();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
         this.SuspendLayout();
         // 
         // selectPDF
         // 
         this.selectPDF.Location = new System.Drawing.Point(12, 13);
         this.selectPDF.Name = "selectPDF";
         this.selectPDF.Size = new System.Drawing.Size(146, 23);
         this.selectPDF.TabIndex = 0;
         this.selectPDF.Text = "Select PDF document...";
         this.selectPDF.UseVisualStyleBackColor = true;
         this.selectPDF.Click += new System.EventHandler(this.OnSelectPDFDocumentClick);
         // 
         // pictureBox
         // 
         this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.pictureBox.BackColor = System.Drawing.Color.White;
         this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.pictureBox.Location = new System.Drawing.Point(166, 13);
         this.pictureBox.Name = "pictureBox";
         this.pictureBox.Size = new System.Drawing.Size(297, 297);
         this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pictureBox.TabIndex = 1;
         this.pictureBox.TabStop = false;
         // 
         // selectedPage
         // 
         this.selectedPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.selectedPage.Enabled = false;
         this.selectedPage.FormattingEnabled = true;
         this.selectedPage.Location = new System.Drawing.Point(12, 60);
         this.selectedPage.Name = "selectedPage";
         this.selectedPage.Size = new System.Drawing.Size(146, 21);
         this.selectedPage.TabIndex = 2;
         // 
         // selectedMatrix
         // 
         this.selectedMatrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.selectedMatrix.Enabled = false;
         this.selectedMatrix.FormattingEnabled = true;
         this.selectedMatrix.Location = new System.Drawing.Point(12, 110);
         this.selectedMatrix.Name = "selectedMatrix";
         this.selectedMatrix.Size = new System.Drawing.Size(146, 21);
         this.selectedMatrix.TabIndex = 3;
         this.selectedMatrix.SelectedIndexChanged += new System.EventHandler(this.selectedMatrix_SelectedIndexChanged);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(9, 44);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(64, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "Select page";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(9, 94);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(67, 13);
         this.label2.TabIndex = 4;
         this.label2.Text = "Select matrix";
         // 
         // convertButton
         // 
         this.convertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.convertButton.Enabled = false;
         this.convertButton.Location = new System.Drawing.Point(12, 287);
         this.convertButton.Name = "convertButton";
         this.convertButton.Size = new System.Drawing.Size(146, 23);
         this.convertButton.TabIndex = 5;
         this.convertButton.Text = "Convert to TIFF";
         this.convertButton.UseVisualStyleBackColor = true;
         this.convertButton.Click += new System.EventHandler(this.OnConvertClick);
         // 
         // matrixView1
         // 
         this.matrixView1.Location = new System.Drawing.Point(13, 138);
         this.matrixView1.Matrix = new int[][] {
        new int[] {
                0,
                0},
        new int[] {
                0,
                0}};
         this.matrixView1.Name = "matrixView1";
         this.matrixView1.Size = new System.Drawing.Size(118, 74);
         this.matrixView1.TabIndex = 6;
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(475, 322);
         this.Controls.Add(this.matrixView1);
         this.Controls.Add(this.convertButton);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.selectedMatrix);
         this.Controls.Add(this.selectedPage);
         this.Controls.Add(this.pictureBox);
         this.Controls.Add(this.selectPDF);
         this.Name = "MainForm";
         this.Text = "Dither matrix sample";
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button selectPDF;
      private System.Windows.Forms.PictureBox pictureBox;
      private System.Windows.Forms.ComboBox selectedPage;
      private System.Windows.Forms.ComboBox selectedMatrix;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button convertButton;
      private MatrixView matrixView1;
   }
}

