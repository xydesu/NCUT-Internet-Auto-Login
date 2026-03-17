using NCUT_Internet_Auto_Login.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "NCUT Auto Login Service";
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// 提高服務的執行優先級，確保早於其他服務獲得 CPU 執行權
try
{
    using var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
    currentProcess.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
    System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
}
catch
{
    // 忽略權限不足或其他例外
}

host.Run();
