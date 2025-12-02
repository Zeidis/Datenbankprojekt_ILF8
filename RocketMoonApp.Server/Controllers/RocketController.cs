using Microsoft.AspNetCore.Mvc;
using RocketMoonApp.Server.Models.RocketModels;

namespace RocketMoonApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RocketController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RocketController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("GetLaunchByDate")]
        public async Task<IActionResult> GetLaunchesByDate([FromQuery] DateTime? seit, [FromQuery] DateTime? bis)
        {
            // Daten sollen in ISO 8601 Format sein
            string startDateStr = seit.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string endDateStr = bis.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");

            string url = $"https://lldev.thespacedevs.com/2.3.0/launches/?net__gte={Uri.EscapeDataString(startDateStr)}&net__lte={Uri.EscapeDataString(endDateStr)}&mode=normal&limit=100";

            var httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<LaunchResponse>();

                var page = response.Next;

                while (page != null)
                {
                    var tempHttpResonse = await _httpClient.GetAsync(page);
                    var tempResponse = await tempHttpResonse.Content.ReadFromJsonAsync<LaunchResponse>();
                    page = tempResponse.Next;
                    response.Results.AddRange(tempResponse.Results);
                }

                return Ok(response);
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

        [HttpGet]
        [Route("GetLaunchByJear")]
        public async Task<IActionResult> GetLaunchesByJear([FromQuery] int jahr)
        {
            string url = $"https://lldev.thespacedevs.com/2.3.0/launches/?limit=100&mode=normal&year={jahr}";

            var httpResponse = await _httpClient.GetAsync(url);
            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<LaunchResponse>();

                var page = response.Next;

                while (page != null)
                {
                    var tempHttpResonse = await _httpClient.GetAsync(page);
                    var tempResponse = await tempHttpResonse.Content.ReadFromJsonAsync<LaunchResponse>();
                    page = tempResponse.Next;
                    response.Results.AddRange(tempResponse.Results);
                }

                return Ok(response);
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
