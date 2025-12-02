using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.RocketModels
{
    public class RocketConfiguration
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("variant")]
        public string Variant { get; set; } = string.Empty;
    }
}
