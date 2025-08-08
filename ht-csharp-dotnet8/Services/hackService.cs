using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Helpers;
using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Text;

namespace ht_csharp_dotnet8.Services
{
    public interface IhackService
    {
        Task<Response> throwex();
    }
    [ServiceDependencies]
    public class hackService(IRepository<Navigation> _repo, IRepository<NavigationRoles> _repoNaviRole, RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager) : IhackService
    {

        public async Task<Response> throwex()
        {
            var executed = false;
            executed = (await _repo.Find(x => x.Label == "Home")).Count > 0;
            if (!executed)
                await _repo.AddAsync(new Navigation()
                {
                    Label = $"Home",
                    Icon = "home",
                    Route = "/private",
                });

            executed = (await _repo.Find(x => x.Label == "Settings")).Count > 0;
            if (!executed)
                await _repo.AddAsync(new Navigation()
                {
                    Label = $"Settings",
                    Icon = "home",
                    Route = "",
                    Children = new List<Navigation>()
                    {
                        new Navigation()
                        {
                            Label = $"Navigation",
                            Icon = "home",
                            Route = "/private/maintenance/navigation",
                        },

                        new Navigation()
                        {
                            Label = $"Application Role",
                            Icon = "home",
                            Route = "/private/maintenance/application-role",
                        }
                    }
                });

            executed = (await _repoNaviRole.GetAllAsync()).Count > 0;
            if (!executed)
            {

                await laimsPrincipalFactory();
                var navigationList = await _repo.GetAllAsync();

                List<Guid> navigationIds = navigationList.Select(x => x.Id).ToList();

                var AdminRoleId = (await _roleManager.FindByNameAsync("Admin")).Id;

                foreach (var item in navigationIds)
                {
                    _repoNaviRole.AddAsync(new NavigationRoles()
                    {
                        RoleId = AdminRoleId,
                        NavigationId = item
                    });
                }
            }

            return new Response();
        }


        private async Task laimsPrincipalFactory()
        {
            RegisterModel model = new RegisterModel()
            {
                Email = "hongliang7622@gmail.com",
                Password = "devpassword",
                Username = "devusername"
            };
            var userExists = await _userManager.FindByNameAsync(model.Username);
          
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    sb.Append(error.Description + " ");
                }

            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Manager));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Supervisor))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Supervisor));
            if (!await _roleManager.RoleExistsAsync(UserRoles.Checker))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.Checker));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new ApplicationRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
        }
    }
}
