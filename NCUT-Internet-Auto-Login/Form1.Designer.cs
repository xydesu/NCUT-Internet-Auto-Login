namespace NCUT_Internet_Auto_Login
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.startMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdatesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            
            this.lblAccount = new System.Windows.Forms.Label();
            this.txtAccount = new NCUT_Internet_Auto_Login.ModernUI.ModernTextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new NCUT_Internet_Auto_Login.ModernUI.ModernTextBox();
            
            this.chkAutoStart = new NCUT_Internet_Auto_Login.ModernUI.ModernCheckBox();
            this.chkStartMinimized = new NCUT_Internet_Auto_Login.ModernUI.ModernCheckBox();

            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.btnInstallService = new NCUT_Internet_Auto_Login.ModernUI.ModernButton();

            this.btnStart = new NCUT_Internet_Auto_Login.ModernUI.ModernButton();
            this.btnStop = new NCUT_Internet_Auto_Login.ModernUI.ModernButton();
            
            this.lblLogTitle = new System.Windows.Forms.Label();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lnkUninstallService = new System.Windows.Forms.LinkLabel();
            
            this.contextMenuStrip.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.SuspendLayout();
            
            // notifyIcon
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Text = "NCUT Auto Login";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            
            // contextMenuStrip
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.hideToolStripMenuItem,
            this.toolStripSeparator1,
            this.startMonitoringToolStripMenuItem,
            this.stopMonitoringToolStripMenuItem,
            this.toolStripSeparator2,
            this.checkForUpdatesMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(125, 148);
            
            // Context Menu Items
            this.showToolStripMenuItem.Text = "顯示視窗";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            this.hideToolStripMenuItem.Text = "隱藏視窗";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            this.startMonitoringToolStripMenuItem.Text = "啟動服務";
            this.startMonitoringToolStripMenuItem.Click += new System.EventHandler(this.startMonitoringToolStripMenuItem_Click);
            this.stopMonitoringToolStripMenuItem.Text = "停止服務";
            this.stopMonitoringToolStripMenuItem.Click += new System.EventHandler(this.stopMonitoringToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Text = "結束程式";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            
            // checkForUpdatesMenuItem
            this.checkForUpdatesMenuItem.Text = "檢查更新";
            this.checkForUpdatesMenuItem.Click += new System.EventHandler(this.checkForUpdatesMenuItem_Click);
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(24, 60);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(236, 37);
            this.lblTitle.Text = "NCUT Auto Login";
            
            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitle.Location = new System.Drawing.Point(28, 100);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(135, 19);
            this.lblSubtitle.Text = "校園網路自動登入系統";
            
            // lblAccount
            this.lblAccount.AutoSize = true;
            this.lblAccount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAccount.ForeColor = System.Drawing.Color.LightGray;
            this.lblAccount.Location = new System.Drawing.Point(24, 150);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(31, 15);
            this.lblAccount.Text = "帳號";
            
            // txtAccount
            this.txtAccount.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtAccount.BorderColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.txtAccount.CornerRadius = 6;
            this.txtAccount.FocusBorderColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.txtAccount.Location = new System.Drawing.Point(24, 170);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(340, 35);
            this.txtAccount.Text = "";
            
            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPassword.ForeColor = System.Drawing.Color.LightGray;
            this.lblPassword.Location = new System.Drawing.Point(24, 220);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(31, 15);
            this.lblPassword.Text = "密碼";
            
            // txtPassword
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtPassword.BorderColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.txtPassword.CornerRadius = 6;
            this.txtPassword.FocusBorderColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.txtPassword.Location = new System.Drawing.Point(24, 240);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(340, 35);
            this.txtPassword.Text = "";
            this.txtPassword.UseSystemPasswordChar = true;
            
            // chkAutoStart — 控制 Windows 服務是否開機自動啟動
            // Centering: content_width=340, total_checkboxes=110+20gap+122=252, offset=(340-252)/2=44 → X=24+44=68
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.ForeColor = System.Drawing.Color.LightGray;
            this.chkAutoStart.Location = new System.Drawing.Point(68, 295);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(110, 17);
            this.chkAutoStart.Text = "服務開機自動啟動";
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.chkAutoStart_CheckedChanged);
            
            // chkStartMinimized — 控制 GUI 程式是否開機後台啟動 → X = 68+110+20 = 198
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.ForeColor = System.Drawing.Color.LightGray;
            this.chkStartMinimized.Location = new System.Drawing.Point(198, 295);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(122, 17);
            this.chkStartMinimized.Text = "程式開機後台啟動";
            this.chkStartMinimized.CheckedChanged += new System.EventHandler(this.chkStartMinimized_CheckedChanged);

            // lblServiceStatus
            this.lblServiceStatus.AutoSize = true;
            this.lblServiceStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblServiceStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblServiceStatus.Location = new System.Drawing.Point(24, 328);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(80, 15);
            this.lblServiceStatus.Text = "服務狀態：查詢中...";

            // btnInstallService — shown only when service is NOT installed.
            // Initial Visible=false prevents a flash before Form1_Load calls UpdateServiceStatusUI().
            this.btnInstallService.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnInstallService.CornerRadius = 8;
            this.btnInstallService.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnInstallService.ForeColor = System.Drawing.Color.White;
            this.btnInstallService.HoverColor = System.Drawing.Color.FromArgb(96, 165, 250);
            this.btnInstallService.Location = new System.Drawing.Point(24, 354);
            this.btnInstallService.Name = "btnInstallService";
            this.btnInstallService.NormalColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnInstallService.PressedColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnInstallService.Size = new System.Drawing.Size(340, 44);
            this.btnInstallService.Text = "安裝服務";
            this.btnInstallService.Visible = false;
            this.btnInstallService.Click += new System.EventHandler(this.btnInstallService_Click);

            // btnStop — outline red style; positioned above btnStart (primary action when running).
            // Initial Visible=false: UpdateServiceStatusUI() in Form1_Load sets correct visibility.
            this.btnStop.CornerRadius = 8;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.HoverColor = System.Drawing.Color.FromArgb(248, 113, 113);
            this.btnStop.IsOutline = true;
            this.btnStop.Location = new System.Drawing.Point(24, 354);
            this.btnStop.Name = "btnStop";
            this.btnStop.NormalColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnStop.OutlineColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnStop.PressedColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnStop.Size = new System.Drawing.Size(340, 44);
            this.btnStop.Text = "停止服務";
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // btnStart
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnStart.CornerRadius = 8;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.HoverColor = System.Drawing.Color.FromArgb(96, 165, 250);
            this.btnStart.Location = new System.Drawing.Point(24, 410);
            this.btnStart.Name = "btnStart";
            this.btnStart.NormalColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnStart.PressedColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnStart.Size = new System.Drawing.Size(340, 44);
            this.btnStart.Text = "啟動服務";
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // lblLogTitle
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblLogTitle.Location = new System.Drawing.Point(24, 472);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(55, 15);
            this.lblLogTitle.Text = "系統狀態";
            
            // pnlLog — wrapper panel providing inner padding for the log text box
            this.pnlLog.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.pnlLog.Location = new System.Drawing.Point(24, 492);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(340, 130);
            this.pnlLog.Controls.Add(this.txtLog);

            // txtLog — inside pnlLog; 10 px padding on all sides
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
            this.txtLog.Location = new System.Drawing.Point(10, 10);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(320, 110);

            // lnkUninstallService — low-key text link at the bottom; shown only when installed
            this.lnkUninstallService.AutoSize = false;
            this.lnkUninstallService.ActiveLinkColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.lnkUninstallService.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lnkUninstallService.LinkColor = System.Drawing.Color.FromArgb(130, 130, 130);
            this.lnkUninstallService.Location = new System.Drawing.Point(24, 638);
            this.lnkUninstallService.Name = "lnkUninstallService";
            this.lnkUninstallService.Size = new System.Drawing.Size(340, 20);
            this.lnkUninstallService.Text = "解除安裝服務";
            this.lnkUninstallService.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkUninstallService.Visible = false;
            this.lnkUninstallService.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUninstallService_LinkClicked);
            
            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(400, 676);
            this.Controls.Add(this.lnkUninstallService);
            this.Controls.Add(this.pnlLog);
            this.Controls.Add(this.lblLogTitle);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnInstallService);
            this.Controls.Add(this.lblServiceStatus);
            this.Controls.Add(this.chkStartMinimized);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.lblAccount);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NCUT Internet Auto Login";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.pnlLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblAccount;
        private NCUT_Internet_Auto_Login.ModernUI.ModernTextBox txtAccount;
        private System.Windows.Forms.Label lblPassword;
        private NCUT_Internet_Auto_Login.ModernUI.ModernTextBox txtPassword;
        private NCUT_Internet_Auto_Login.ModernUI.ModernCheckBox chkAutoStart;
        private NCUT_Internet_Auto_Login.ModernUI.ModernCheckBox chkStartMinimized;
        private System.Windows.Forms.Label lblServiceStatus;
        private NCUT_Internet_Auto_Login.ModernUI.ModernButton btnInstallService;
        private NCUT_Internet_Auto_Login.ModernUI.ModernButton btnStart;
        private NCUT_Internet_Auto_Login.ModernUI.ModernButton btnStop;
        private System.Windows.Forms.Label lblLogTitle;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.LinkLabel lnkUninstallService;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}