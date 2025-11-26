using System.Text.Json.Serialization;

namespace RocketMoonApp.Server.Models.SunAndMoonModels
{
    public class Entry
    {
        [JsonPropertyName("phen")]
        public string Phen {  get; set; }

        [JsonPropertyName("time")]
        public string Zeit {  get; set; }
    }
}
