namespace WeerKanaalBackend.util
{
    public record Coords(double Latitude, double Longitude);
    public record City(string Name, Coords Location);

    public static class Cities
    {
        public static readonly IReadOnlyList<City> AllCities =
        [
            new City("Antwerpen", new Coords(51.22, 4.40)),
            new City("Brussel", new Coords(50.85, 4.35)),
            new City("Gent", new Coords(51.05, 3.73)),
            new City("Brugge", new Coords(51.21, 3.23)),
            new City("Hasselt", new Coords(50.93, 5.34)),
            new City("Leuven", new Coords(50.88, 4.70)),
            new City("Mechelen", new Coords(51.03, 4.48)),
            new City("Aalst", new Coords(50.94, 4.04)),
            new City("Sint-Niklaas", new Coords(51.17, 4.14)),
            new City("Kortrijk", new Coords(50.83, 3.26)),
            new City("Oostende", new Coords(51.22, 2.93)),
            new City("Roeselare", new Coords(50.95, 3.12)),
            new City("Charleroi", new Coords(50.41, 4.44)),
            new City("Liège", new Coords(50.63, 5.58)),
            new City("Namur", new Coords(50.47, 4.87)),
            new City("Mons", new Coords(50.45, 3.95)),
        ];
    }
}