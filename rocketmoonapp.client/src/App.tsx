import { useEffect, useState } from 'react';
import './App.css';
import { MoonChart } from './components/MoonCharts'; 

//Weather forecast interface
interface Forecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

//Moon phase interfaces
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
    
    const [forecasts, setForecasts] = useState<Forecast[]>();
    const [moonPhases, setMoonPhases] = useState<MoonPhaseApiResponse | null>(null);

    useEffect(() => {
        populateWeatherData();
        populateMoonPhaseData();
    }, []);

    
    const weatherContents = forecasts === undefined ? (
        <p><em>Loading weather...</em></p>
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

    
    const moonContents = moonPhases === null ? (
        <p><em>Loading moon phases...</em></p>
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
        <div className="dashboard-container">
            
           
            <header className="header">
                <h1>SpaceX Success Rate Insights</h1>
                <div>
                    <button className="btn-secondary" style={{backgroundColor: '#5b4dff', color: 'white'}}>Daten abrufen</button>
                    <button className="btn-secondary" style={{backgroundColor: '#e5e7eb', color: '#374151'}}>Zurücksetzen</button>
                </div>
            </header>

            {/* Sideboard + grafik*/}
            <main className="main-content">
                
                {/* filters sideboard*/}
                <aside className="sidebar">
                    <h2 style={{fontSize: '20px', marginBottom: '20px'}}>Filter</h2>
                    
                    <div className="filter-group">
                        <label>Datenbereich</label>
                        <select className="filter-select">
                            <option>Letztes Jahr</option>
                            <option>Alles</option>
                        </select>
                    </div>

                    <div className="filter-group">
                        <label>Raketentyp</label>
                        <select className="filter-select">
                            <option>Alle</option>
                            <option>Falcon 9</option>
                            <option>Starship</option>
                        </select>
                    </div>

                    <div className="filter-group">
                        <label>Mondphase</label>
                        <select className="filter-select">
                            <option>Alle</option>
                            <option>Full Moon</option>
                            <option>New Moon</option>
                        </select>
                    </div>

                    <button className="btn-primary">Filter anwenden</button>
                </aside>

                {/*Grafik*/}
                <section className="chart-area">
                   
                    <MoonChart />
                </section>
            </main>

          
           
            <details className="debug-section">
                <summary>Backend Debug Data (Click to show tables)</summary>
                
                <div style={{marginTop: '20px'}}>
                    <h3 id="tableLabel">Weather forecast</h3>
                    {weatherContents}
                    
                    <hr style={{margin: '20px 0'}}/>
                    
                    <h3 id="moonTableLabel">Moon Phases (API Data)</h3>
                    {moonContents}
                </div>
            </details>
        </div>
    );

    //Helper functions
    async function populateWeatherData() {
        const response = await fetch('weatherforecast');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }

    async function populateMoonPhaseData() {
        const response = await fetch('http://localhost:5285/MoonPhase?jahr=2024');
        if (response.ok) {
            const data: MoonPhaseApiResponse = await response.json();
            setMoonPhases(data);
        }
    }
}

export default App;