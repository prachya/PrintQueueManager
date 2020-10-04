namespace PrintQueueManager
{
    partial class OrdersBrowser
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.orderGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProfileList = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.OrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShortTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeOfOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.orderGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // orderGridView
            // 
            this.orderGridView.AllowUserToAddRows = false;
            this.orderGridView.AllowUserToDeleteRows = false;
            this.orderGridView.AllowUserToResizeRows = false;
            this.orderGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.orderGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.orderGridView.ColumnHeadersHeight = 40;
            this.orderGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.orderGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.OrderId,
            this.SKU,
            this.ShortTitle,
            this.Qty,
            this.CustomerName,
            this.TimeOfOrder,
            this.PrintRun,
            this.OrderNotes,
            this.CustomerEmail,
            this.Remark});
            this.orderGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderGridView.EnableHeadersVisualStyles = false;
            this.orderGridView.GridColor = System.Drawing.SystemColors.Control;
            this.orderGridView.Location = new System.Drawing.Point(0, 51);
            this.orderGridView.Name = "orderGridView";
            this.orderGridView.ReadOnly = true;
            this.orderGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.orderGridView.Size = new System.Drawing.Size(977, 274);
            this.orderGridView.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbProfileList);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(977, 51);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 256);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(977, 69);
            this.panel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Search (Order ID or SKU )";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(157, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(130, 20);
            this.txtSearch.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(293, 10);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = " Search Database";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(97, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(24, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(54, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(715, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Profile:";
            // 
            // cmbProfileList
            // 
            this.cmbProfileList.FormattingEnabled = true;
            this.cmbProfileList.Location = new System.Drawing.Point(761, 12);
            this.cmbProfileList.Name = "cmbProfileList";
            this.cmbProfileList.Size = new System.Drawing.Size(196, 21);
            this.cmbProfileList.TabIndex = 19;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(571, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Load orders from CSV";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(420, 10);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(109, 23);
            this.button5.TabIndex = 22;
            this.button5.Text = " Search this table";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // OrderId
            // 
            this.OrderId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OrderId.DataPropertyName = "OrderId";
            this.OrderId.HeaderText = "Order Id";
            this.OrderId.MinimumWidth = 50;
            this.OrderId.Name = "OrderId";
            this.OrderId.ReadOnly = true;
            this.OrderId.Width = 70;
            // 
            // SKU
            // 
            this.SKU.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SKU.DataPropertyName = "SKU";
            this.SKU.FillWeight = 38.91166F;
            this.SKU.HeaderText = "SKU";
            this.SKU.Name = "SKU";
            this.SKU.ReadOnly = true;
            this.SKU.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SKU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SKU.Width = 150;
            // 
            // ShortTitle
            // 
            this.ShortTitle.DataPropertyName = "ShortTitle";
            this.ShortTitle.HeaderText = "Short Title";
            this.ShortTitle.MinimumWidth = 200;
            this.ShortTitle.Name = "ShortTitle";
            this.ShortTitle.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Qty.DataPropertyName = "Quantity";
            this.Qty.FillWeight = 124.2535F;
            this.Qty.HeaderText = "Qty";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            this.Qty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Qty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CustomerName
            // 
            this.CustomerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CustomerName.DataPropertyName = "CustomerName";
            this.CustomerName.FillWeight = 38.91166F;
            this.CustomerName.HeaderText = "Customer Name";
            this.CustomerName.MinimumWidth = 150;
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            this.CustomerName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CustomerName.Width = 200;
            // 
            // TimeOfOrder
            // 
            this.TimeOfOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TimeOfOrder.DataPropertyName = "TimeOfOrder";
            this.TimeOfOrder.FillWeight = 186.7286F;
            this.TimeOfOrder.HeaderText = "Time of Order";
            this.TimeOfOrder.Name = "TimeOfOrder";
            this.TimeOfOrder.ReadOnly = true;
            this.TimeOfOrder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TimeOfOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TimeOfOrder.Width = 150;
            // 
            // PrintRun
            // 
            this.PrintRun.DataPropertyName = "PrintRun";
            this.PrintRun.HeaderText = "Print Run";
            this.PrintRun.MinimumWidth = 100;
            this.PrintRun.Name = "PrintRun";
            this.PrintRun.ReadOnly = true;
            // 
            // OrderNotes
            // 
            this.OrderNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OrderNotes.DataPropertyName = "OrderNotes";
            this.OrderNotes.FillWeight = 38.91166F;
            this.OrderNotes.HeaderText = "Order Notes";
            this.OrderNotes.MinimumWidth = 100;
            this.OrderNotes.Name = "OrderNotes";
            this.OrderNotes.ReadOnly = true;
            this.OrderNotes.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.OrderNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CustomerEmail
            // 
            this.CustomerEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CustomerEmail.DataPropertyName = "CustomerEmail";
            this.CustomerEmail.FillWeight = 233.3712F;
            this.CustomerEmail.HeaderText = "Customer email";
            this.CustomerEmail.MinimumWidth = 150;
            this.CustomerEmail.Name = "CustomerEmail";
            this.CustomerEmail.ReadOnly = true;
            this.CustomerEmail.Width = 200;
            // 
            // Remark
            // 
            this.Remark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Remark.DataPropertyName = "Remark";
            this.Remark.HeaderText = "Remark";
            this.Remark.MinimumWidth = 100;
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // OrdersBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 325);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.orderGridView);
            this.Controls.Add(this.panel1);
            this.Name = "OrdersBrowser";
            this.Text = "OrdersBrowser";
            ((System.ComponentModel.ISupportInitialize)(this.orderGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView orderGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProfileList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKU;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeOfOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
    }
}