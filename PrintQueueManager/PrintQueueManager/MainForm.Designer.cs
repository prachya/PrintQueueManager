namespace PrintQueueManager
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createProsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fTPLocationSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshRepoFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manuallyImportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moretestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timCheckFTP = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.StatusMessage = new System.Windows.Forms.StatusStrip();
            this.StatusMessageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.StatusMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.ItemSize = new System.Drawing.Size(70, 25);
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1215, 574);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.TabControl1_TabIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1215, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProfileToolStripMenuItem,
            this.createProsetToolStripMenuItem,
            this.fTPLocationSettingToolStripMenuItem,
            this.refreshRepoFolderToolStripMenuItem,
            this.searchOrdersToolStripMenuItem,
            this.manuallyImportCSVToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // newProfileToolStripMenuItem
            // 
            this.newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem";
            this.newProfileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newProfileToolStripMenuItem.Text = "New Profile";
            this.newProfileToolStripMenuItem.Click += new System.EventHandler(this.newProfileToolStripMenuItem_Click);
            // 
            // createProsetToolStripMenuItem
            // 
            this.createProsetToolStripMenuItem.Name = "createProsetToolStripMenuItem";
            this.createProsetToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.createProsetToolStripMenuItem.Text = "Create Preset";
            this.createProsetToolStripMenuItem.Click += new System.EventHandler(this.createProsetToolStripMenuItem_Click);
            // 
            // fTPLocationSettingToolStripMenuItem
            // 
            this.fTPLocationSettingToolStripMenuItem.Name = "fTPLocationSettingToolStripMenuItem";
            this.fTPLocationSettingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.fTPLocationSettingToolStripMenuItem.Text = "Settings";
            this.fTPLocationSettingToolStripMenuItem.Click += new System.EventHandler(this.fTPLocationSettingToolStripMenuItem_Click);
            // 
            // refreshRepoFolderToolStripMenuItem
            // 
            this.refreshRepoFolderToolStripMenuItem.Name = "refreshRepoFolderToolStripMenuItem";
            this.refreshRepoFolderToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.refreshRepoFolderToolStripMenuItem.Text = "Refresh repo folder";
            this.refreshRepoFolderToolStripMenuItem.Click += new System.EventHandler(this.refreshRepoFolderToolStripMenuItem_Click);
            // 
            // searchOrdersToolStripMenuItem
            // 
            this.searchOrdersToolStripMenuItem.Name = "searchOrdersToolStripMenuItem";
            this.searchOrdersToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.searchOrdersToolStripMenuItem.Text = "Search Orders";
            this.searchOrdersToolStripMenuItem.Click += new System.EventHandler(this.SearchOrdersToolStripMenuItem_Click);
            // 
            // manuallyImportCSVToolStripMenuItem
            // 
            this.manuallyImportCSVToolStripMenuItem.Name = "manuallyImportCSVToolStripMenuItem";
            this.manuallyImportCSVToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.manuallyImportCSVToolStripMenuItem.Text = "Manually Import CSV";
            this.manuallyImportCSVToolStripMenuItem.Click += new System.EventHandler(this.manuallyImportCSVToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.downloadToolStripMenuItem,
            this.test1ToolStripMenuItem,
            this.printTestToolStripMenuItem,
            this.moretestToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.downloadToolStripMenuItem.Text = "Put to queue";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // test1ToolStripMenuItem
            // 
            this.test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            this.test1ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.test1ToolStripMenuItem.Text = "test1";
            this.test1ToolStripMenuItem.Click += new System.EventHandler(this.test1ToolStripMenuItem_Click);
            // 
            // printTestToolStripMenuItem
            // 
            this.printTestToolStripMenuItem.Name = "printTestToolStripMenuItem";
            this.printTestToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.printTestToolStripMenuItem.Text = "print test";
            this.printTestToolStripMenuItem.Click += new System.EventHandler(this.printTestToolStripMenuItem_Click);
            // 
            // moretestToolStripMenuItem
            // 
            this.moretestToolStripMenuItem.Name = "moretestToolStripMenuItem";
            this.moretestToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.moretestToolStripMenuItem.Text = "moretest";
            this.moretestToolStripMenuItem.Click += new System.EventHandler(this.moretestToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // timCheckFTP
            // 
            this.timCheckFTP.Interval = 10000;
            this.timCheckFTP.Tick += new System.EventHandler(this.timCheckFTP_Tick);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 598);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1215, 39);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            // 
            // StatusMessage
            // 
            this.StatusMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMessageLabel});
            this.StatusMessage.Location = new System.Drawing.Point(0, 637);
            this.StatusMessage.Name = "StatusMessage";
            this.StatusMessage.Size = new System.Drawing.Size(1215, 22);
            this.StatusMessage.TabIndex = 0;
            this.StatusMessage.Text = "statusStrip1";
            // 
            // StatusMessageLabel
            // 
            this.StatusMessageLabel.Name = "StatusMessageLabel";
            this.StatusMessageLabel.Size = new System.Drawing.Size(13, 17);
            this.StatusMessageLabel.Text = "  ";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 659);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.StatusMessage);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Print Queue Manager v1.42.35";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.StatusMessage.ResumeLayout(false);
            this.StatusMessage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fTPLocationSettingToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.Timer timCheckFTP;
        private System.Windows.Forms.ToolStripMenuItem createProsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshRepoFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem test1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printTestToolStripMenuItem;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem moretestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchOrdersToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip StatusMessage;
        private System.Windows.Forms.ToolStripStatusLabel StatusMessageLabel;
        private System.Windows.Forms.ToolStripMenuItem manuallyImportCSVToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

