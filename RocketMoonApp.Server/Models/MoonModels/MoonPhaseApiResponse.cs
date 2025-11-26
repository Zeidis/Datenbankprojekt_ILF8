using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.MoonModels
{
    public class MoonPhaseApiResponse
    {
        [JsonPropertyName("apiversion")]
        public string ApiVersion { get; set; }

        [JsonPropertyName("day")]
        public int? Tag { get; set; }

        [JsonPropertyName("month")]
        public int? Monat { get; set; }

        [JsonPropertyName("numphases")]
        public int Numphases { get; set; }

        [JsonPropertyName("phasedata")]
        public List<MoonPhaseData> Phasedata { get; set; } = new List<MoonPhaseData>();

        [JsonPropertyName("year")]
        public int Jahr { get; set; }
    }
}
