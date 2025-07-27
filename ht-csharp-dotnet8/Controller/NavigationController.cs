using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;
using ht_csharp_dotnet8.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ht_csharp_dotnet8.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NavigationController(INavigationService _navigationService) : ControllerBase
    {
        [HttpGet]
        [Route("getNavigation")]
        public async Task<IActionResult> GetNavigation()
        {
            return Ok(await _navigationService.GetNavigation());
        }

        [HttpPost]
        [Route("getPaged")]
        public async Task<IActionResult> GetPaged([FromBody] PageListingRequest req)
        {
            return Ok(await _navigationService.GetPagedListingAsync(req));
        }

        [HttpPost]
        [Route("onEdit")]
        public async Task<IActionResult> Edit([FromBody] NavigationRolesModel req)
        {
            return Ok(await _navigationService.EditAsync(req));
        }

        [HttpPost]
        [Route("onDelete")]
        public async Task<IActionResult> Delete([FromBody] Navigation req)
        {
            return Ok(await _navigationService.DeleteAsync(req));
        }

        [HttpPost]
        [Route("getNaviRoles")]
        public async Task<IActionResult> getNaviRoles([FromBody] Guid req)
        {
            return Ok(await _navigationService.GetNaviRoles(req));
        }


    }
}
