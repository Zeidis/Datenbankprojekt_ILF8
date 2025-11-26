using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.SunAndMoonModels
{
    public class Geometry
    {
        [JsonPropertyName("coordinates")]
        public List<decimal> Coordinates { get; set; } = new List<decimal>();

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
