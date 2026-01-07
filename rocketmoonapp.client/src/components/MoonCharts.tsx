import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Cell
} from 'recharts';


const data = [
  { name: 'New Moon', rate: 10, icon: '/moon-phases/elipse_1.svg' },
  { name: 'Waxing Crescent', rate: 50, icon: '/moon-phases/elipse_2.svg' },
  { name: 'First Quarter', rate: 70, icon: '/moon-phases/elipse_3.svg' },
  { name: 'Waxing Gibbous', rate: 45, icon: '/moon-phases/elipse_4.svg' },
  { name: 'Full Moon', rate: 48, icon: '/moon-phases/elipse_5.svg' },
  { name: 'Waning Gibbous', rate: 90, icon: '/moon-phases/elipse_6.svg' },
  { name: 'Last Quarter', rate: 60, icon: '/moon-phases/elipse_7.svg' },
  { name: 'Waning Crescent', rate: 55, icon: '/moon-phases/elipse_8.svg' },
];

const CustomTick = (props: any) => {
  const { x, y, payload } = props;
  const iconPath = data[payload.index]?.icon;
  

  const size = 38; 
  const offset = size / 2; 

  return (
    <image 
      x={x - offset} 
      y={y + 10}     
      href={iconPath} 
      height={size} 
      width={size} 
    />
  );
};

export const MoonChart = () => {
  return (

    <div style={{ width: '100%', height: '500px' }}>
      
      <h3 style={{ fontSize: '18px', marginBottom: '8px', fontWeight: 600 }}>
        Startfrequenz & Mondphase
      </h3>
      <p style={{ color: '#5b4dff', fontSize: '14px', marginBottom: '30px', fontWeight: 500 }}>
        2023–2024
      </p>
      
      <ResponsiveContainer width="100%" height="85%">
        
        <BarChart data={data} margin={{ top: 20, right: 30, left: 0, bottom: 60 }}>
          
          <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f0f0f0" />
          
          <XAxis 
            dataKey="name" 
            tick={<CustomTick />} 
            axisLine={false} 
            tickLine={false}
            interval={0} 
          />
          
          <YAxis 
            tickFormatter={(value) => `${value}%`} 
            axisLine={false} 
            tickLine={false}
            tick={{ fill: '#5b4dff', fontSize: 12, fontWeight: 500 }}
          />
          
          <Tooltip 
            cursor={{ fill: 'rgba(91, 77, 255, 0.05)' }}
            contentStyle={{ borderRadius: '12px', border: 'none', boxShadow: '0 4px 12px rgba(0,0,0,0.1)' }}
          />
          
          
          <Bar dataKey="rate" radius={[8, 8, 8, 8]} barSize={32}>
            {data.map((entry, index) => (
              <Cell key={`cell-${index}`} fill="#5b4dff" />
            ))}
          </Bar>
          
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
};