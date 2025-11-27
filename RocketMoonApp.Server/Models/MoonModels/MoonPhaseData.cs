using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.MoonModels
{
    public class MoonPhaseData
    {
        [JsonPropertyName("day")]
        public int Tag { get; set; }

        [JsonPropertyName("month")]
        public int Monat { get; set; }

        [JsonPropertyName("phase")]
        public string Phase { get; set; }

        [JsonPropertyName("time")]
        public string Zeit { get; set; }

        [JsonPropertyName("year")]
        public int Jahr { get; set; }
    }
}
