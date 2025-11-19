namespace RocketMoonApp.Server.Models
{
    public class MoonPhaseDto
    {
        public int Id { get; set; }
        public string PhaseName { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
}