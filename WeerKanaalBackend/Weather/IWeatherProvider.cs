using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Weather;

public interface IWeatherProvider
{
    Task<List<CityWeather>> GetAllForecastsOfCityListAsync();
}