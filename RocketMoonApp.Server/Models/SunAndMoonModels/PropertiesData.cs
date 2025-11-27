using RocketMoonApp.Server.Models.MoonModels;
using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.SunAndMoonModels
{
    public class PropertiesData
    {
        [JsonPropertyName("closestphase")]
        public MoonPhaseData Closestphase { get; set; } = new();

        [JsonPropertyName("curphase")]
        public string Curphase { get; set; }

        [JsonPropertyName("day")]
        public int Tag { get; set; }

        [JsonPropertyName("day_of_week")]
        public string DayOfWeek { get; set; }

        [JsonPropertyName("fracillum")]
        public string Fracillum { get; set; }

        [JsonPropertyName("isdst")]
        public bool IsDST { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("month")]
        public int Monat { get; set; }

        [JsonPropertyName("moondata")]
        public List<Entry> Moondata { get; set; } = new List<Entry>();

        [JsonPropertyName("sundata")]
        public List<Entry> Sundata { get; set; } = new List<Entry>();

        [JsonPropertyName("tz")]
        public float TimeZone { get; set; }

        [JsonPropertyName("year")]
        public int Jahr { get; set; }
    }
}
