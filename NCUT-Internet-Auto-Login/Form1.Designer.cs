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
            this.txtLog = new System.Windows.Forms.TextBox();
            
            this.contextMenuStrip.SuspendLayout();
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
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(125, 126);
            
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
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.ForeColor = System.Drawing.Color.LightGray;
            this.chkAutoStart.Location = new System.Drawing.Point(24, 298);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(110, 17);
            this.chkAutoStart.Text = "服務開機自動啟動";
            this.chkAutoStart.CheckedChanged += new System.EventHandler(this.chkAutoStart_CheckedChanged);
            
            // chkStartMinimized — 控制 GUI 程式是否開機後台啟動
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.ForeColor = System.Drawing.Color.LightGray;
            this.chkStartMinimized.Location = new System.Drawing.Point(190, 298);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(122, 17);
            this.chkStartMinimized.Text = "程式開機後台啟動";
            this.chkStartMinimized.CheckedChanged += new System.EventHandler(this.chkStartMinimized_CheckedChanged);

            // lblServiceStatus
            this.lblServiceStatus.AutoSize = true;
            this.lblServiceStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblServiceStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblServiceStatus.Location = new System.Drawing.Point(24, 330);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(80, 15);
            this.lblServiceStatus.Text = "服務狀態：查詢中...";

            // btnInstallService
            this.btnInstallService.BackColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.btnInstallService.CornerRadius = 8;
            this.btnInstallService.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnInstallService.ForeColor = System.Drawing.Color.White;
            this.btnInstallService.HoverColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnInstallService.Location = new System.Drawing.Point(24, 355);
            this.btnInstallService.Name = "btnInstallService";
            this.btnInstallService.NormalColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.btnInstallService.PressedColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.btnInstallService.Size = new System.Drawing.Size(340, 40);
            this.btnInstallService.Text = "安裝服務";
            this.btnInstallService.Click += new System.EventHandler(this.btnInstallService_Click);

            // btnStart
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnStart.CornerRadius = 8;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.HoverColor = System.Drawing.Color.FromArgb(96, 165, 250);
            this.btnStart.Location = new System.Drawing.Point(24, 408);
            this.btnStart.Name = "btnStart";
            this.btnStart.NormalColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnStart.PressedColor = System.Drawing.Color.FromArgb(37, 99, 235);
            this.btnStart.Size = new System.Drawing.Size(340, 48);
            this.btnStart.Text = "啟動服務";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            
            // btnStop
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnStop.CornerRadius = 8;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.HoverColor = System.Drawing.Color.FromArgb(248, 113, 113);
            this.btnStop.Location = new System.Drawing.Point(24, 468);
            this.btnStop.Name = "btnStop";
            this.btnStop.NormalColor = System.Drawing.Color.FromArgb(239, 68, 68);
            this.btnStop.PressedColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnStop.Size = new System.Drawing.Size(340, 48);
            this.btnStop.Text = "停止服務";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            
            // lblLogTitle
            this.lblLogTitle.AutoSize = true;
            this.lblLogTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLogTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblLogTitle.Location = new System.Drawing.Point(24, 534);
            this.lblLogTitle.Name = "lblLogTitle";
            this.lblLogTitle.Size = new System.Drawing.Size(55, 15);
            this.lblLogTitle.Text = "系統狀態";
            
            // txtLog
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(21, 21, 21);
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
            this.txtLog.Location = new System.Drawing.Point(24, 554);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(340, 110);
            
            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.ClientSize = new System.Drawing.Size(400, 690);
            this.Controls.Add(this.txtLog);
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
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopMonitoringToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}