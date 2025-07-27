using ht_csharp_dotnet8.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ht_csharp_dotnet8.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LoggerController(IhackService hackService) : ControllerBase
    {

        [HttpGet]
        [Route("log")]
        public async Task<IActionResult> Log()
        {
            return Ok(await hackService.throwex());
        }
    }
}
