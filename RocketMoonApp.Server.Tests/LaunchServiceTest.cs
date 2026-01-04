using RocketMoonApp.Server.Models;
using RocketMoonApp.Server.Services;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Microsoft.Extensions.Logging;

namespace RocketMoonApp.Server.Tests
{
    public class LaunchServiceTests
    {
        private readonly ILogger<LaunchService> _logger;

        public LaunchServiceTests()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = loggerFactory.CreateLogger<LaunchService>();
        }


        [Fact(Skip = "Integration test - runs against real API")]
        public async Task IntegrationTest_RealApiCall()
        {
            var httpClient = new HttpClient();
            var launchService = new LaunchService(httpClient, _logger);

            // Kürzerer Zeitraum = weniger Requests = weniger Rate Limiting
            var startingDate = new DateTime(2025, 11, 1);
            var endingDate = new DateTime(2025, 11, 30);

            var launches = await launchService.GetLaunchesFromTimeframeAsync(startingDate, endingDate);

            Assert.NotNull(launches);
            // Keine Assert.NotEmpty - es könnten auch 0 Launches sein
        }

        [Fact]
        public async Task GetLaunchesFromTimeframeAsync_MapDataCorrectly()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var launchService = new LaunchService(httpClient, _logger);

            var mockResponse = @"{
                ""count"": 1,
                ""next"": null,
                ""previous"": null,
                ""results"": [
                    {
                        ""id"": ""test-uuid-123"",
                        ""rocket"": {
                            ""configuration"": {
                                ""name"": ""Falcon 9""
                            }
                        },
                        ""net"": ""2025-01-15T10:00:00Z"",
                        ""pad"": {
                            ""location"": {
                                ""id"": 1
                            },
                            ""country"": {
                                ""name"": ""USA""
                            },
                            ""latitude"": 28.5,
                            ""longitude"": -80.5
                        },
                        ""status"": {
                            ""abbrev"": ""Success""
                        }
                    }
                ]
            }";

            mockHttpMessageHandler.SetupResponse(mockResponse, HttpStatusCode.OK);

            var startingDate = new DateTime(2025, 1, 1);
            var endingDate = new DateTime(2025, 12, 31);

            // Act
            var launches = await launchService.GetLaunchesFromTimeframeAsync(startingDate, endingDate);

            // Assert
            Assert.NotNull(launches);
            Assert.NotEmpty(launches);

            var firstLaunch = launches.First();
            Assert.Equal("test-uuid-123", firstLaunch.LaunchId);
            Assert.Equal("Falcon 9", firstLaunch.RocketName);
            Assert.Equal("USA", firstLaunch.Location.CountryName);
            Assert.Equal("Success", firstLaunch.Status);
        }

        [Fact]
        public async Task GetLaunchesFromTimeframeAsync_ShouldHandleEmptyResults()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var launchService = new LaunchService(httpClient, _logger);

            var startingDate = new DateTime(2025, 1, 1);
            var endingDate = new DateTime(2025, 12, 31);

            // Valide API-Response aber ohne Ergebnisse
            var emptyResponse = @"{
                ""count"": 0,
                ""next"": null,
                ""previous"": null,
                ""results"": []
            }";

            mockHttpMessageHandler.SetupResponse(emptyResponse, HttpStatusCode.OK);

            // Act
            var launches = await launchService.GetLaunchesFromTimeframeAsync(startingDate, endingDate);

            // Assert
            Assert.NotNull(launches);
            Assert.Empty(launches);
        }

        [Fact]
        public async Task GetLaunchesFromTimeframeAsync_ShouldHandleInvalidUrl()
        {
            // Arrange
            var mockHttpMessageHandler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(mockHttpMessageHandler);
            var launchService = new LaunchService(httpClient, _logger);

            var startingDate = new DateTime(2025, 1, 1);
            var endingDate = new DateTime(2025, 12, 31);

            // Mock gibt 404 zurück - das wirft eine Exception in GetStringAsync
            mockHttpMessageHandler.SetupResponse(string.Empty, HttpStatusCode.NotFound);

            // Act
            var launches = await launchService.GetLaunchesFromTimeframeAsync(startingDate, endingDate);

            // Assert
            Assert.NotNull(launches);
            Assert.Empty(launches);  // Bei Fehler wird eine leere Liste zurückgegeben
        }
    }
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _mockResponse = null!;

        public void SetupResponse(string content, HttpStatusCode statusCode)
        {
            _mockResponse = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content)
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_mockResponse);
        }
    }
}