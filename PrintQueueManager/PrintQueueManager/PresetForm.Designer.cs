namespace PrintQueueManager
{
    partial class PresetForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbPapersize = new System.Windows.Forms.ComboBox();
            this.cmbQualities = new System.Windows.Forms.ComboBox();
            this.cmbPaperSources = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmbMediaType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtId = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.btnAddPrinter = new System.Windows.Forms.Button();
            this.cmbPrinters = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnEditPrinter = new System.Windows.Forms.Button();
            this.btnDeletePrinter = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Media type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Paper size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Print Quality";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Paper source";
            // 
            // cmbPapersize
            // 
            this.cmbPapersize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPapersize.FormattingEnabled = true;
            this.cmbPapersize.Location = new System.Drawing.Point(158, 112);
            this.cmbPapersize.Name = "cmbPapersize";
            this.cmbPapersize.Size = new System.Drawing.Size(245, 21);
            this.cmbPapersize.TabIndex = 5;
            // 
            // cmbQualities
            // 
            this.cmbQualities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQualities.FormattingEnabled = true;
            this.cmbQualities.Location = new System.Drawing.Point(158, 159);
            this.cmbQualities.Name = "cmbQualities";
            this.cmbQualities.Size = new System.Drawing.Size(245, 21);
            this.cmbQualities.TabIndex = 6;
            // 
            // cmbPaperSources
            // 
            this.cmbPaperSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaperSources.FormattingEnabled = true;
            this.cmbPaperSources.Location = new System.Drawing.Point(158, 206);
            this.cmbPaperSources.Name = "cmbPaperSources";
            this.cmbPaperSources.Size = new System.Drawing.Size(245, 21);
            this.cmbPaperSources.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(246, 403);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(341, 403);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cmbMediaType
            // 
            this.cmbMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMediaType.FormattingEnabled = true;
            this.cmbMediaType.Location = new System.Drawing.Point(158, 66);
            this.cmbMediaType.Name = "cmbMediaType";
            this.cmbMediaType.Size = new System.Drawing.Size(245, 21);
            this.cmbMediaType.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(51, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Preset name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(158, 20);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(245, 20);
            this.textBox1.TabIndex = 12;
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(432, 138);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(60, 20);
            this.txtId.TabIndex = 13;
            this.txtId.Visible = false;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.button3.Location = new System.Drawing.Point(54, 403);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnAddPrinter
            // 
            this.btnAddPrinter.Location = new System.Drawing.Point(117, 59);
            this.btnAddPrinter.Name = "btnAddPrinter";
            this.btnAddPrinter.Size = new System.Drawing.Size(65, 23);
            this.btnAddPrinter.TabIndex = 15;
            this.btnAddPrinter.Text = "Add ";
            this.btnAddPrinter.UseVisualStyleBackColor = true;
            this.btnAddPrinter.Click += new System.EventHandler(this.button4_Click);
            // 
            // cmbPrinters
            // 
            this.cmbPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinters.FormattingEnabled = true;
            this.cmbPrinters.Location = new System.Drawing.Point(118, 24);
            this.cmbPrinters.Name = "cmbPrinters";
            this.cmbPrinters.Size = new System.Drawing.Size(309, 21);
            this.cmbPrinters.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Configured Printers";
            // 
            // btnEditPrinter
            // 
            this.btnEditPrinter.Location = new System.Drawing.Point(188, 59);
            this.btnEditPrinter.Name = "btnEditPrinter";
            this.btnEditPrinter.Size = new System.Drawing.Size(65, 23);
            this.btnEditPrinter.TabIndex = 18;
            this.btnEditPrinter.Text = "Edit";
            this.btnEditPrinter.UseVisualStyleBackColor = true;
            this.btnEditPrinter.Click += new System.EventHandler(this.btnEditPrinter_Click);
            // 
            // btnDeletePrinter
            // 
            this.btnDeletePrinter.Location = new System.Drawing.Point(259, 59);
            this.btnDeletePrinter.Name = "btnDeletePrinter";
            this.btnDeletePrinter.Size = new System.Drawing.Size(65, 23);
            this.btnDeletePrinter.TabIndex = 19;
            this.btnDeletePrinter.Text = "Delete";
            this.btnDeletePrinter.UseVisualStyleBackColor = true;
            this.btnDeletePrinter.Click += new System.EventHandler(this.btnDeletePrinter_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbPrinters);
            this.panel1.Controls.Add(this.btnDeletePrinter);
            this.panel1.Controls.Add(this.btnAddPrinter);
            this.panel1.Controls.Add(this.btnEditPrinter);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(40, 256);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(441, 106);
            this.panel1.TabIndex = 20;
            this.panel1.Visible = false;
            // 
            // PresetForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(493, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbMediaType);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbPaperSources);
            this.Controls.Add(this.cmbQualities);
            this.Controls.Add(this.cmbPapersize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PresetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PresetForm";
            this.Load += new System.EventHandler(this.PresetForm_Load);
            this.Shown += new System.EventHandler(this.PresetForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbPapersize;
        private System.Windows.Forms.ComboBox cmbQualities;
        private System.Windows.Forms.ComboBox cmbPaperSources;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox cmbMediaType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnAddPrinter;
        private System.Windows.Forms.ComboBox cmbPrinters;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnEditPrinter;
        private System.Windows.Forms.Button btnDeletePrinter;
        private System.Windows.Forms.Panel panel1;
    }
}