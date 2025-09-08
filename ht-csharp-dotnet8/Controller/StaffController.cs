using ht_csharp_dotnet8.Models;
using ht_csharp_dotnet8.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ht_csharp_dotnet8.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StaffController(IStaffService _staffService) : ControllerBase
    {
        [HttpPost]
        [Route("getPaged")]
        public async Task<IActionResult> GetPaged([FromBody] PageListingRequest req)
        {
            return Ok(await _staffService.GetPagedListingAsync(req));
        }

        [HttpPost]
        [Route("getLazyLoad")]
        public async Task<IActionResult> GetLazyLoad([FromBody] List<Guid> ids)
        {
            return Ok(await _staffService.GetLazyLoad(ids));
        }

        //[HttpPost]
        //[Route("onAdd")]
        //public async Task<IActionResult> Create([FromBody] ApplicationRole req)
        //{
        //    await _roleManager.CreateAsync(req);
        //    return Ok(new Response("Created Successful"));
        //}

        //[HttpPost]
        //[Route("onEdit")]
        //public async Task<IActionResult> Edit([FromBody] ApplicationRole req)
        //{
        //    await _roleManager.UpdateAsync(req);
        //    return Ok(new Response("Edit Successful"));
        //}

        //[HttpPost]
        //[Route("onDelete")]
        //public async Task<IActionResult> Delete([FromBody] ApplicationRole req)
        //{
        //    await _roleManager.DeleteAsync(req);           ;
        //    return Ok(new Response("Delete Successful"));
        //}

        //[HttpPost]
        //[Route("getApplicationRole")]
        //public async Task<IActionResult> getNaviRoles([FromBody] string req)
        //{
        //    return Ok(await _roleManager.FindByIdAsync(req));
        //}


    }
}
