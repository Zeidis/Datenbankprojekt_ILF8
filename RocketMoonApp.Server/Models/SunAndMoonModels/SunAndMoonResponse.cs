using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.SunAndMoonModels
{
    public class SunAndMoonResponse
    {
        [JsonPropertyName("apiversion")]
        public string ApiVersion { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; } = new Geometry();

        [JsonPropertyName("properties")]
        public Properties Properties { get; set; } = new Properties();

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
