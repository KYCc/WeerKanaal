export type CityWeather = {
    name: string;
    report: {
        tempMin: number;
        tempMax: number;
        icon: WeatherIconType;
        windWarning: boolean;
    }
}

export const WeatherIconEnum = {
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

export type WeatherIconType = typeof WeatherIconEnum[keyof typeof WeatherIconEnum];

const MOCK_DATA: CityWeather[] = [
    { name: 'Antwerpen',    report: { tempMin: 12, tempMax: 19, icon: WeatherIconEnum.Sunny,       windWarning: false } },
    { name: 'Brussel',      report: { tempMin: 11, tempMax: 18, icon: WeatherIconEnum.CloudySunny, windWarning: false } },
    { name: 'Gent',         report: { tempMin: 10, tempMax: 16, icon: WeatherIconEnum.Cloudy,      windWarning: false } },
    { name: 'Brugge',       report: { tempMin: 9,  tempMax: 15, icon: WeatherIconEnum.Rainy,       windWarning: false } },
    { name: 'Hasselt',      report: { tempMin: 19, tempMax: 31, icon: WeatherIconEnum.Hottie,      windWarning: false } },
    { name: 'Leuven',       report: { tempMin: 12, tempMax: 20, icon: WeatherIconEnum.SunnyRainy,  windWarning: false } },
    { name: 'Mechelen',     report: { tempMin: 8,  tempMax: 14, icon: WeatherIconEnum.Foggy,       windWarning: false } },
    { name: 'Aalst',        report: { tempMin: 13, tempMax: 21, icon: WeatherIconEnum.Windy,       windWarning: true  } },
    { name: 'Sint-Niklaas', report: { tempMin: -6, tempMax: -1, icon: WeatherIconEnum.Snowy,       windWarning: true  } },
    { name: 'Kortrijk',     report: { tempMin: 14, tempMax: 17, icon: WeatherIconEnum.Stormy,      windWarning: true  } },
    { name: 'Oostende',     report: { tempMin: 11, tempMax: 15, icon: WeatherIconEnum.Windy,       windWarning: true  } },
    { name: 'Roeselare',    report: { tempMin: -3, tempMax: 2,  icon: WeatherIconEnum.Frosty,      windWarning: false } },
    { name: 'Charleroi',    report: { tempMin: 10, tempMax: 17, icon: WeatherIconEnum.Cloudy,      windWarning: false } },
    { name: 'Liège',        report: { tempMin: 9,  tempMax: 16, icon: WeatherIconEnum.Rainy,       windWarning: false } },
    { name: 'Namur',        report: { tempMin: 10, tempMax: 18, icon: WeatherIconEnum.CloudySunny, windWarning: false } },
    { name: 'Mons',         report: { tempMin: 11, tempMax: 19, icon: WeatherIconEnum.Sunny,       windWarning: false } },
];

export const data: CityWeather[] = window.__Weather__ ?? MOCK_DATA;

// Tomorrow's date as an ISO string (injected by the backend during capture; mock in dev).
const MOCK_DATE = "2026-07-10";
export const reelDate: string = window.__Date__ ?? MOCK_DATE;

// Overall weather mood (injected by the backend, same logic as the music picker).
export type Vibe = "hot" | "cold" | "rain" | "storm" | "grey";
const MOCK_VIBE: Vibe = "hot";
export const vibe: Vibe = window.__Vibe__ ?? MOCK_VIBE;

declare global {
    interface Window {
        __Weather__?: CityWeather[]
        __Date__?: string
        __Vibe__?: Vibe
    }
}