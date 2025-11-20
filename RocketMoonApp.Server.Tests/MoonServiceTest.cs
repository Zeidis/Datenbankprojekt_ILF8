using RocketMoonApp.Server.Models;
using RocketMoonApp.Server.Services;
using System.Net;
using System.Text.Json;
using Xunit;
using Microsoft.Extensions.Logging;

namespace RocketMoonApp.Server.Tests
{
    public class MoonServiceTests
    {
        private readonly ILogger<MoonService> _logger;

        public MoonServiceTests()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<MoonService>();
        }

        [Fact(Skip = "Integration test - runs against real API")]
        public async Task IntegrationTest_RealApiCall()
        {
            //Arrange
            var httpClient = new HttpClient();
            var moonService = new MoonService(httpClient, _logger);

            var year = 2025;

            // Act
            var moonPhases = await moonService.GetMoonPhasesForYearAsync(year);

            // Assert
            Assert.NotNull(moonPhases);
        }

        [Fact]
        public async Task GetMoonPhasesForYearAsync_MapDataCorrectly()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var moonService = new MoonService(httpClient, _logger);

            var mockResponse = "{\"phasedata\": [{\"phase\": \"Full Moon\",\"year\": 2025,\"month\": 11,\"day\": 27}]}";

            mockHttpMessageHandler.SetupResponse(mockResponse, HttpStatusCode.OK);

            var year = 2025;

            // Act
            var moonPhases = await moonService.GetMoonPhasesForYearAsync(year);

            // Assert
            Assert.NotNull(moonPhases);
            Assert.NotEmpty(moonPhases);

            var firstPhase = moonPhases.First();
            Assert.Equal("Full Moon", firstPhase.PhaseName);
            Assert.Equal(new DateTime(2025, 11, 27), firstPhase.Date);
        }

        [Fact]
        public async Task GetMoonPhasesForYearAsync_ShouldHandleEmptyResults()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var moonService = new MoonService(httpClient, _logger);

            var emptyResponse = "{\"phasedata\": []}";

            mockHttpMessageHandler.SetupResponse(emptyResponse, HttpStatusCode.OK);

            var year = 2025;

            // Act
            var moonPhases = await moonService.GetMoonPhasesForYearAsync(year);

            // Assert
            Assert.NotNull(moonPhases);
            Assert.Empty(moonPhases);
        }

        [Fact]
        public async Task GetMoonPhaseForDayAsync_MapDataCorrectly()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var moonService = new MoonService(httpClient, _logger);

            var mockResponse = "{\"properties\": {\"curphase\": \"New Moon\"}}";

            mockHttpMessageHandler.SetupResponse(mockResponse, HttpStatusCode.OK);

            var date = new DateTime(2025, 11, 20);

            // Act
            var moonPhase = await moonService.GetMoonPhaseForDayAsync(date);

            // Assert
            Assert.NotNull(moonPhase);
            Assert.Equal("New Moon", moonPhase.PhaseName);
            Assert.Equal(date, moonPhase.Date);
        }

        [Fact]
        public async Task GetMoonPhaseForDayAsync_ShouldHandleInvalidUrl()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var moonService = new MoonService(httpClient, _logger);

            mockHttpMessageHandler.SetupResponse(string.Empty, HttpStatusCode.NotFound);

            var date = new DateTime(2025, 11, 20);

            // Act
            var moonPhase = await moonService.GetMoonPhaseForDayAsync(date);

            // Assert
            Assert.NotNull(moonPhase);
            Assert.Equal(string.Empty, moonPhase.PhaseName);
        }
    }
}