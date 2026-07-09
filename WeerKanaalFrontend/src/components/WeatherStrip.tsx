import {type CityWeather} from "../model/CityWeather.ts";
import {WeatherIcon} from "../icons";

interface WeatherStripProps {
    cityWeather: CityWeather
}

export function WeatherStrip({ cityWeather }: WeatherStripProps) {
    const { name, report } = cityWeather;
    return (
        <div className="flex flex-1 items-center bg-white rounded-[30px] px-[52px] shadow-[0_14px_34px_rgba(35,70,120,0.12)]">
            <WeatherIcon icon={report.icon} className="w-[112px] h-[112px]" />
            {report.windWarning ? (
                <div className="flex flex-1 flex-col items-start gap-[10px] ml-[40px]">
                    <span className="font-medium text-[50px] text-ink">{name}</span>
                    <span className="bg-ink text-[#f2f7fc] font-medium text-[24px] tracking-[2px] px-[18px] py-[6px] rounded-full">WINDSTOTEN</span>
                </div>
            ) : (
                <span className="flex-1 ml-[40px] font-medium text-[50px] text-ink">{name}</span>
            )}
            <div className="flex items-baseline gap-[20px]">
                <span className="font-bold text-[66px] tracking-[-2px] text-accent">{report.tempMax}°</span>
                <span className="font-normal text-[42px] text-faint">{report.tempMin}°</span>
            </div>
        </div>
    )
}
