using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Models;

namespace ht_csharp_dotnet8.Services
{
    public interface IhackService
    {
        Task<Response> throwex();
    }
    [ServiceDependencies]
    public class hackService(IRepository<SystemConfig> _repo) : IhackService
    {

        public async Task<Response> throwex()
        {
            await _repo.AddAsync(new SystemConfig()
            {
                Code = "TSE1T",
                Value = "asd"
            });

            return new Response();
        }
    }
}
