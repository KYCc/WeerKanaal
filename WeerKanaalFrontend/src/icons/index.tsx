import {WeatherIconEnum, type WeatherIconType} from "../model/CityWeather.ts";
import type {ComponentType, SVGProps} from "react";

// TWC glyphs mapped to our WeatherIconEnum (day variants).
// Hottie is a red copy of the sun; everything else keeps its native TWC colors.
import Sun from "./SVGs/sun.svg?react";
import HottieIcon from "./SVGs/hottie.svg?react";
import CloudySunnyIcon from "./SVGs/cloudy-sunny.svg?react";
import CloudyIcon from "./SVGs/cloudy.svg?react";
import SunnyRainyIcon from "./SVGs/sunny-rainy.svg?react";
import RainyIcon from "./SVGs/rainy.svg?react";
import SnowyIcon from "./SVGs/snowy.svg?react";
import StormyIcon from "./SVGs/stormy.svg?react";
import FrostyIcon from "./SVGs/frosty.svg?react";
import WindyIcon from "./SVGs/windy.svg?react";
import FoggyIcon from "./SVGs/foggy.svg?react";

type IconComponent = ComponentType<SVGProps<SVGSVGElement>>;

const ICONS: Record<WeatherIconType, IconComponent> = {
    [WeatherIconEnum.Hottie]: HottieIcon,
    [WeatherIconEnum.Sunny]: Sun,
    [WeatherIconEnum.CloudySunny]: CloudySunnyIcon,
    [WeatherIconEnum.Cloudy]: CloudyIcon,
    [WeatherIconEnum.SunnyRainy]: SunnyRainyIcon,
    [WeatherIconEnum.Rainy]: RainyIcon,
    [WeatherIconEnum.Snowy]: SnowyIcon,
    [WeatherIconEnum.Stormy]: StormyIcon,
    [WeatherIconEnum.Frosty]: FrostyIcon,
    [WeatherIconEnum.Windy]: WindyIcon,
    [WeatherIconEnum.Foggy]: FoggyIcon,
}

interface WeatherIconProps extends SVGProps<SVGSVGElement> {
    icon: WeatherIconType
}

export function WeatherIcon({icon, ...props}: WeatherIconProps) {
    const Icon = ICONS[icon];
    return (
        <Icon {...props} />
    )
}
