using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.RocketModels
{
    public class Agency
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}
