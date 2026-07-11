using System.Globalization;
using Microsoft.Extensions.Logging;
using WeerKanaalBackend.Instagram;
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

        _logger.LogInformation("Now recording video...");
        PlaywrightRecorder recorder = new(forecasts);
        var videoPath = await recorder.RecordVideo();
        _logger.LogInformation("Video recorded");
        
        _logger.LogInformation("Now converting to mp4...");
        var musicPath = MusicPicker.PickRandom();
        _logger.LogInformation("Selected track: {Track}", musicPath);
        var encoder = new VideoEncoder();
        var mp4Path = await encoder.ToMp4Async(videoPath, musicPath);
        _logger.LogInformation("Encoded MP4: {Path}", mp4Path);

        _logger.LogInformation("Now uploading to Instagram...");
        var publisher = new InstagramPublisher();
        var tomorrow = DateTime.Today.AddDays(1).ToString("d MMMM yyyy", new CultureInfo("nl-BE"));
        var caption = $"Het weer voor morgen, {tomorrow}. #weer #weerbelgië #weerkanaal #belgie";
        var mediaId = await publisher.PublishReelAsync(mp4Path, caption);
        _logger.LogInformation("Published reel: {MediaId}", mediaId);

        _logger.LogInformation("Daily run completed");
    }
}
