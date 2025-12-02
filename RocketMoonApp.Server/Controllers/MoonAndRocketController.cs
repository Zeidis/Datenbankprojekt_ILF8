using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RocketMoonApp.Server.Models.MoonAndRocketModels;
using RocketMoonApp.Server.Models.MoonModels;
using RocketMoonApp.Server.Models.RocketModels;
using System.Net.Http;

namespace RocketMoonApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoonAndRocketController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public MoonAndRocketController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("GetMoonAndRocket")]
        public async Task<IActionResult> GetMoonAndRocket([FromQuery] int jahr)
        {
            string url = $"https://lldev.thespacedevs.com/2.3.0/launches/?limit=100&mode=normal&year={jahr}";

            var httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var rocketResponse = await httpResponse.Content.ReadFromJsonAsync<LaunchResponse>();

                var page = rocketResponse.Next;

                while (page != null)
                {
                    var tempHttpResonse = await _httpClient.GetAsync(page);
                    var tempResponse = await tempHttpResonse.Content.ReadFromJsonAsync<LaunchResponse>();
                    page = tempResponse.Next;
                    rocketResponse.Results.AddRange(tempResponse.Results);
                }

                url = $"https://aa.usno.navy.mil/api/moon/phases/year?year={jahr}";
                httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var moonResponse = await httpResponse.Content.ReadFromJsonAsync<MoonPhaseApiResponse>();

                    // hier wird drei int: Jahr, Monat, Tag in DateTime konvertiert
                    var gesammteDaten = moonResponse.Phasedata.Select(p => new
                    {
                        PhaseName = p.Phase,
                        Date = new DateTime(p.Jahr, p.Monat, p.Tag)
                    }).ToList();


                    var results = new List<MoonAndRocketResponse>();

                    foreach (var launch in rocketResponse.Results)
                    {
                        // Suchen welche Phase liegt neben RocketStart
                        var nearest = gesammteDaten.OrderBy(p => Math.Abs((p.Date - launch.Net.Value).TotalDays)).FirstOrDefault();

                        var item = new MoonAndRocketResponse
                        {
                            Date = launch.Net.Value,
                            MoonPhase = nearest?.PhaseName ?? "Unknown",
                            IsSuccess = launch.Status.Id == 3
                        };

                        results.Add(item);
                    }

                    return Ok(results);
                }
                else
                {
                    return BadRequest(httpResponse);
                }
            }
            else
            {
                if ((int)httpResponse.StatusCode == 429) // Too Many Requests
                {
                    return StatusCode(StatusCodes.Status429TooManyRequests, new { error = "Too Many Requests" });
                }
                return StatusCode(StatusCodes.Status502BadGateway, new { error = "Unbekannte Fehler" });
            }
        }
    }
}
