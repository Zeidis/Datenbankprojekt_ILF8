using System.ComponentModel.DataAnnotations;

namespace RocketMoonApp.Server.Models
{
    public class MoonPhase
    {
        [Key]
        public int MoonPhaseId { get; set; }

        public string PhaseName { get; set; } = string.Empty;

        public double PhasePercentage { get; set; }

        public DateTime Date { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}