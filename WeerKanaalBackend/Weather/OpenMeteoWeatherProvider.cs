using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WeerKanaalBackend.util;

namespace WeerKanaalBackend.Weather;

public class OpenMeteoWeatherProvider : IWeatherProvider
{
    private const int DaytimeStartHour = 7;
    private const int DaytimeEndHour = 21;

    private readonly ILogger<OpenMeteoWeatherProvider> _logger;
    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherProvider(ILogger<OpenMeteoWeatherProvider> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<List<CityWeather>> GetAllForecastsOfCityListAsync()
    {
        var tomorrowString = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
        var latitudes = Cities.Latitudes.Select(lat => lat.ToString(CultureInfo.InvariantCulture));
        var longitudes = Cities.Longitudes.Select(lon => lon.ToString(CultureInfo.InvariantCulture));
        var query = "v1/forecast?latitude="
                  + string.Join(",", latitudes)
                  + "&longitude=" + string.Join(",", longitudes)
                  + "&daily=temperature_2m_max,temperature_2m_min,wind_speed_10m_max"
                  + "&hourly=weather_code"
                  + "&timezone=Europe/Brussels"
                  + "&start_date=" + tomorrowString
                  + "&end_date=" + tomorrowString;
        
        List<OpenMeteoData>? openMeteoData;
        try
        {
            var response = await _httpClient.GetAsync(query);
            response.EnsureSuccessStatusCode();
            openMeteoData = await response.Content.ReadFromJsonAsync<List<OpenMeteoData>>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to fetch weather from OpenMeteo");
            throw;
        }

        var result = new List<CityWeather>();
        if (openMeteoData == null)
        {
            _logger.LogError("OpenMeteo returned null data");
            return result;
        }

        for (var i = 0; i < openMeteoData.Count && i < Cities.AllCities.Count; i++)
        {
            var data = openMeteoData[i];
            var daytimeCodes = data.Hourly?.WeatherCode?
                .Skip(DaytimeStartHour)
                .Take(DaytimeEndHour - DaytimeStartHour + 1)
                .ToList();
            if (data.Daily?.TempMin is null ||
                data.Daily.TempMax is null ||
                data.Daily.WindSpeedMax is null ||
                daytimeCodes is null || daytimeCodes.Count == 0) continue;

            result.Add(new CityWeather(
                Cities.AllCities[i].Name,
                new WeatherReport(
                    data.Daily.TempMin[0],
                    data.Daily.TempMax[0],
                    daytimeCodes.Max(),
                    data.Daily.WindSpeedMax[0])));
        }
        return result;
    }
}
