using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Tests;

public class WeatherReportTests
{
    private static WeatherReport Report(int weatherCode, double tempMax = 15, double tempMin = 5, double maxWindSpeed = 10)
        => new(tempMin, tempMax, weatherCode, maxWindSpeed);

    [Theory]
    [InlineData(0, WeatherIcon.Sunny)]
    [InlineData(1, WeatherIcon.Sunny)]
    [InlineData(2, WeatherIcon.CloudySunny)]
    [InlineData(3, WeatherIcon.Cloudy)]
    [InlineData(45, WeatherIcon.Foggy)]
    [InlineData(48, WeatherIcon.Foggy)]
    [InlineData(51, WeatherIcon.Rainy)]
    [InlineData(53, WeatherIcon.Rainy)]
    [InlineData(55, WeatherIcon.Rainy)]
    [InlineData(61, WeatherIcon.Rainy)]
    [InlineData(63, WeatherIcon.Rainy)]
    [InlineData(65, WeatherIcon.Rainy)]
    [InlineData(80, WeatherIcon.SunnyRainy)]
    [InlineData(81, WeatherIcon.SunnyRainy)]
    [InlineData(82, WeatherIcon.SunnyRainy)]
    [InlineData(71, WeatherIcon.Snowy)]
    [InlineData(73, WeatherIcon.Snowy)]
    [InlineData(75, WeatherIcon.Snowy)]
    [InlineData(77, WeatherIcon.Snowy)]
    [InlineData(85, WeatherIcon.Snowy)]
    [InlineData(86, WeatherIcon.Snowy)]
    [InlineData(56, WeatherIcon.Frosty)]
    [InlineData(57, WeatherIcon.Frosty)]
    [InlineData(66, WeatherIcon.Frosty)]
    [InlineData(67, WeatherIcon.Frosty)]
    [InlineData(95, WeatherIcon.Stormy)]
    [InlineData(96, WeatherIcon.Stormy)]
    [InlineData(99, WeatherIcon.Stormy)]
    public void MapsWeatherCodeToIcon(int weatherCode, WeatherIcon expected)
    {
        Assert.Equal(expected, Report(weatherCode).Icon);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(40)]
    [InlineData(100)]
    [InlineData(-1)]
    public void UnknownCodeFallsBackToCloudySunny(int weatherCode)
    {
        Assert.Equal(WeatherIcon.CloudySunny, Report(weatherCode).Icon);
    }

    [Theory]
    [InlineData(0, 28.0, WeatherIcon.Hottie)]
    [InlineData(1, 28.0, WeatherIcon.Hottie)]
    [InlineData(0, 27.9, WeatherIcon.Sunny)]
    [InlineData(1, 27.9, WeatherIcon.Sunny)]
    [InlineData(2, 35.0, WeatherIcon.CloudySunny)] // Hottie only applies to clear-sky codes
    public void HottieRequiresClearSkyAndAtLeast28Degrees(int weatherCode, double tempMax, WeatherIcon expected)
    {
        Assert.Equal(expected, Report(weatherCode, tempMax).Icon);
    }

    [Theory]
    [InlineData(50.0, false)]
    [InlineData(50.1, true)]
    public void WindWarningTriggersAbove50(double maxWindSpeed, bool expected)
    {
        Assert.Equal(expected, Report(3, maxWindSpeed: maxWindSpeed).WindWarning);
    }

    [Theory]
    [InlineData(19.4, 19)]
    [InlineData(19.5, 20)]
    [InlineData(-0.5, -1)]
    [InlineData(-0.4, 0)]
    public void TemperaturesRoundAwayFromZero(double temp, int expected)
    {
        var report = new WeatherReport(temp, temp, 3, 10);
        Assert.Equal(expected, report.TempMin);
        Assert.Equal(expected, report.TempMax);
    }
}
