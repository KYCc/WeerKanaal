namespace WeerKanaalBackend.util
{
    public record Coords(double Latitude, double Longitude);
    public record City(string Name, Coords Location);

    public static class Cities
    {
        public static IReadOnlyList<City> AllCities { get; } =
        [
            new City("Antwerpen", new Coords(51.22, 4.40)),
            new City("Brussel", new Coords(50.85, 4.35)),
            new City("Gent", new Coords(51.05, 3.73)),
            new City("Brugge", new Coords(51.21, 3.23)),
            new City("Hasselt", new Coords(50.93, 5.34)),
            new City("Leuven", new Coords(50.88, 4.70)),
            new City("Mechelen", new Coords(51.03, 4.48)),
            new City("Aalst", new Coords(50.94, 4.04)),
            new City("Sint-Niklaas", new Coords(51.22, 4.40)),
            new City("Kortrijk", new Coords(51.22, 4.40)),
            new City("Oostende", new Coords(51.22, 4.40)),
            new City("Roeselare", new Coords(51.22, 4.40)),
            new City("Charleroi", new Coords(51.22, 4.40)),
            new City("Liège", new Coords(51.22, 4.40)),
            new City("Namur", new Coords(51.22, 4.40)),
            new City("Mons", new Coords(51.22, 4.40)),
            new City("La Louvière", new Coords(51.22, 4.40)),
            new City("Tournai", new Coords(51.22, 4.40)),
            new City("Seraing", new Coords(51.22, 4.40)),
            new City("Verviers", new Coords(51.22, 4.40)),
        ];
    }
}