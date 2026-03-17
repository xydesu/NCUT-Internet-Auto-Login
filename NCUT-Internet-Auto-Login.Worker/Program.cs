using NCUT_Internet_Auto_Login.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "NCUT Auto Login Service";
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
