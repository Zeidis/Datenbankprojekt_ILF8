using RocketMoonApp.Server.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Newtonsoft.Json.Linq;

namespace RocketMoonApp.Server.Tests
{
    public class DbContextTests
    {
        [Fact]
        public void AllDbSetsAvailable()
        {
            var options = new DbContextOptionsBuilder<RocketMoonDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            using var context = new RocketMoonDbContext(options);

            Assert.NotNull(context.Launches);
            Assert.NotNull(context.Locations);
            Assert.NotNull(context.MoonPhases);
            Assert.NotNull(context.AnalysisCaches);
        }

        [Fact]
        public void ConnectionStringIsRelativeInAppSettings()
        {
            // Attempt to load appsettings.json from the RocketMoonApp.Server project directory
            var current = Directory.GetCurrentDirectory();

            // Walk up to repository root to find RocketMoonApp.Server
            var dir = new DirectoryInfo(current);
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, "RocketMoonApp.Server")))
            {
                dir = dir.Parent;
            }

            Assert.NotNull(dir); // repository root not found

            var appSettingsPath = Path.Combine(dir!.FullName, "RocketMoonApp.Server", "appsettings.json");
            Assert.True(File.Exists(appSettingsPath), $"Could not find {appSettingsPath}");

            var json = File.ReadAllText(appSettingsPath);
            var parsed = JObject.Parse(json);
            var cs = parsed.SelectToken("ConnectionStrings.RocketMoon")?.ToString();

            Assert.False(string.IsNullOrWhiteSpace(cs), "Connection string RocketMoon not found in appsettings.json");
            Assert.Contains("./rocketmoon.db", cs);
        }
    }
}