using System.ComponentModel.DataAnnotations;

namespace RocketMoonApp.Server.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        public string CountryName { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;

        // Optional, aber sauber für EF für 1:n Beziehung
        public ICollection<Launch> Launches { get; set; } = new List<Launch>();

        // Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}