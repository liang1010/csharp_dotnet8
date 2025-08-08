using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ht_csharp_dotnet8.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DropDownController(RoleManager<ApplicationRole> _roleManager) : ControllerBase
    {

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> Log()
        {
            return Ok(new Response<List<ApplicationRole>>(message: "Load Successful.")
            {
                Data = await _roleManager.Roles.ToListAsync()
            });
        }
    }
}
