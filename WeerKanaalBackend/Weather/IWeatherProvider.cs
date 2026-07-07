using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Weather;

public interface IWeatherProvider
{
    Task<CityWeather[]> GetForecastsAsync();
}