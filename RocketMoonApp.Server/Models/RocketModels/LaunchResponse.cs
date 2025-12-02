using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.RocketModels
{
    public class LaunchResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; init; }

        [JsonPropertyName("next")]
        public string? Next { get; init; }

        [JsonPropertyName("previous")]
        public string? Previous { get; init; }

        [JsonPropertyName("results")]
        public List<LaunchResult> Results { get; init; } = new();
    }
}
