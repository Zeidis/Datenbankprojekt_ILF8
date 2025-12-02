using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetMoonAndRocket([FromQuery] DateTime? seit, [FromQuery] DateTime? bis)
        {
            // Daten sollen in ISO 8601 Format sein
            string startDateStr = seit.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string endDateStr = bis.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");

            string url = $"https://lldev.thespacedevs.com/2.3.0/launches/?net__gte={Uri.EscapeDataString(startDateStr)}&net__lte={Uri.EscapeDataString(endDateStr)}&mode=normal&limit=100";

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

                foreach(var result in rocketResponse.Results)
                {
                    url = $"https://aa.usno.navy.mil/api/rstt/oneday?date={result.Net:yyyy-M-d}&coords={string.Create(System.Globalization.CultureInfo.InvariantCulture, $"{result.Pad.Latitude},{result.Pad.Longitude}")}";
                    httpResponse = await _httpClient.GetAsync(url);

                }
                return Ok(rocketResponse);
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
