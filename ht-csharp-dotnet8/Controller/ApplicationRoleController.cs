using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;
using ht_csharp_dotnet8.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ht_csharp_dotnet8.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationRoleController(IApplicationRoleService _applicationRoleService, RoleManager<ApplicationRole> _roleManager) : ControllerBase
    {
        [HttpPost]
        [Route("getPaged")]
        public async Task<IActionResult> GetPaged([FromBody] PageListingRequest req)
        {
            return Ok(await _applicationRoleService.GetPagedListingAsync(req));
        }

        [HttpPost]
        [Route("onAdd")]
        public async Task<IActionResult> Create([FromBody] ApplicationRole req)
        {
            await _roleManager.CreateAsync(req);
            return Ok(new Response("Created Successful"));
        }

        [HttpPost]
        [Route("onEdit")]
        public async Task<IActionResult> Edit([FromBody] ApplicationRole req)
        {
            await _roleManager.UpdateAsync(req);
            return Ok(new Response("Edit Successful"));
        }

        [HttpPost]
        [Route("onDelete")]
        public async Task<IActionResult> Delete([FromBody] ApplicationRole req)
        {
            await _roleManager.DeleteAsync(req);           ;
            return Ok(new Response("Delete Successful"));
        }

        [HttpPost]
        [Route("getApplicationRole")]
        public async Task<IActionResult> getNaviRoles([FromBody] string req)
        {
            return Ok(await _roleManager.FindByIdAsync(req));
        }


    }
}
