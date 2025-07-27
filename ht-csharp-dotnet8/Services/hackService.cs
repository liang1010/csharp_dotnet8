using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Helpers;
using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Identity;

namespace ht_csharp_dotnet8.Services
{
    public interface IhackService
    {
        Task<Response> throwex();
    }
    [ServiceDependencies]
    public class hackService(IRepository<Navigation> _repo) : IhackService
    {

        public async Task<Response> throwex()
        {
            await _repo.AddAsync(new Navigation()
            {
                Label = $"Home{DateTime.Now}",
                Icon = "home",
                Route = "/private",
            });

            await _repo.AddAsync(new Navigation()
            {
                Label = $"Settings{DateTime.Now}",
                Icon = "home",
                Route = "",
                Children = new List<Navigation>()
                    {
                        new Navigation()
                        {
                            Label = $"Navigation{DateTime.Now}",
                            Icon = "home",
                            Route = "/private/maintenance/navigation",
                        }
                    }
            });
            return new Response();
        }
    }
}
