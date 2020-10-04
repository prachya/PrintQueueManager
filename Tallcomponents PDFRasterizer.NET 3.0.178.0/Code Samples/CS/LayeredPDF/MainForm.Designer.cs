namespace LayeredPDF
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
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.splitContainer2 = new System.Windows.Forms.SplitContainer();
         this.pagesListBox = new System.Windows.Forms.ListBox();
         this.layersListBox = new System.Windows.Forms.CheckedListBox();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.label1 = new System.Windows.Forms.Label();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.splitContainer2.Panel1.SuspendLayout();
         this.splitContainer2.Panel2.SuspendLayout();
         this.splitContainer2.SuspendLayout();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 24);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
         this.splitContainer1.Size = new System.Drawing.Size(705, 442);
         this.splitContainer1.SplitterDistance = 155;
         this.splitContainer1.TabIndex = 0;
         // 
         // splitContainer2
         // 
         this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer2.Location = new System.Drawing.Point(0, 0);
         this.splitContainer2.Name = "splitContainer2";
         this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer2.Panel1
         // 
         this.splitContainer2.Panel1.Controls.Add(this.pagesListBox);
         // 
         // splitContainer2.Panel2
         // 
         this.splitContainer2.Panel2.Controls.Add(this.label1);
         this.splitContainer2.Panel2.Controls.Add(this.layersListBox);
         this.splitContainer2.Size = new System.Drawing.Size(155, 442);
         this.splitContainer2.SplitterDistance = 253;
         this.splitContainer2.TabIndex = 0;
         // 
         // pagesListBox
         // 
         this.pagesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pagesListBox.FormattingEnabled = true;
         this.pagesListBox.Location = new System.Drawing.Point(0, 0);
         this.pagesListBox.Name = "pagesListBox";
         this.pagesListBox.Size = new System.Drawing.Size(155, 253);
         this.pagesListBox.TabIndex = 0;
         this.pagesListBox.SelectedIndexChanged += new System.EventHandler(this.OnPageIndexChanged);
         // 
         // layersListBox
         // 
         this.layersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.layersListBox.FormattingEnabled = true;
         this.layersListBox.Location = new System.Drawing.Point(0, 29);
         this.layersListBox.Name = "layersListBox";
         this.layersListBox.Size = new System.Drawing.Size(155, 154);
         this.layersListBox.TabIndex = 0;
         this.layersListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnLayerCheck);
         // 
         // pictureBox1
         // 
         this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
         this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.pictureBox1.Location = new System.Drawing.Point(0, 0);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(546, 442);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pictureBox1.TabIndex = 0;
         this.pictureBox1.TabStop = false;
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(705, 24);
         this.menuStrip1.TabIndex = 1;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // openToolStripMenuItem
         // 
         this.openToolStripMenuItem.Name = "openToolStripMenuItem";
         this.openToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
         this.openToolStripMenuItem.Text = "Open...";
         this.openToolStripMenuItem.Click += new System.EventHandler(this.OnOpenClick);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(4, 8);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(38, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Layers";
         // 
         // MainForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(705, 466);
         this.Controls.Add(this.splitContainer1);
         this.Controls.Add(this.menuStrip1);
         this.Name = "MainForm";
         this.Text = "Layered PDF Demo Application";
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         this.splitContainer1.ResumeLayout(false);
         this.splitContainer2.Panel1.ResumeLayout(false);
         this.splitContainer2.Panel2.ResumeLayout(false);
         this.splitContainer2.Panel2.PerformLayout();
         this.splitContainer2.ResumeLayout(false);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.SplitContainer splitContainer2;
      private System.Windows.Forms.ListBox pagesListBox;
      private System.Windows.Forms.CheckedListBox layersListBox;
      private System.Windows.Forms.PictureBox pictureBox1;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
      private System.Windows.Forms.Label label1;
   }
}

