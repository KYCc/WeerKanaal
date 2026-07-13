using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Video;

public static class MusicPicker
{
    private const string DefaultDir = "Music";
    private const string HotDir = "Music/hot";
    private const string ColdDir = "Music/cold";
    private const string GreyDir = "Music/grey";
    private const string RainDir = "Music/rain";
    private const string StormDir = "Music/storm";
    private static readonly string[] AudioExtensions = [".mp3", ".wav", ".m4a", ".ogg", ".aac"];

    public static string PickRandomFromFolder(string folder = DefaultDir)
    {
        var tracks = EnumerateTracks(folder).ToArray();
        
        if (tracks.Length == 0 && folder != DefaultDir)
            tracks = EnumerateTracks(DefaultDir, recursive: true).ToArray();

        if (tracks.Length == 0)
            throw new InvalidOperationException($"No audio tracks found in '{folder}'.");

        return tracks[Random.Shared.Next(tracks.Length)];
    }

    public static string PickRandomByWeather(List<CityWeather> forecasts)
    {
        return PickRandomFromFolder(PickFolder(forecasts));
    }

    public static string PickVibe(List<CityWeather> forecasts) => PickFolder(forecasts) switch
    {
        StormDir => "storm",
        ColdDir => "cold",
        RainDir => "rain",
        GreyDir => "grey",
        _ => "hot"
    };

    private static string PickFolder(List<CityWeather> forecasts)
    {
        if (forecasts.Count == 0) return DefaultDir;

        var folders = forecasts.Select(FolderFor).ToList();
        
        if (folders.Contains(StormDir)) return StormDir;
        if (folders.Contains(ColdDir)) return ColdDir;
        
        return folders
            .GroupBy(f => f)
            .OrderByDescending(g => g.Count())
            .ThenByDescending(g => Severity(g.Key))
            .First().Key;
    }

    private static string FolderFor(CityWeather city)
    {
        if (city.Report.WindWarning) return StormDir;

        return city.Report.Icon switch
        {
            WeatherIcon.Stormy or WeatherIcon.Windy => StormDir,
            WeatherIcon.Snowy or WeatherIcon.Frosty => ColdDir,
            WeatherIcon.Rainy or WeatherIcon.SunnyRainy => RainDir,
            WeatherIcon.Cloudy or WeatherIcon.Foggy => GreyDir,
            _ => HotDir
        };
    }

    private static int Severity(string folder) => folder switch
    {
        StormDir => 4,
        ColdDir => 3,
        RainDir => 2,
        GreyDir => 1,
        _ => 0
    };

    private static IEnumerable<string> EnumerateTracks(string folder, bool recursive = false) =>
        Directory.Exists(folder)
            ? Directory.EnumerateFiles(folder, "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                .Where(f => AudioExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
            : [];
}
