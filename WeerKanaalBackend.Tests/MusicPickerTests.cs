using WeerKanaalBackend.util;
using WeerKanaalBackend.Video;

namespace WeerKanaalBackend.Tests;

public class MusicPickerTests
{
    private static CityWeather City(int weatherCode, double tempMax = 15, double maxWindSpeed = 10)
        => new("Testcity", new WeatherReport(5, tempMax, weatherCode, maxWindSpeed));

    private static CityWeather Stormy() => City(95);
    private static CityWeather Snowy() => City(71);
    private static CityWeather Rainy() => City(61);
    private static CityWeather Cloudy() => City(3);
    private static CityWeather Sunny() => City(0);

    [Fact]
    public void EmptyForecastDefaultsToHot()
    {
        Assert.Equal("hot", MusicPicker.PickVibe([]));
    }

    [Theory]
    [InlineData(95, "storm")]
    [InlineData(71, "cold")]  // snow
    [InlineData(56, "cold")]  // freezing drizzle -> Frosty
    [InlineData(61, "rain")]
    [InlineData(80, "rain")]  // showers -> SunnyRainy
    [InlineData(3, "grey")]   // overcast
    [InlineData(45, "grey")]  // fog
    [InlineData(0, "hot")]    // clear sky
    [InlineData(2, "hot")]    // CloudySunny counts as hot
    public void SingleCityMapsToVibe(int weatherCode, string expected)
    {
        Assert.Equal(expected, MusicPicker.PickVibe([City(weatherCode)]));
    }

    [Fact]
    public void WindWarningForcesStorm()
    {
        Assert.Equal("storm", MusicPicker.PickVibe([City(0, maxWindSpeed: 60)]));
    }

    [Fact]
    public void WarmOvercastDayCountsAsHot()
    {
        Assert.Equal("hot", MusicPicker.PickVibe([City(3, tempMax: 25)]));
        Assert.Equal("hot", MusicPicker.PickVibe([City(45, tempMax: 25)]));
        Assert.Equal("grey", MusicPicker.PickVibe([City(3, tempMax: 24.4)]));
    }

    [Fact]
    public void SingleStormyCityOverridesMajority()
    {
        Assert.Equal("storm", MusicPicker.PickVibe([Rainy(), Rainy(), Rainy(), Stormy()]));
    }

    [Fact]
    public void SingleColdCityOverridesMajorityExceptStorm()
    {
        Assert.Equal("cold", MusicPicker.PickVibe([Rainy(), Rainy(), Rainy(), Snowy()]));
        Assert.Equal("storm", MusicPicker.PickVibe([Snowy(), Snowy(), Stormy()]));
    }

    [Fact]
    public void MajorityWinsOtherwise()
    {
        Assert.Equal("rain", MusicPicker.PickVibe([Rainy(), Rainy(), Cloudy()]));
        Assert.Equal("grey", MusicPicker.PickVibe([Cloudy(), Cloudy(), Rainy()]));
    }

    [Fact]
    public void TieGoesToMostSevereVibe()
    {
        Assert.Equal("rain", MusicPicker.PickVibe([Rainy(), Cloudy()]));
        Assert.Equal("hot", MusicPicker.PickVibe([Sunny(), Cloudy()]));
        Assert.Equal("rain", MusicPicker.PickVibe([Rainy(), Sunny()]));
    }
}
