using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.RocketModels
{
    public class Pad
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public float Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public float Longitude { get; set; }

        [JsonPropertyName("location")]
        public Location? Location { get; set; }
    }
}
