import { useEffect, useState } from 'react';
import './App.css';

// Weather forecast interface
interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

// --- Moon phase interfaces ---
export interface MoonPhaseApiResponse {
    apiversion: string;
    numphases: number;
    phasedata: MoonPhaseData[];
    year: number;
}

export interface MoonPhaseData {
    day: number;
    month: number;
    phase: string;
    time: string;
    year: number;
}

function App() {
    // --- Weather forecast state ---
    const [forecasts, setForecasts] = useState<Forecast[]>();

    // --- Moon phase state ---
    const [moonPhases, setMoonPhases] = useState<MoonPhaseApiResponse | null>(null);

    useEffect(() => {
        populateWeatherData();
        populateMoonPhaseData();
    }, []);

    // --- Weather loading content ---
    const weatherContents = forecasts === undefined ? (
        <p>
            <em>Loading weather…</em>
        </p>
    ) : (
        <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map((forecast) => (
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );

    // --- Moon phase loading content ---
    const moonContents = moonPhases === null ? (
        <p>
            <em>Loading moon phases…</em>
        </p>
    ) : (
        <table className="table table-striped" aria-labelledby="moonTableLabel">
            <thead>
                <tr>
                    <th>Day</th>
                    <th>Month</th>
                    <th>Phase</th>
                    <th>Time</th>
                    <th>Year</th>
                </tr>
            </thead>
            <tbody>
                {moonPhases.phasedata.map((m, index) => (
                    <tr key={index}>
                        <td>{m.day}</td>
                        <td>{m.month}</td>
                        <td>{m.phase}</td>
                        <td>{m.time}</td>
                        <td>{m.year}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );

    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>
            <p>This shows data from your ASP.NET backend.</p>
            {weatherContents}

            <hr />

            <h1 id="moonTableLabel">Moon Phases</h1>
            <p>Fetched from your backend using the Moon API.</p>
            {moonContents}
        </div>
    );

    // ----------------------
    // Fetch weather
    // ----------------------
    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }

    // ----------------------
    // Fetch moon phases
    // ----------------------
    async function populateMoonPhaseData() {
        const response = await fetch('http://localhost:5285/MoonPhase/GetMoonPhase?jahr=2024');
        if (response.ok) {
            const data: MoonPhaseApiResponse = await response.json();
            setMoonPhases(data);
        }
    }
}

export default App;