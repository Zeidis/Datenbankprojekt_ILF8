using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RocketMoonApp.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MoonAndRocketController : ControllerBase
    {
        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test([FromQuery] DateTime? seit, [FromQuery] DateTime? bis)
        {
            return Ok("Test");
        }
    }
}
