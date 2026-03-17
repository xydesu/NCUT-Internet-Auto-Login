using System;
using System.Threading;
using System.Threading.Tasks;

namespace NCUT_Internet_Auto_Login
{
    public class WorkerRunner : IDisposable
    {
        private CancellationTokenSource _cts;
        private bool _isRunning = false;
        private AutoLoginService _service;

        public event Action<string> OnLogMessage;
        public event Action<bool> OnStatusChanged;

        public bool IsRunning => _isRunning;

        public async Task StartAsync()
        {
            if (_isRunning) return;
            
            var settings = AppSettings.Load();
            _service = new AutoLoginService(settings.Username, settings.Password);
            _service.OnLogMessage += (msg) => OnLogMessage?.Invoke(msg);
            _service.OnStatusChanged += (status) => OnStatusChanged?.Invoke(status);

            _cts = new CancellationTokenSource();
            _isRunning = true;

            _ = Task.Run(async () =>
            {
                try
                {
                    await _service.StartMonitoringAsync(_cts.Token);
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    OnLogMessage?.Invoke($"[ERROR] 背景監控異常中斷: {ex.Message}");
                }
                finally
                {
                    _isRunning = false;
                    OnStatusChanged?.Invoke(_isRunning);
                }
            }, _cts.Token);
        }

        public async Task StopAsync()
        {
            _cts?.Cancel();
            _isRunning = false;
            await Task.Delay(500); 
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _service?.Dispose();
        }
    }
}