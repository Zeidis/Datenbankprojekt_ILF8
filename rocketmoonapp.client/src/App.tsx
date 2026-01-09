import { use, useEffect, useState } from 'react';
import './App.css';
import { MoonChart } from './components/MoonCharts'; 


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

interface RocketType{
    id:string;
    name:string;
    successRate: number;
}


// State 
function App() {

    const [activeYear, setActiveYear] = useState<number>(2025);
    const [activeRocket, setActiveRocket] = useState<string>('Alle');


    const [moonPhases, setMoonPhases] = useState<MoonPhaseApiResponse | null>(null);
    const[rocketTypes, setRocketTypes] = useState<RocketType[]>([]);

    const[selectedYear, setSelectedYear] = useState<number>(2025);
    const[selectedRocket, setSelectedRocket] = useState<string>('Alle');


    useEffect(() => {
        populateMoonPhaseData(activeYear);
    }, [activeYear]); // trigger when activeYear changes

    useEffect(() => {
        fetchRocketTypes();
    }, []); // 1 mal beim laden


    const handleApplyFilters = () => {
        setActiveYear(selectedYear);
        setActiveRocket(selectedRocket);
    }

    const handleReset = () => {
        const defaultYear = 2025;
        const defaultRocket = 'All';

        setSelectedYear(defaultYear);
        setSelectedRocket(defaultRocket);
        setActiveYear(defaultYear);
        setActiveRocket(defaultRocket);
    };

async function populateMoonPhaseData(year: number){
    try{
        const response = await fetch(`http://localhost:5285/MoonPhase?jahr=${year}`);
        if(response.ok){
            const data: MoonPhaseApiResponse = await response.json();
            setMoonPhases(data);
        }
    }
    catch (error){
        console.error('Error fetching moon phase data:', error);
    }
}

async function fetchRocketTypes(){
    //wait for data from API
     
    const mockData: RocketType[] = [
        { id: '1', name: 'Falcon 9', successRate: 97 },
        { id: '2', name: 'Starship', successRate: 0 },
    ];
    setRocketTypes(mockData);
}

    const generateYears = () =>{
        const currentYear = 2025;
        const start = 2015;
        const result = [];
        for(let i = currentYear; i >= start; i--){
            result.push(i);
        }
        return result;
    };

    return (
        <div className="dashboard-container">
            
           
            <header className="header">
                <h1>SpaceX Success Rate Insights</h1>
                <div>

                    <button 
                    className="btn-secondary" style={{backgroundColor: '#e5e7eb', 
                    color: '#374151'}}
                    onClick={handleReset}>
                        Zurücksetzen
                        </button>
                </div>
            </header>

            {/* Sideboard + grafik*/}
            <main className="main-content">
                
                {/* filters sideboard*/}
                <aside className="sidebar">
                    <h2 style={{fontSize: '20px', marginBottom: '20px'}}>Filter</h2>
                    
                    <div className="filter-group">
                        <label>Jahr</label>
                        <select 
                        className="filter-select"
                        value={selectedYear}
                        onChange={(e) => setSelectedYear(Number(e.target.value))}>
                            {generateYears().map(year => (
                                <option key={year} value={year}>{year}</option>
                        ))}
                        </select>
                    </div>

                    <div className="filter-group">
                        <label>Raketentyp</label>  
                        <select 
                        className="filter-select"
                        value={selectedRocket}
                        onChange={(e) => setSelectedRocket(e.target.value)}>
                            <option value="All">Alle</option>
                            {rocketTypes.map(rocket => (
                                <option key={rocket.id} value={rocket.name}>
                                    {rocket.name}
                                </option>
                            ))}

                        </select>
                    </div>

                    <button className="btn-primary"
                    onClick={handleApplyFilters}>
                        Filter anwenden
                        </button>
                </aside>

                {/*Grafik*/}
                <section className="chart-area">
                   
                    <MoonChart year={activeYear} rocketName={activeRocket} />
                </section>
            </main>
        </div>
    );
}

export default App;