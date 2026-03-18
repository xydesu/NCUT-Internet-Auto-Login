using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NCUT_Internet_Auto_Login
{
    public partial class Form1 : Form
    {
        private const string ServiceName = "NCUT Auto Login Service";
        private const string AppName = "!_NCUT_AutoLogin";
        private const string LegacyAppName = "NCUT_Internet_Auto_Login";
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const int ServiceOperationTimeoutSeconds = 15;

        private AppSettings settings;
        private System.Windows.Forms.Timer serviceStatusTimer;

        public Form1()
        {
            InitializeComponent();

            // 套用 ModernUI 色彩風格
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.btnStart.NormalColor = Color.FromArgb(59, 130, 246);
            this.txtAccount.BackColor = Color.FromArgb(45, 45, 45);
            this.txtPassword.BackColor = Color.FromArgb(45, 45, 45);

            this.MaximizeBox = true;
            this.MinimizeBox = true;

            // 設置 NotifyIcon 圖示
            if (this.Icon != null)
                this.notifyIcon.Icon = this.Icon;
            else
                this.notifyIcon.Icon = SystemIcons.Application;

            this.ShowIcon = false;

            // 定期輪詢服務狀態
            serviceStatusTimer = new System.Windows.Forms.Timer();
            serviceStatusTimer.Interval = 3000;
            serviceStatusTimer.Tick += ServiceStatusTimer_Tick;
            serviceStatusTimer.Start();
        }

        // ──────────────────────────────────────────────────────
        // 服務狀態輪詢
        // ──────────────────────────────────────────────────────

        private void ServiceStatusTimer_Tick(object sender, EventArgs e)
        {
            UpdateServiceStatusUI();
        }

        private void UpdateServiceStatusUI()
        {
            if (this.lblServiceStatus.InvokeRequired)
            {
                this.lblServiceStatus.Invoke(new Action(UpdateServiceStatusUI));
                return;
            }

            bool installed = IsServiceInstalled();
            this.btnInstallService.Text = installed ? "解除安裝服務" : "安裝服務";
            this.chkAutoStart.Enabled = installed;

            if (!installed)
            {
                this.lblServiceStatus.Text = "服務狀態：未安裝";
                this.lblServiceStatus.ForeColor = Color.Gray;
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = false;
                this.startMonitoringToolStripMenuItem.Enabled = false;
                this.stopMonitoringToolStripMenuItem.Enabled = false;
                return;
            }

            try
            {
                using (var sc = new ServiceController(ServiceName))
                {
                    sc.Refresh();
                    switch (sc.Status)
                    {
                        case ServiceControllerStatus.Running:
                            this.lblServiceStatus.Text = "服務狀態：運行中 ✔";
                            this.lblServiceStatus.ForeColor = Color.LightGreen;
                            this.btnStart.Enabled = false;
                            this.btnStop.Enabled = true;
                            this.startMonitoringToolStripMenuItem.Enabled = false;
                            this.stopMonitoringToolStripMenuItem.Enabled = true;
                            break;
                        case ServiceControllerStatus.Stopped:
                            this.lblServiceStatus.Text = "服務狀態：已停止";
                            this.lblServiceStatus.ForeColor = Color.Orange;
                            this.btnStart.Enabled = true;
                            this.btnStop.Enabled = false;
                            this.startMonitoringToolStripMenuItem.Enabled = true;
                            this.stopMonitoringToolStripMenuItem.Enabled = false;
                            break;
                        case ServiceControllerStatus.StartPending:
                        case ServiceControllerStatus.StopPending:
                            this.lblServiceStatus.Text = "服務狀態：切換中...";
                            this.lblServiceStatus.ForeColor = Color.Yellow;
                            this.btnStart.Enabled = false;
                            this.btnStop.Enabled = false;
                            this.startMonitoringToolStripMenuItem.Enabled = false;
                            this.stopMonitoringToolStripMenuItem.Enabled = false;
                            break;
                        default:
                            this.lblServiceStatus.Text = $"服務狀態：{sc.Status}";
                            this.lblServiceStatus.ForeColor = Color.Yellow;
                            this.btnStart.Enabled = false;
                            this.btnStop.Enabled = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblServiceStatus.Text = $"服務狀態：查詢失敗 ({ex.Message})";
                this.lblServiceStatus.ForeColor = Color.Red;
            }
        }

        private bool IsServiceInstalled()
        {
            try
            {
                using (var sc = new ServiceController(ServiceName))
                {
                    var _ = sc.Status; // throws InvalidOperationException if not found
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // ──────────────────────────────────────────────────────
        // 表單載入
        // ──────────────────────────────────────────────────────

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = AppSettings.Load();
            this.txtAccount.Text = string.IsNullOrEmpty(settings.Username) ? "ncut" : settings.Username;
            this.txtPassword.Text = string.IsNullOrEmpty(settings.Password) ? "ncut" : settings.Password;

            // 從服務啟動類型讀取自動啟動狀態（不觸發事件）
            this.chkAutoStart.CheckedChanged -= chkAutoStart_CheckedChanged;
            this.chkAutoStart.Checked = IsServiceAutoStart();
            this.chkAutoStart.Enabled = IsServiceInstalled();
            this.chkAutoStart.CheckedChanged += chkAutoStart_CheckedChanged;

            // 程式開機後台啟動狀態
            this.chkStartMinimized.CheckedChanged -= chkStartMinimized_CheckedChanged;
            this.chkStartMinimized.Checked = IsGUIAutoStartEnabled();
            this.chkStartMinimized.CheckedChanged += chkStartMinimized_CheckedChanged;

            if (Program.StartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
                this.notifyIcon.BalloonTipText = "程式已在後台啟動";
                this.notifyIcon.ShowBalloonTip(2000);
            }

            LogMessage("NCUT校園網自動登入V2");
            LogMessage("by sangege & AI LIFE\n");
            LogMessage("https://github.com/apple050620312/NCUT-Internet-Auto-Login\n");
            LogMessage($"使用的帳號: {this.txtAccount.Text}");
            LogMessage($"使用的密碼: {(string.IsNullOrEmpty(this.txtPassword.Text) ? "" : "******")}\n");

            if (Program.StartMinimized)
                LogMessage($"{Program.GetTimestamp()} 程式以後台模式啟動\n");

            UpdateServiceStatusUI();

            // 背景檢查更新（不阻塞 UI）
            CheckForUpdatesBackground();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
                this.notifyIcon.BalloonTipText = "程式已最小化到系統托盤";
                this.notifyIcon.ShowBalloonTip(2000);
            }
        }

        // ──────────────────────────────────────────────────────
        // 儲存設定
        // ──────────────────────────────────────────────────────

        private void SaveSettings()
        {
            Program.Account = this.txtAccount.Text;
            Program.Password = this.txtPassword.Text;
            settings.Username = Program.Account;
            settings.Password = Program.Password;
            settings.Save();
        }

        // ──────────────────────────────────────────────────────
        // 服務安裝 / 解除安裝
        // ──────────────────────────────────────────────────────

        private void btnInstallService_Click(object sender, EventArgs e)
        {
            if (IsServiceInstalled())
                UninstallService();
            else
                InstallService();
        }

        private void InstallService()
        {
            SaveSettings();

            string workerDllPath = Path.Combine(Application.StartupPath, "NCUT-Internet-Auto-Login.Worker.dll");
            if (!File.Exists(workerDllPath))
            {
                MessageBox.Show(
                    $"找不到 Worker 執行檔:\n{workerDllPath}\n\n請確認 NCUT-Internet-Auto-Login.Worker.dll 與本程式位於同一目錄。",
                    "安裝失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string dotnetPath = GetDotnetExePath();
            if (dotnetPath == null)
            {
                MessageBox.Show(
                    "找不到 .NET Runtime (dotnet.exe)。\n\n" +
                    "請先安裝 .NET 9 Runtime：\nhttps://dotnet.microsoft.com/download/dotnet/9.0",
                    "安裝失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Combine both sc.exe calls into one elevated cmd.exe invocation (single UAC prompt).
                // binPath= value: "dotnet.exe" "worker.dll"  (inner quotes escaped as \")
                string binPath = $"\\\"{dotnetPath}\\\" \\\"{workerDllPath}\\\"";
                string createCmd = $"sc.exe create \"{ServiceName}\" binPath= \"{binPath}\" start= auto DisplayName= \"{ServiceName}\"";
                string descCmd   = $"sc.exe description \"{ServiceName}\" \"NCUT 校園網路自動登入服務，開機自動啟動登入，無需使用者登入\"";
                RunElevated("cmd.exe", $"/c {createCmd} && {descCmd}");

                LogMessage($"{Program.GetTimestamp()} 服務安裝成功，已設定為開機自動啟動");

                // 同步 checkbox 狀態（不觸發事件）
                this.chkAutoStart.CheckedChanged -= chkAutoStart_CheckedChanged;
                this.chkAutoStart.Checked = true;
                this.chkAutoStart.Enabled = true;
                this.chkAutoStart.CheckedChanged += chkAutoStart_CheckedChanged;

                UpdateServiceStatusUI();
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 安裝服務失敗: {ex.Message}");
                MessageBox.Show($"安裝服務失敗:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UninstallService()
        {
            if (MessageBox.Show("確定要解除安裝服務嗎？服務將停止並從系統中移除。",
                "確認解除安裝", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                // Stop then delete in one elevated cmd.exe call (single UAC prompt).
                // Use & (not &&) so delete runs even when the service was already stopped.
                string stopCmd   = $"sc.exe stop \"{ServiceName}\"";
                string waitCmd   = "timeout /t 5 /nobreak > nul";
                string deleteCmd = $"sc.exe delete \"{ServiceName}\"";
                RunElevated("cmd.exe", $"/c {stopCmd} & {waitCmd} & {deleteCmd}", timeoutMs: 30000);

                LogMessage($"{Program.GetTimestamp()} 服務已解除安裝");

                this.chkAutoStart.CheckedChanged -= chkAutoStart_CheckedChanged;
                this.chkAutoStart.Checked = false;
                this.chkAutoStart.Enabled = false;
                this.chkAutoStart.CheckedChanged += chkAutoStart_CheckedChanged;

                UpdateServiceStatusUI();
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 解除安裝服務失敗: {ex.Message}");
                MessageBox.Show($"解除安裝服務失敗:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Searches common locations and PATH for dotnet.exe.
        /// Returns null if not found.
        /// </summary>
        private static string GetDotnetExePath()
        {
            // Check the standard 64-bit and 32-bit Program Files paths first
            string pf64 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            string pf32 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            foreach (string dir in new[] { pf64, pf32 })
            {
                string candidate = Path.Combine(dir, "dotnet", "dotnet.exe");
                if (File.Exists(candidate))
                    return candidate;
            }

            // Fall back to PATH
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            foreach (string dir in pathEnv.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                string candidate = Path.Combine(dir.Trim(), "dotnet.exe");
                if (File.Exists(candidate))
                    return candidate;
            }

            return null;
        }

        // ──────────────────────────────────────────────────────
        // 服務啟動 / 停止
        // ──────────────────────────────────────────────────────

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartService();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopService();
        }

        private void StartService()
        {
            SaveSettings();
            try
            {
                using (var sc = new ServiceController(ServiceName))
                {
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running,
                            TimeSpan.FromSeconds(ServiceOperationTimeoutSeconds));
                        LogMessage($"{Program.GetTimestamp()} 服務已啟動");
                    }
                }
            }
            catch (System.TimeoutException)
            {
                string msg = "服務未能在預期時間內啟動，請稍後重試或檢查事件檢視器。";
                LogMessage($"{Program.GetTimestamp()} {msg}");
                MessageBox.Show(msg, "啟動逾時", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 啟動服務失敗: {ex.Message}");
                MessageBox.Show($"啟動服務失敗:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateServiceStatusUI();
            this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
            this.notifyIcon.BalloonTipText = "登入服務已啟動";
            this.notifyIcon.ShowBalloonTip(2000);
        }

        private void StopService()
        {
            try
            {
                using (var sc = new ServiceController(ServiceName))
                {
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped,
                            TimeSpan.FromSeconds(ServiceOperationTimeoutSeconds));
                        LogMessage($"{Program.GetTimestamp()} 服務已停止");
                    }
                }
            }
            catch (System.TimeoutException)
            {
                string msg = "服務未能在預期時間內停止，請稍後重試或檢查事件檢視器。";
                LogMessage($"{Program.GetTimestamp()} {msg}");
                MessageBox.Show(msg, "停止逾時", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 停止服務失敗: {ex.Message}");
                MessageBox.Show($"停止服務失敗:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateServiceStatusUI();
            this.notifyIcon.BalloonTipTitle = "NCUT Auto Login";
            this.notifyIcon.BalloonTipText = "登入服務已停止";
            this.notifyIcon.ShowBalloonTip(2000);
        }

        // ──────────────────────────────────────────────────────
        // 日誌
        // ──────────────────────────────────────────────────────

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

        // ──────────────────────────────────────────────────────
        // 系統托盤功能
        // ──────────────────────────────────────────────────────

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
            StartService();
        }

        private void stopMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopService();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serviceStatusTimer != null)
            {
                serviceStatusTimer.Stop();
                serviceStatusTimer.Dispose();
            }

            this.notifyIcon.Visible = false;
            Application.Exit();
        }

        private void ShowWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        // ──────────────────────────────────────────────────────
        // 開機自啟動設定
        // ──────────────────────────────────────────────────────

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ConfigureServiceStartType(this.chkAutoStart.Checked);
                LogMessage(this.chkAutoStart.Checked
                    ? $"{Program.GetTimestamp()} 已設定服務為開機自動啟動（開機即登入，無需用戶登入）"
                    : $"{Program.GetTimestamp()} 已取消服務開機自動啟動");
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 設定服務啟動類型失敗: {ex.Message}");
                // 還原 checkbox 狀態
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
                    EnableGUIAutoStart();
                    LogMessage($"{Program.GetTimestamp()} 已啟用程式開機後台啟動");
                }
                else
                {
                    DisableGUIAutoStart();
                    LogMessage($"{Program.GetTimestamp()} 已停用程式開機後台啟動");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 設定程式開機啟動時發生錯誤: {ex.Message}");
                this.chkStartMinimized.CheckedChanged -= chkStartMinimized_CheckedChanged;
                this.chkStartMinimized.Checked = !this.chkStartMinimized.Checked;
                this.chkStartMinimized.CheckedChanged += chkStartMinimized_CheckedChanged;
            }
        }

        /// <summary>
        /// 讀取 Windows Service 啟動類型是否為 Automatic（2）
        /// </summary>
        private bool IsServiceAutoStart()
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Services\{ServiceName}"))
                {
                    if (key != null)
                    {
                        var startValue = key.GetValue("Start");
                        return startValue != null && (int)startValue == 2;
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 檢查 GUI 程式是否已設定開機後台啟動（HKCU Run 機碼）
        /// </summary>
        private bool IsGUIAutoStartEnabled()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, false))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(AppName) ?? key.GetValue(LegacyAppName);
                        return value != null;
                    }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 使用 sc.exe（需管理員權限）設定服務啟動類型
        /// </summary>
        private void ConfigureServiceStartType(bool autoStart)
        {
            string startType = autoStart ? "auto" : "demand";
            RunElevated("sc.exe", $"config \"{ServiceName}\" start= {startType}");
        }

        /// <summary>
        /// 在 HKCU Run 寫入 GUI 開機後台啟動項目
        /// </summary>
        private void EnableGUIAutoStart()
        {
            string commandLine = $"\"{Application.ExecutablePath}\" -minimized";
            using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    key.SetValue(AppName, commandLine);
                    if (key.GetValue(LegacyAppName) != null)
                        key.DeleteValue(LegacyAppName);
                }
            }
        }

        /// <summary>
        /// 移除 HKCU Run 中的 GUI 開機啟動項目
        /// </summary>
        private void DisableGUIAutoStart()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, true))
            {
                if (key != null)
                {
                    if (key.GetValue(AppName) != null)
                        key.DeleteValue(AppName);
                    if (key.GetValue(LegacyAppName) != null)
                        key.DeleteValue(LegacyAppName);
                }
            }
        }

        /// <summary>
        /// 以管理員身份執行命令（觸發 UAC 提示），若程序無法啟動或在逾時內未完成則拋出例外
        /// </summary>
        private static void RunElevated(string fileName, string arguments, int timeoutMs = 10000)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var proc = Process.Start(psi))
            {
                if (proc == null)
                    throw new InvalidOperationException($"無法啟動程序: {fileName}");

                if (!proc.WaitForExit(timeoutMs))
                    throw new System.TimeoutException(
                        $"程序執行逾時 ({timeoutMs / 1000} 秒): {fileName} {arguments}");

                if (proc.ExitCode != 0)
                    throw new InvalidOperationException(
                        $"命令執行失敗 (結束代碼 {proc.ExitCode}): {fileName} {arguments}");
            }
        }

        // ──────────────────────────────────────────────────────
        // OTA 更新
        // ──────────────────────────────────────────────────────

        private async void CheckForUpdatesBackground()
        {
            try
            {
                var info = await UpdateChecker.CheckAsync();
                if (!info.IsUpdateAvailable) return;

                if (this.InvokeRequired)
                    this.Invoke(new Action(() => PromptForUpdate(info)));
                else
                    PromptForUpdate(info);
            }
            catch { /* 更新檢查失敗時靜默忽略 */ }
        }

        private void checkForUpdatesMenuItem_Click(object sender, EventArgs e)
        {
            LogMessage($"{Program.GetTimestamp()} 正在檢查更新...");
            CheckForUpdatesManual();
        }

        private async void CheckForUpdatesManual()
        {
            try
            {
                var info = await UpdateChecker.CheckAsync();
                if (info.IsUpdateAvailable)
                {
                    PromptForUpdate(info);
                }
                else
                {
                    string current = $"v{UpdateChecker.CurrentVersion.ToString(3)}";
                    LogMessage($"{Program.GetTimestamp()} 目前已是最新版本 ({current})");
                    MessageBox.Show(
                        $"目前已是最新版本 ({current})",
                        "檢查更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 檢查更新失敗: {ex.Message}");
                MessageBox.Show($"檢查更新失敗:\n{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PromptForUpdate(UpdateInfo info)
        {
            string msg = $"發現新版本 {info.TagName}！\n\n是否要下載並安裝更新？";
            if (MessageBox.Show(msg, "有可用更新", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                DownloadAndInstallUpdate(info);
        }

        private async void DownloadAndInstallUpdate(UpdateInfo info)
        {
            if (string.IsNullOrEmpty(info.DownloadUrl))
            {
                Process.Start(UpdateChecker.ReleasesUrl);
                return;
            }

            LogMessage($"{Program.GetTimestamp()} 正在下載更新 {info.TagName}...");
            try
            {
                await UpdateChecker.DownloadAndInstallAsync(info.DownloadUrl);
                LogMessage($"{Program.GetTimestamp()} 安裝程式已啟動，請依照提示完成安裝");
            }
            catch (Exception ex)
            {
                LogMessage($"{Program.GetTimestamp()} 下載更新失敗: {ex.Message}");
                MessageBox.Show(
                    $"下載更新失敗:\n{ex.Message}\n\n請手動前往 GitHub 下載。",
                    "下載失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.Start(UpdateChecker.ReleasesUrl);
            }
        }

        // ──────────────────────────────────────────────────────
        // 表單關閉
        // ──────────────────────────────────────────────────────

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
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
                if (serviceStatusTimer != null)
                {
                    serviceStatusTimer.Stop();
                    serviceStatusTimer.Dispose();
                }
                base.OnFormClosing(e);
            }
        }
    }
}
