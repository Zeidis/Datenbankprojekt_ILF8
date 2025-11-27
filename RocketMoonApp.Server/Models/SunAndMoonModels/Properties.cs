using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.SunAndMoonModels
{
    public class Properties
    {
        [JsonPropertyName("data")]
        public PropertiesData Data { get; set; }
    }
}
