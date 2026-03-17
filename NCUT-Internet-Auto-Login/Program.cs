using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace NCUT_Internet_Auto_Login
{
    internal static class Program
    {
        public static string Account { get; set; } = "ncut";
        public static string Password { get; set; } = "ncut";
        public static bool StartMinimized { get; private set; } = false;

        // 共用單一 HttpClient 實例以避免 Socket 耗盡
        private static readonly HttpClientHandler handler = new HttpClientHandler
        {
            AllowAutoRedirect = false
        };
        private static readonly HttpClient httpClient = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 提高程式的執行優先級，確保比其他開機啟動的應用程式更早取得 CPU 執行權
            try
            {
                using (Process currentProcess = Process.GetCurrentProcess())
                {
                    currentProcess.PriorityClass = ProcessPriorityClass.High;
                }
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
            }
            catch
            {
                // 忽略權限不足或其他例外
            }

            // 檢查命令列參數
            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "-minimized" || arg.ToLower() == "/minimized")
                    {
                        StartMinimized = true;
                        break;
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string GetTimestamp()
        {
            return DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]");
        }
    }
}
