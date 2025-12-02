using RocketMoonApp.Server.Models.RocketModels;
using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.RocketModels
{
    public class RocketDetails
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("configuration")]
        public RocketConfiguration? Configuration { get; set; }
    }
}
