using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NCUT_Internet_Auto_Login
{
    public partial class Form1 : Form
    {
        private bool isRunning = false;
        private const string AppName = "!_NCUT_AutoLogin";
        private const string LegacyAppName = "NCUT_Internet_Auto_Login";
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string RegistryValueName_StartMinimized = "NCUT_StartMinimized";
        private AppSettings settings;
        private WorkerRunner workerRunner;
        private System.Windows.Forms.Timer autoStartCheckTimer;

        public Form1()
        {
            InitializeComponent();

            // 套用 ModernUI 色彩風格
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.btnStart.NormalColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.txtAccount.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            
            // 設置視窗樣式
            
            // 設置視窗屬性
            // this.Resizable = false;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            
            // 設置 NotifyIcon 圖示
            if (this.Icon != null)
            {
                this.notifyIcon.Icon = this.Icon;
            }
            else
            {
                this.notifyIcon.Icon = SystemIcons.Application;
            }
            
            // 設置表單圖示
            this.ShowIcon = false; // 預設不顯示圖示
            
            // 初始化 WorkerRunner
            workerRunner = new WorkerRunner();
            workerRunner.OnLogMessage += LogMessage;
            workerRunner.OnStatusChanged += UpdateUIState;

            // 建立定時檢查 Worker 狀態的 Timer (自動啟動機制)
            autoStartCheckTimer = new System.Windows.Forms.Timer();
            autoStartCheckTimer.Interval = 5000;
            autoStartCheckTimer.Tick += AutoStartCheckTimer_Tick;
            autoStartCheckTimer.Start();
        }

        private void AutoStartCheckTimer_Tick(object sender, EventArgs e)
        {
            // 如果應該執行但沒落在運行，則自動補啟動
            if (isRunning && !workerRunner.IsRunning)
            {
                LogMessage($"{Program.GetTimestamp()} 偵測到背景服務未運行，正在自動重新啟動...");
                _ = workerRunner.StartAsync();
            }
        }

        private void UpdateUIState(bool running)
        {
            if (isRunning != running)
            {
                isRunning = running;
                this.btnStart.Enabled = !running;
                this.btnStop.Enabled = running;
                this.startMonitoringToolStripMenuItem.Enabled = !running;
                this.stopMonitoringToolStripMenuItem.Enabled = running;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 載入設定檔並填入帳號密碼
            settings = AppSettings.Load();
            this.txtAccount.Text = string.IsNullOrEmpty(settings.Username) ? "ncut" : settings.Username;
            this.txtPassword.Text = string.IsNullOrEmpty(settings.Password) ? "ncut" : settings.Password;

            // 檢查開機自啟動狀態 (若路徑錯誤發動自動修復)
            this.chkAutoStart.Checked = IsAutoStartEnabled(true);
            
            // 檢查自啟動時在後台運行的設定
            this.chkStartMinimized.Checked = IsStartMinimizedEnabled();
            
            // 根據設定更新 chkStartMinimized 的啟用狀態
            this.chkStartMinimized.Enabled = this.chkAutoStart.Checked;

            // 如果是以最小化模式啟動，則隱藏視窗到系統托盤
            if (Program.StartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
                this.notifyIcon.BalloonTipText = "程式已在後台啟動";
                this.notifyIcon.ShowBalloonTip(2000);
            }

            // 顯示 ASCII Art Banner
            LogMessage("NCUT校園網自動登入V2");
            LogMessage("by sangege & AI LIFE\n");
            LogMessage("https://github.com/apple050620312/NCUT-Internet-Auto-Login\n");
            LogMessage($"使用的帳號: {this.txtAccount.Text}");
            LogMessage($"使用的密碼: {(string.IsNullOrEmpty(this.txtPassword.Text) ? "" : "******")}\n");
            
            if (Program.StartMinimized)
            {
                LogMessage($"{Program.GetTimestamp()} 程式以後台模式啟動\n");
            }

            // 啟動後自動開始監控
            StartMonitoring();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 當視窗最小化時，隱藏到系統托盤
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
                this.notifyIcon.BalloonTipText = "程式已最小化到系統托盤";
                this.notifyIcon.ShowBalloonTip(2000);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartMonitoring();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopMonitoring();
        }

        private void StartMonitoring()
        {
            if (isRunning) return;

            // 更新帳號密碼並儲存目前輸入值到共用設定檔
            Program.Account = this.txtAccount.Text;
            Program.Password = this.txtPassword.Text;

            settings.Username = Program.Account;
            settings.Password = Program.Password;
            settings.Save();

            // 啟動進程內 Worker
            try
            {
                _ = workerRunner.StartAsync();
                LogMessage($"{Program.GetTimestamp()} 背景監控執行緒已啟動");
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 啟動背景執行緒時發生錯誤: {ex.Message}");
            }

            UpdateUIState(true);

            this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
            this.notifyIcon.BalloonTipText = "背景服務已收到啟動指示與新設定";
            this.notifyIcon.ShowBalloonTip(2000);
        }

        private void StopMonitoring()
        {
            if (!isRunning) return;

            // 停止進程內 Worker
            try
            {
                _ = workerRunner.StopAsync();
                LogMessage($"{Program.GetTimestamp()} 背景監控執行緒已停止");
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 停止背景執行緒時發生錯誤: {ex.Message}");
            }
            
            UpdateUIState(false);

            LogMessage($"{Program.GetTimestamp()} UI 控制面板已標記服務為停止狀態");

            this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
            this.notifyIcon.BalloonTipText = "背景登入服務已停止";
            this.notifyIcon.ShowBalloonTip(2000);
        }

        private void LogMessage(string message)
        {
            if (this.txtLog.InvokeRequired)
            {
                this.txtLog.Invoke(new Action(() => LogMessage(message)));
            }
            else
            {
                this.txtLog.AppendText(message + Environment.NewLine);
                this.txtLog.SelectionStart = this.txtLog.Text.Length;
                this.txtLog.ScrollToCaret();
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            this.txtLog.Clear();
        }

        #region 系統托盤功能

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowWindow();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void startMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartMonitoring();
        }

        private void stopMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopMonitoring();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 停止 Timer 檢查
            if (autoStartCheckTimer != null)
            {
                autoStartCheckTimer.Stop();
                autoStartCheckTimer.Dispose();
            }

            // 停止 Worker
            if (workerRunner != null)
            {
                _ = workerRunner.StopAsync();
                workerRunner.Dispose();
            }

            // 隱藏托盤圖示
            this.notifyIcon.Visible = false;

            // 關閉應用程式
            Application.Exit();
        }

        private void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        #endregion

        #region 開機自啟動功能

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkAutoStart.Checked)
                {
                    EnableAutoStart();
                    LogMessage($"{Program.GetTimestamp()} 已啟用開機自動啟動");
                    
                    // 啟用「自啟動時在後台運行」選項
                    this.chkStartMinimized.Enabled = true;
                }
                else
                {
                    DisableAutoStart();
                    LogMessage($"{Program.GetTimestamp()} 已停用開機自動啟動");
                    
                    // 停用「自啟動時在後台運行」選項
                    this.chkStartMinimized.Enabled = false;
                    
                    // 同時取消「自啟動時在後台運行」的設定
                    if (this.chkStartMinimized.Checked)
                    {
                        this.chkStartMinimized.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 設定開機自啟動時發生錯誤: {ex.Message}");
                MessageBox.Show($"設定開機自啟動時發生錯誤:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 恢復 checkbox 狀態
                this.chkAutoStart.CheckedChanged -= chkAutoStart_CheckedChanged;
                this.chkAutoStart.Checked = !this.chkAutoStart.Checked;
                this.chkAutoStart.CheckedChanged += chkAutoStart_CheckedChanged;
            }
        }

        private void chkStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.chkStartMinimized.Checked)
                {
                    EnableStartMinimized();
                    LogMessage($"{Program.GetTimestamp()} 已啟用自啟動時在後台運行");
                }
                else
                {
                    DisableStartMinimized();
                    LogMessage($"{Program.GetTimestamp()} 已停用自啟動時在後台運行");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 設定自啟動後台運行時發生錯誤: {ex.Message}");
                MessageBox.Show($"設定自啟動後台運行時發生錯誤:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                // 恢復 checkbox 狀態
                this.chkStartMinimized.CheckedChanged -= chkStartMinimized_CheckedChanged;
                this.chkStartMinimized.Checked = !this.chkStartMinimized.Checked;
                this.chkStartMinimized.CheckedChanged += chkStartMinimized_CheckedChanged;
            }
        }

        private bool IsAutoStartEnabled(bool autoFixPath = false)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(AppName);
                        bool isLegacy = false;
                        
                        if (value == null)
                        {
                            value = key.GetValue(LegacyAppName);
                            isLegacy = true;
                        }

                        if (value != null)
                        {
                            string currentPath = $"\"{Application.ExecutablePath}\"";
                            string savedPath = value.ToString();
                            
                            // 比對扣除 -minimized 參數後的值
                            string savedExe = savedPath.Replace(" -minimized", "").Trim();
                            
                            if (autoFixPath && (!savedExe.Equals(currentPath, StringComparison.OrdinalIgnoreCase) || isLegacy))
                            {
                                // 路徑不同或仍為舊版名稱，自動修復
                                key.SetValue(AppName, this.chkStartMinimized.Checked ? currentPath + " -minimized" : currentPath);
                                if (isLegacy && key.GetValue(LegacyAppName) != null)
                                {
                                    key.DeleteValue(LegacyAppName);
                                }
                                LogMessage($"{Program.GetTimestamp()} 已自動更新開機自啟動路徑，升級登錄檔");
                            }
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 檢查開機自啟動狀態時發生錯誤: {ex.Message}");
            }
            return false;
        }

        private bool IsStartMinimizedEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(RegistryValueName_StartMinimized);
                        if (value != null)
                        {
                            return value.ToString() == "1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 檢查自啟動後台運行狀態時發生錯誤: {ex.Message}");
            }
            return false;
        }

        private void EnableAutoStart()
        {
            string exePath = Application.ExecutablePath;
            bool withMinimized = this.chkStartMinimized.Checked;
            
            string commandLine = withMinimized ? $"\"{exePath}\" -minimized" : $"\"{exePath}\"";
            
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.SetValue(AppName, commandLine);
                    // 清除舊版命名的登錄檔，以避免重複執行
                    if (key.GetValue(LegacyAppName) != null)
                    {
                        key.DeleteValue(LegacyAppName);
                    }
                }
            }
        }

        private void DisableAutoStart()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    object value = key.GetValue(AppName);
                    if (value != null)
                    {
                        key.DeleteValue(AppName);
                    }
                    
                    if (key.GetValue(LegacyAppName) != null)
                    {
                        key.DeleteValue(LegacyAppName);
                    }
                    
                    // 同時刪除 StartMinimized 設定
                    object minimizedValue = key.GetValue(RegistryValueName_StartMinimized);
                    if (minimizedValue != null)
                    {
                        key.DeleteValue(RegistryValueName_StartMinimized);
                    }
                }
            }
        }

        private void EnableStartMinimized()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.SetValue(RegistryValueName_StartMinimized, "1");
                    
                    // 更新自啟動命令列以包含 -minimized 參數
                    if (this.chkAutoStart.Checked)
                    {
                        EnableAutoStart();
                    }
                }
            }
        }

        private void DisableStartMinimized()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    object value = key.GetValue(RegistryValueName_StartMinimized);
                    if (value != null)
                    {
                        key.DeleteValue(RegistryValueName_StartMinimized);
                    }
                    
                    // 更新自啟動命令列以移除 -minimized 參數
                    if (this.chkAutoStart.Checked)
                    {
                        EnableAutoStart();
                    }
                }
            }
        }

        #endregion

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 如果是用戶點擊關閉按鈕，則最小化到托盤而不是關閉
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
                this.notifyIcon.BalloonTipText = "程式已最小化到系統托盤，右鍵托盤圖示可選擇結束程式";
                this.notifyIcon.ShowBalloonTip(3000);
            }
            else
            {
                // 停止 Timer 檢查
                if (autoStartCheckTimer != null)
                {
                    autoStartCheckTimer.Stop();
                    autoStartCheckTimer.Dispose();
                }
                
                // 停止 Worker
                if (workerRunner != null)
                {
                    _ = workerRunner.StopAsync();
                    workerRunner.Dispose();
                }
                base.OnFormClosing(e);
            }
        }
    }
}

