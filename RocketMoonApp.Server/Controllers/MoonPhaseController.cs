using Microsoft.AspNetCore.Mvc;
using RocketMoonApp.Server.Models.MoonModels;
using RocketMoonApp.Server.Models.SunAndMoonModels;

namespace RocketMoonApp.Server.Controllers
{
    public class MoonPhaseController : Controller
    {
        private readonly HttpClient _httpClient;

        public MoonPhaseController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("MoonPhase")]
        public async Task<ActionResult<MoonPhaseApiResponse>> GetMoonPhaseApiResponse(int jahr)
        {
            var url = $"https://aa.usno.navy.mil/api/moon/phases/year?year={jahr}";
            var httpResponse = await _httpClient.GetAsync(url);

            if(!httpResponse.IsSuccessStatusCode)
                return BadRequest(httpResponse);

            var response = await httpResponse.Content.ReadFromJsonAsync<MoonPhaseApiResponse>();
            return Ok(response);
        }

        [HttpGet]
        [Route("MoonPhaseByDate")]
        public async Task<ActionResult<MoonPhaseApiResponse>> GetMoonPhaseByDateApiResponse(MoonPhaseApiRequest request)
        {
            var url = $"https://aa.usno.navy.mil/api/moon/phases/date?date={request.Date:yyyy-M-d}&nump={request.Nump}";
            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
                return BadRequest(httpResponse);

            var response = await httpResponse.Content.ReadFromJsonAsync<MoonPhaseApiResponse>();
            return Ok(response);
        }

        [HttpGet]
        [Route("SunAndMoon")]
        public async Task<ActionResult<MoonPhaseApiResponse>> GetSunAndMoonApiResponse(SunAndMoonRequest request)
        {
            var url = $"https://aa.usno.navy.mil/api/rstt/oneday?date={request.Date:yyyy-M-d}&coords={string.Create(System.Globalization.CultureInfo.InvariantCulture, $"{request.Latitude},{request.Longitude}")}";

            if (request.TimeZone != null)
                url += $"&tz={request.TimeZone}";
            if (request.DST != null)
                url += $"&dst={request.DST}";

            var httpResponse = await _httpClient.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
                return BadRequest(httpResponse);

            var response = await httpResponse.Content.ReadFromJsonAsync<SunAndMoonResponse>();
            return Ok(response);
        }
    }
}
