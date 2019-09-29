namespace PrintQueueManager
{
    partial class ProfileFrame
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileFrame));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEditFilter = new System.Windows.Forms.Button();
            this.cmbPrintRun = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnChangePrintStatus = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.btnKill = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.cmbPresets = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPreset = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFilterTypes = new System.Windows.Forms.ComboBox();
            this.txtProfileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.lblLeft = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPaused = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPending = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblComlete = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.orderGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintingOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SKU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reprint = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ShortTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeOfOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Linked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LinkedFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrintRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfileOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.startPrintTimer = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.cmbPageSelector = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.orderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orderGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEditFilter);
            this.panel1.Controls.Add(this.cmbPrintRun);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.btnChangePrintStatus);
            this.panel1.Controls.Add(this.btnMove);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.cmbPrinter);
            this.panel1.Controls.Add(this.cmbPresets);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblPreset);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbFilterTypes);
            this.panel1.Controls.Add(this.txtProfileName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(1298, 114);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnEditFilter
            // 
            this.btnEditFilter.Location = new System.Drawing.Point(663, 12);
            this.btnEditFilter.Name = "btnEditFilter";
            this.btnEditFilter.Size = new System.Drawing.Size(23, 23);
            this.btnEditFilter.TabIndex = 20;
            this.btnEditFilter.Text = "...";
            this.btnEditFilter.UseVisualStyleBackColor = true;
            this.btnEditFilter.Click += new System.EventHandler(this.BtnEditFilter_Click);
            // 
            // cmbPrintRun
            // 
            this.cmbPrintRun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrintRun.FormattingEnabled = true;
            this.cmbPrintRun.Location = new System.Drawing.Point(407, 86);
            this.cmbPrintRun.Name = "cmbPrintRun";
            this.cmbPrintRun.Size = new System.Drawing.Size(252, 21);
            this.cmbPrintRun.TabIndex = 19;
            this.cmbPrintRun.SelectedIndexChanged += new System.EventHandler(this.cmbPrintRun_SelectedIndexChanged);
            this.cmbPrintRun.SelectionChangeCommitted += new System.EventHandler(this.cmbPrintRun_SelectionChangeCommitted);
            this.cmbPrintRun.SelectedValueChanged += new System.EventHandler(this.cmbPrintRun_SelectedValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(350, 89);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(58, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "Printer run:";
            // 
            // btnChangePrintStatus
            // 
            this.btnChangePrintStatus.Location = new System.Drawing.Point(202, 85);
            this.btnChangePrintStatus.Name = "btnChangePrintStatus";
            this.btnChangePrintStatus.Size = new System.Drawing.Size(128, 23);
            this.btnChangePrintStatus.TabIndex = 17;
            this.btnChangePrintStatus.Text = "Change Print Status";
            this.toolTip1.SetToolTip(this.btnChangePrintStatus, "Change Print Status manually");
            this.btnChangePrintStatus.UseVisualStyleBackColor = true;
            this.btnChangePrintStatus.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(115, 85);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(69, 23);
            this.btnMove.TabIndex = 16;
            this.btnMove.Text = "Move";
            this.toolTip1.SetToolTip(this.btnMove, "Move selected items to another profile");
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(663, 51);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(23, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.btnDeleteProfile);
            this.panel4.Controls.Add(this.btnKill);
            this.panel4.Controls.Add(this.checkBox1);
            this.panel4.Controls.Add(this.maskedTextBox2);
            this.panel4.Controls.Add(this.maskedTextBox1);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.lblStatus);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Location = new System.Drawing.Point(706, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(403, 111);
            this.panel4.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(146, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Status:";
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Location = new System.Drawing.Point(281, 10);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(102, 23);
            this.btnDeleteProfile.TabIndex = 22;
            this.btnDeleteProfile.Text = "Delete Profile";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Click += new System.EventHandler(this.BtnDeleteProfile_Click);
            // 
            // btnKill
            // 
            this.btnKill.Location = new System.Drawing.Point(189, 10);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(75, 23);
            this.btnKill.TabIndex = 21;
            this.btnKill.Text = "Kill";
            this.btnKill.UseVisualStyleBackColor = true;
            this.btnKill.Click += new System.EventHandler(this.button6_Click_2);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(149, 50);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Auto start/stop";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // maskedTextBox2
            // 
            this.maskedTextBox2.Location = new System.Drawing.Point(71, 79);
            this.maskedTextBox2.Mask = "00.00";
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.Size = new System.Drawing.Size(53, 20);
            this.maskedTextBox2.TabIndex = 19;
            this.maskedTextBox2.Leave += new System.EventHandler(this.maskedTextBox2_Leave);
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Location = new System.Drawing.Point(71, 50);
            this.maskedTextBox1.Mask = "90.00";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(53, 20);
            this.maskedTextBox1.TabIndex = 18;
            this.maskedTextBox1.Leave += new System.EventHandler(this.maskedTextBox1_Leave);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Stop time:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Start time:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(99, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "Stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.White;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(198, 82);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 15);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Stopped";
            this.lblStatus.TextChanged += new System.EventHandler(this.lblStatus_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = global::PrintQueueManager.Properties.Resources.if_arrow_full_down_103291_1_;
            this.btnDown.Location = new System.Drawing.Point(43, 85);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(35, 23);
            this.btnDown.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnDown, "Move selected item down.");
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(6, 85);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(33, 23);
            this.btnUp.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnUp, "Move selected item up.");
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Edit...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinter.FormattingEnabled = true;
            this.cmbPrinter.Location = new System.Drawing.Point(407, 52);
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.Size = new System.Drawing.Size(252, 21);
            this.cmbPrinter.TabIndex = 8;
            this.cmbPrinter.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            this.cmbPrinter.SelectionChangeCommitted += new System.EventHandler(this.comboBox2_SelectionChangeCommitted);
            // 
            // cmbPresets
            // 
            this.cmbPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPresets.FormattingEnabled = true;
            this.cmbPresets.Location = new System.Drawing.Point(62, 51);
            this.cmbPresets.Name = "cmbPresets";
            this.cmbPresets.Size = new System.Drawing.Size(231, 21);
            this.cmbPresets.TabIndex = 7;
            this.cmbPresets.SelectionChangeCommitted += new System.EventHandler(this.cmbPresets_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(350, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Printer:";
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Location = new System.Drawing.Point(20, 55);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(40, 13);
            this.lblPreset.TabIndex = 5;
            this.lblPreset.Text = "Preset:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(350, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Filter type:";
            // 
            // cmbFilterTypes
            // 
            this.cmbFilterTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterTypes.FormattingEnabled = true;
            this.cmbFilterTypes.Items.AddRange(new object[] {
            "SKU countains A3",
            "SKU contains A5",
            "SKU contains A4",
            "Starts with D",
            "Starts with N,H",
            "Starts with C",
            "SKU contains A2",
            "SKU contains A1"});
            this.cmbFilterTypes.Location = new System.Drawing.Point(407, 13);
            this.cmbFilterTypes.Name = "cmbFilterTypes";
            this.cmbFilterTypes.Size = new System.Drawing.Size(252, 21);
            this.cmbFilterTypes.TabIndex = 3;
            this.cmbFilterTypes.SelectedIndexChanged += new System.EventHandler(this.cmbFilterTypes_SelectedIndexChanged);
            this.cmbFilterTypes.SelectionChangeCommitted += new System.EventHandler(this.cmbFilterTypes_SelectionChangeCommitted);
            // 
            // txtProfileName
            // 
            this.txtProfileName.Location = new System.Drawing.Point(62, 15);
            this.txtProfileName.Name = "txtProfileName";
            this.txtProfileName.Size = new System.Drawing.Size(234, 20);
            this.txtProfileName.TabIndex = 2;
            this.txtProfileName.TextChanged += new System.EventHandler(this.txtProfileName_TextChanged);
            this.txtProfileName.Leave += new System.EventHandler(this.txtProfileName_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.cmbPageSelector);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 379);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1298, 82);
            this.panel2.TabIndex = 2;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(368, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(44, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "Next";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.lblLeft);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.lblPaused);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.lblPending);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.lblComlete);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.panel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel3.Location = new System.Drawing.Point(1002, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(296, 82);
            this.panel3.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Paused:";
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Location = new System.Drawing.Point(228, 45);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(35, 13);
            this.lblLeft.TabIndex = 12;
            this.lblLeft.Text = "[     ]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(164, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Pending:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // lblPaused
            // 
            this.lblPaused.AutoSize = true;
            this.lblPaused.Location = new System.Drawing.Point(94, 45);
            this.lblPaused.Name = "lblPaused";
            this.lblPaused.Size = new System.Drawing.Size(35, 13);
            this.lblPaused.TabIndex = 11;
            this.lblPaused.Text = "[     ]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Complete:";
            // 
            // lblPending
            // 
            this.lblPending.AutoSize = true;
            this.lblPending.Location = new System.Drawing.Point(228, 19);
            this.lblPending.Name = "lblPending";
            this.lblPending.Size = new System.Drawing.Size(35, 13);
            this.lblPending.TabIndex = 10;
            this.lblPending.Text = "[     ]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(164, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Time left:";
            // 
            // lblComlete
            // 
            this.lblComlete.AutoSize = true;
            this.lblComlete.Location = new System.Drawing.Point(94, 20);
            this.lblComlete.Name = "lblComlete";
            this.lblComlete.Size = new System.Drawing.Size(35, 13);
            this.lblComlete.TabIndex = 9;
            this.lblComlete.Text = "[     ]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(64, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(298, 20);
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // orderGridView
            // 
            this.orderGridView.AllowUserToAddRows = false;
            this.orderGridView.AllowUserToDeleteRows = false;
            this.orderGridView.AllowUserToResizeRows = false;
            this.orderGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.orderGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.orderGridView.ColumnHeadersHeight = 40;
            this.orderGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.orderGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.PrintingOrder,
            this.Qty,
            this.SKU,
            this.Reprint,
            this.ShortTitle,
            this.CustomerName,
            this.TimeOfOrder,
            this.Linked,
            this.LinkedFileName,
            this.PrintStatus,
            this.PrintRun,
            this.OrderId,
            this.OrderNotes,
            this.CustomerEmail,
            this.ProfileOrder,
            this.Remark});
            this.orderGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderGridView.EnableHeadersVisualStyles = false;
            this.orderGridView.Location = new System.Drawing.Point(0, 114);
            this.orderGridView.Name = "orderGridView";
            this.orderGridView.ReadOnly = true;
            this.orderGridView.Size = new System.Drawing.Size(1298, 265);
            this.orderGridView.TabIndex = 3;
            this.orderGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.orderGridView_CellContentClick);
            this.orderGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.orderGridView_CellFormatting);
            this.orderGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.orderGridView_CellMouseClick);
            this.orderGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.orderGridView_CellMouseEnter);
            this.orderGridView.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.orderGridView_CellMouseMove);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Column1";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // PrintingOrder
            // 
            this.PrintingOrder.DataPropertyName = "PrintingOrder";
            this.PrintingOrder.HeaderText = "Column1";
            this.PrintingOrder.Name = "PrintingOrder";
            this.PrintingOrder.ReadOnly = true;
            this.PrintingOrder.Visible = false;
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
            // Reprint
            // 
            this.Reprint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Reprint.DataPropertyName = "Reprint";
            this.Reprint.HeaderText = "Reprint";
            this.Reprint.MinimumWidth = 50;
            this.Reprint.Name = "Reprint";
            this.Reprint.ReadOnly = true;
            this.Reprint.Text = "Reprint";
            this.Reprint.Width = 50;
            // 
            // ShortTitle
            // 
            this.ShortTitle.DataPropertyName = "ShortTitle";
            this.ShortTitle.HeaderText = "Short Title";
            this.ShortTitle.MinimumWidth = 200;
            this.ShortTitle.Name = "ShortTitle";
            this.ShortTitle.ReadOnly = true;
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
            // Linked
            // 
            this.Linked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Linked.DataPropertyName = "Linked";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Linked.DefaultCellStyle = dataGridViewCellStyle2;
            this.Linked.HeaderText = "Linked";
            this.Linked.Name = "Linked";
            this.Linked.ReadOnly = true;
            this.Linked.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Linked.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LinkedFileName
            // 
            this.LinkedFileName.DataPropertyName = "LinkedFileName";
            this.LinkedFileName.HeaderText = "Filename";
            this.LinkedFileName.MinimumWidth = 70;
            this.LinkedFileName.Name = "LinkedFileName";
            this.LinkedFileName.ReadOnly = true;
            // 
            // PrintStatus
            // 
            this.PrintStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PrintStatus.DataPropertyName = "PrintingStatus";
            this.PrintStatus.FillWeight = 38.91166F;
            this.PrintStatus.HeaderText = "Print Status";
            this.PrintStatus.Name = "PrintStatus";
            this.PrintStatus.ReadOnly = true;
            this.PrintStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PrintStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PrintStatus.Width = 80;
            // 
            // PrintRun
            // 
            this.PrintRun.DataPropertyName = "PrintRun";
            this.PrintRun.HeaderText = "Print Run";
            this.PrintRun.MinimumWidth = 100;
            this.PrintRun.Name = "PrintRun";
            this.PrintRun.ReadOnly = true;
            // 
            // OrderId
            // 
            this.OrderId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OrderId.DataPropertyName = "Id";
            this.OrderId.HeaderText = "Order Id";
            this.OrderId.MinimumWidth = 50;
            this.OrderId.Name = "OrderId";
            this.OrderId.ReadOnly = true;
            this.OrderId.Width = 70;
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
            // ProfileOrder
            // 
            this.ProfileOrder.DataPropertyName = "ProfileOrder";
            this.ProfileOrder.HeaderText = "ProfileOrder";
            this.ProfileOrder.Name = "ProfileOrder";
            this.ProfileOrder.ReadOnly = true;
            this.ProfileOrder.Visible = false;
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "PDF files|*.pdf";
            // 
            // startPrintTimer
            // 
            this.startPrintTimer.Interval = 60000;
            this.startPrintTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // cmbPageSelector
            // 
            this.cmbPageSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPageSelector.FormattingEnabled = true;
            this.cmbPageSelector.Location = new System.Drawing.Point(474, 6);
            this.cmbPageSelector.Name = "cmbPageSelector";
            this.cmbPageSelector.Size = new System.Drawing.Size(75, 21);
            this.cmbPageSelector.TabIndex = 15;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(432, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 13);
            this.label13.TabIndex = 16;
            this.label13.Text = "Page";
            // 
            // orderBindingSource
            // 
            this.orderBindingSource.DataSource = typeof(PrintQueueManager.Data.Order);
            // 
            // ProfileFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orderGridView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "ProfileFrame";
            this.Size = new System.Drawing.Size(1298, 461);
            this.Load += new System.EventHandler(this.ProfileFrame_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orderGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbFilterTypes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView orderGridView;
        private System.Windows.Forms.BindingSource orderBindingSource;
        public System.Windows.Forms.TextBox txtProfileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPreset;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPaused;
        private System.Windows.Forms.Label lblPending;
        private System.Windows.Forms.Label lblComlete;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer startPrintTimer;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.Button btnDown;
        public System.Windows.Forms.Button btnUp;
        public System.Windows.Forms.ComboBox cmbPrinter;
        public System.Windows.Forms.ComboBox cmbPresets;
        public System.Windows.Forms.MaskedTextBox maskedTextBox1;
        public System.Windows.Forms.MaskedTextBox maskedTextBox2;
        public System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button5;
        public System.Windows.Forms.Label lblLeft;
        public System.Windows.Forms.Button btnMove;
        public System.Windows.Forms.Button btnChangePrintStatus;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button btnKill;
        public System.Windows.Forms.ComboBox cmbPrintRun;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintingOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn SKU;
        private System.Windows.Forms.DataGridViewButtonColumn Reprint;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeOfOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linked;
        private System.Windows.Forms.DataGridViewTextBoxColumn LinkedFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrintRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfileOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.Button btnEditFilter;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbPageSelector;
    }
}
