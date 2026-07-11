using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeerKanaalBackend.Orchestrator;
using WeerKanaalBackend.Weather;

// Load local .env for development (a no-op in CI, where vars come from the real environment).
if (File.Exists(".env")) DotNetEnv.Env.Load();

var builder = Host.CreateApplicationBuilder(args);

// HttpClient factory for the weather provider.
builder.Services.AddHttpClient<IWeatherProvider, OpenMeteoWeatherProvider>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/");
    client.Timeout = Timeout.InfiniteTimeSpan;
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("User-Agent", "weer-kanaal-belgie");
})
.AddStandardResilienceHandler();

builder.Services.AddScoped<Orchestrator>();

var host = builder.Build();

using var scope = host.Services.CreateScope();
var orchestrator = scope.ServiceProvider.GetRequiredService<Orchestrator>();
await orchestrator.RunAsync();
