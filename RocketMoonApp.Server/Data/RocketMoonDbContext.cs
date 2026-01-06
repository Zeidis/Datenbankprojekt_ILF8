using Microsoft.EntityFrameworkCore;
using RocketMoonApp.Server.Models;

namespace RocketMoonApp.Server.Data
{
    public class RocketMoonDbContext : DbContext
    {
        public RocketMoonDbContext(DbContextOptions<RocketMoonDbContext> options) : base(options)
        {
        }

        public DbSet<Launch> Launches { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<MoonPhase> MoonPhases { get; set; } = null!;
        public DbSet<AnalysisCache> AnalysisCaches { get; set; } = null!;
    }
}