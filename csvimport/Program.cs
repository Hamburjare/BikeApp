using csvimport;

using Microsoft.Extensions.Hosting; // Requires NuGet package

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddTransient<ImportData>(); })
    .Build();

var my = host.Services.GetRequiredService<ImportData>();
await my.ExecuteAsync();
