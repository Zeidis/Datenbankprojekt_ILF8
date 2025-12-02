using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.RocketModels
{
    public class LaunchResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public LaunchStatus? Status { get; set; }

        [JsonPropertyName("net")]
        public DateTime? Net { get; set; }

        [JsonPropertyName("window_start")]
        public DateTime? WindowStart { get; set; }

        [JsonPropertyName("window_end")]
        public DateTime? WindowEnd { get; set; }

        [JsonPropertyName("launch_service_provider")]
        public Agency? LaunchServiceProvider { get; set; }

        [JsonPropertyName("rocket")]
        public RocketDetails? Rocket { get; set; }

        [JsonPropertyName("pad")]
        public Pad? Pad { get; set; }
    }
}
