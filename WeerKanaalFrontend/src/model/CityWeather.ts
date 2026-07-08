export type CityWeather = {
    name: string;
    report: {
        tempMin: number;
        tempMax: number;
        icon: WeatherIconType;
        windWarning: number;
    }
}

export const WeatherIcon = {
    Hottie: 0,
    Sunny: 1,
    CloudySunny: 2,
    Cloudy: 3,
    SunnyRainy: 4,
    Rainy: 5,
    Snowy: 6,
    Stormy: 7,
    Frosty: 8,
    Windy: 9,
    Foggy: 10
} as const;

export type WeatherIconType = typeof WeatherIcon[keyof typeof WeatherIcon];