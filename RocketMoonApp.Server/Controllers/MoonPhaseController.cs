using Microsoft.AspNetCore.Mvc;
using RocketMoonApp.Server.Models.MoonModels;

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
    }
}
