namespace WeerKanaalBackend.util;

public record WeatherReport(
    int TempMax,
    int TempMin,
    WeatherIcon Icon);

public record CityWeather(string Name, WeatherReport Report);