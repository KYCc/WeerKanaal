using System.Text.Json.Serialization;

namespace WeerKanaalBackend.util;

public class OpenMeteoData
{
    [JsonPropertyName("latitude")] 
    public double Latitude { get; set; }
    [JsonPropertyName("longitude")] 
    public double Longitude { get; set; }
    [JsonPropertyName("daily")] 
    public OpenMeteoDaily? Daily { get; set; }
}

public class OpenMeteoDaily
{
    [JsonPropertyName("temperature_2m_max")]
    public List<double>? TempMax { get; set; }
    [JsonPropertyName("temperature_2m_min")]
    public List<double>? TempMin { get; set; }
    [JsonPropertyName("weather_code")] 
    public List<int>? WeatherCode { get; set; }
    [JsonPropertyName("wind_speed_10m_max")]
    public List<double>? WindSpeedMax { get; set; }
}
    