namespace RocketMoonApp.Server.Models
{
    public class MoonPhase
    {
        public int Id { get; set; } = 0;
        public string PhaseName { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public Location Location { get; set; } = new Location();
    }
}