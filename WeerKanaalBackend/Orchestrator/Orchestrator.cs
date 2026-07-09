using Microsoft.Extensions.Logging;
using WeerKanaalBackend.Video;
using WeerKanaalBackend.Weather;

namespace WeerKanaalBackend.Orchestrator;

public class Orchestrator
{
    private readonly ILogger<Orchestrator> _logger;
    private readonly IWeatherProvider _weatherProvider;

    public Orchestrator(ILogger<Orchestrator> logger, IWeatherProvider weatherProvider)
    {
        _logger = logger;
        _weatherProvider = weatherProvider;
    }
    
    public async Task RunAsync()
    {
        _logger.LogInformation("Daily run started");
        _logger.LogInformation("Fetching tomorrow's forecast");
        var forecasts = await _weatherProvider.GetAllForecastsOfCityListAsync();
        _logger.LogInformation("Retrieved {Count} city forecasts", forecasts.Count);

        PlaywrightRecorder recorder = new(forecasts);
        await recorder.RecordVideo();
        
        _logger.LogInformation("Daily run completed");
    }
}
