namespace WeerKanaalBackend.util;

public class WeatherReport
{
    public int TempMin { get; private set; }
    public int TempMax { get; private set; }
    public WeatherIcon Icon { get; private set; }
    public bool WindWarning { get; private set; }

    public WeatherReport(double tempMin, double tempMax, int weatherCode, double maxWindSpeed)
    {
        this.TempMin = (int)Math.Round(tempMin, MidpointRounding.AwayFromZero);
        this.TempMax = (int)Math.Round(tempMax, MidpointRounding.AwayFromZero);
        this.Icon = MapDataToIcon(weatherCode, tempMax);
        this.WindWarning = maxWindSpeed > 50.0;
    }

    private WeatherIcon MapDataToIcon(int weatherCode, double tempMax)
    {
        if (weatherCode is 0 or 1 && tempMax >= 28.0) return WeatherIcon.Hottie;

        return weatherCode switch
        {
            0 or 1 => WeatherIcon.Sunny,
            2 => WeatherIcon.CloudySunny,
            3 => WeatherIcon.Cloudy,
            45 or 48 => WeatherIcon.Foggy,
            51 or 53 or 55 or 61 or 63 or 65 => WeatherIcon.Rainy,
            80 or 81 or 82 => WeatherIcon.SunnyRainy,
            71 or 73 or 75 or 77 or 85 or 86 => WeatherIcon.Snowy,
            56 or 57 or 66 or 67 => WeatherIcon.Frosty,
            95 or 96 or 99 => WeatherIcon.Stormy,
            _ => WeatherIcon.CloudySunny
        };
    }
}

public record CityWeather(string Name, WeatherReport Report);