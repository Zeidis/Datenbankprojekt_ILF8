using System.ComponentModel.DataAnnotations;

namespace RocketMoonApp.Server.Models
{
    public class AnalysisCache
    {
        [Key]
        public int Id { get; set; }

        public string MoonPhaseCategory { get; set; } = string.Empty;

        public int TotalLaunches { get; set; }

        public int SuccessfulLaunches { get; set; }

        public float SuccessRate { get; set; }

        public DateTime CalculatedAt { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}