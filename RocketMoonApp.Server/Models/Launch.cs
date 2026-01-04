using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketMoonApp.Server.Models
{
    public class Launch
    {
        [Key]
        public string LaunchId { get; set; } = string.Empty;

        public string RocketName { get; set; } = string.Empty;

        public DateTimeOffset Date { get; set; }

        // Foreign Key
        public int LocationId { get; set; }

        // Navigation
        [ForeignKey(nameof(LocationId))]
        public Location Location { get; set; } = null!;

        public string Status { get; set; } = string.Empty;

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}