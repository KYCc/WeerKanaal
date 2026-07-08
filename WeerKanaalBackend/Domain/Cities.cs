namespace WeerKanaalBackend.util;

public record Coords(double Latitude, double Longitude);
public record City(string Name, Coords Location);

public static class Cities
{
    public static readonly IReadOnlyList<City> AllCities =
    [
        new("Antwerpen", new Coords(51.22, 4.40)),
        new("Brussel", new Coords(50.85, 4.35)),
        new("Gent", new Coords(51.05, 3.73)),
        new("Brugge", new Coords(51.21, 3.23)),
        new("Hasselt", new Coords(50.93, 5.34)),
        new("Leuven", new Coords(50.88, 4.70)),
        new("Mechelen", new Coords(51.03, 4.48)),
        new("Aalst", new Coords(50.94, 4.04)),
        new("Sint-Niklaas", new Coords(51.17, 4.14)),
        new("Kortrijk", new Coords(50.83, 3.26)),
        new("Oostende", new Coords(51.22, 2.93)),
        new("Roeselare", new Coords(50.95, 3.12)),
        new("Charleroi", new Coords(50.41, 4.44)),
        new("Liège", new Coords(50.63, 5.58)),
        new("Namur", new Coords(50.47, 4.87)),
        new("Mons", new Coords(50.45, 3.95)),
    ];

    public static IReadOnlyList<string> Names => AllCities.Select(city => city.Name).ToList();
    public static IReadOnlyList<double> Latitudes => AllCities.Select(city => city.Location.Latitude).ToList();
    public static IReadOnlyList<double> Longitudes => AllCities.Select(city => city.Location.Longitude).ToList();

    public static readonly IReadOnlyDictionary<(double Latitude, double Longitude), string> CityDictionary =
        AllCities.ToDictionary(city => (city.Location.Latitude, city.Location.Longitude), city => city.Name);
}
