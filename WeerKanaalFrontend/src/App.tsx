import {WeatherStrip} from "./components/WeatherStrip.tsx";
import {data, reelDate, type Vibe, vibe} from "./model/CityWeather.ts";
import {useEffect, useState} from "react";

// Format tomorrow's date, nl-BE (e.g. "10 juli" / "2026").
// Parse at noon so a UTC-midnight ISO date can't slip to the previous day.
const day = new Date(reelDate + "T12:00:00");
const dayMonth = new Intl.DateTimeFormat("nl-BE", { day: "numeric", month: "long" }).format(day);
const year = new Intl.DateTimeFormat("nl-BE", { year: "numeric" }).format(day);

const vibeGradients : Record<Vibe, string> = {
    hot: "bg-[linear-gradient(180deg,#d8ecfc,#eaf4fd_38%,#f6fafe)]",
    cold: "bg-[linear-gradient(180deg,#a9e4f5,#c4eefa_40%,#ddf6fd)]",
    rain: "bg-[linear-gradient(180deg,#8fb2d8,#a7c6e4_45%,#c2dbf0)]",
    storm: "bg-[linear-gradient(180deg,#5c6b83,#74849b_45%,#8f9db2)]",
    grey: "bg-[linear-gradient(180deg,#b4bcc6,#c6ccd4_45%,#d6dbe0)]"
}

const PAGE_SIZE = 4;

function App() {
    const [startIndex, setStartIndex] = useState(0);
    const [ready, setReady] = useState(false);

    // Ready once fonts are loaded and a frame has painted.
    useEffect(() => {
        document.fonts.ready.then(() => {
            requestAnimationFrame(() => {
                requestAnimationFrame(() => {
                    document.body.dataset.ready = "true";
                    setReady(true);
                })
            })
        })
    }, []);

    // Once ready, advance to the next 4 cities every 5s, stopping on the last group.
    useEffect(() => {
        if (!ready || startIndex + PAGE_SIZE >= data.length) return;
        const id = setTimeout(() => setStartIndex(i => i + PAGE_SIZE), 5000);
        return () => clearTimeout(id);
    }, [ready, startIndex]);

  return (
    <div className="flex w-[1080px] h-[1920px] flex-col font-display">
        <div className="bg-black h-[420px]"></div>
        <div className={`flex flex-col box-border h-[1080px] px-[64px] py-[72px] ${vibeGradients[vibe]}`}>
            <div className="flex items-end justify-between mb-[44px]">
                <div className="flex flex-col gap-[10px]">
                    <span className={`font-bold text-[26px] tracking-[7px] ${vibe == "storm" ? "text-white" : "text-accent"}`}>HET WEER KANAAL</span>
                    <span className={`font-bold text-[78px] leading-none tracking-[-2px] ${vibe == "storm" ? "text-white" : "text-ink"}`}>Morgen</span>
                </div>
                <div className="flex flex-col items-end gap-[6px] pb-[8px]">
                    <span className="font-medium text-[34px] text-ink">{dayMonth}</span>
                    <span className="font-normal text-[28px] text-muted">{year}</span>
                </div>
            </div>
            <div className="flex-1 flex flex-col">
                {ready && (
                    <div key={startIndex} className="flex-1 flex flex-col gap-[28px]">
                        {data.slice(startIndex, startIndex + PAGE_SIZE).map((city, i) => (
                            <WeatherStrip key={i} cityWeather={city} index={i} />
                        ))}
                    </div>
                )}
            </div>
        </div>
        <div className="bg-black h-[420px]"></div>
    </div>
  )
}

export default App
