namespace WeerKanaalBackend.util
{
    public record Coords(double Latitude, double Longitude);
    public record City(string Name, Coords Location);

    public static class Cities
    {
        public static readonly string[] Names =
        [
            "Antwerpen",
            "Brussel",
            "Gent",
            "Brugge",
            "Hasselt",
            "Leuven",
            "Mechelen",
            "Aalst",
            "Sint-Niklaas",
            "Kortrijk",
            "Oostende",
            "Roeselare",
            "Charleroi",
            "Liège",
            "Namur",
            "Mons",
        ];

        public static readonly double[] Latitudes =
        [
            51.22,
            50.85,
            51.05,
            51.21,
            50.93,
            50.88,
            51.03,
            50.94,
            51.17,
            50.83,
            51.22,
            50.95,
            50.41,
            50.63,
            50.47,
            50.45,
        ];

        public static readonly double[] Longitudes =
        [
            4.40,
            4.35,
            3.73,
            3.23,
            5.34,
            4.70,
            4.48,
            4.04,
            4.14,
            3.26,
            2.93,
            3.12,
            4.44,
            5.58,
            4.87,
            3.95,
        ];

        public static readonly IReadOnlyList<City> AllCities =
            Names
                .Select((name, i) => new City(name, new Coords(Latitudes[i], Longitudes[i])))
                .ToList();
    }
}