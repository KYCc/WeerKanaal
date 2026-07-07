using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Weather;

public class OpenMeteoWeatherProvider : IWeatherProvider
{
    public Task<CityWeather[]> GetForecastsAsync()
    {
        var cities = Cities.AllCities;
        var url = "https://api.open-meteo.com/v1/forecast?latitude=" 
                  + string.Join(",", Cities.Latitudes) 
                  + "&longitude=" + string.Join(",", Cities.Longitudes) 
                  + "&daily=temperature_2m_max,temperature_2m_min,weather_code"
                  + "&timezone=Europe/Brussels"
                  + "&start_date=" + DateTime.Today.AddDays(1).ToString("yyyy-MM-dd")
                  + "&end_date=" + DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");

        CityWeather[] forecasts  = new CityWeather[cities.Count];
        for (int i = 0; i < cities.Count; i++)
        {
            forecasts[i] = new CityWeather(Cities.Names[i], new WeatherReport(0, 20, WeatherIcon.CloudySunny));
        }
        
        return forecasts;
    }
}