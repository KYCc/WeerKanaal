using Microsoft.Playwright;
using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Video;

public class PlaywrightRecorder
{
    private List<CityWeather> _forecasts;
    
    public PlaywrightRecorder(List<CityWeather> forecasts)
    {
        _forecasts = forecasts;
    }

    public async void RecordVideo()
    {
        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = true,
            Args = ["--allow-file-access-from-files"],
        });
        var context = await browser.NewContextAsync(new()
        {
            ViewportSize   = new() { Width = 1080, Height = 1920 },
            RecordVideoDir = "Video",
            RecordVideoSize = new() { Width = 1080, Height = 1920 },
        });
    }
}