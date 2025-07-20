namespace ht_csharp_dotnet8.Services
{
    public interface IhackService
    {
        Task<string> throwex();
    }
    public class hackService: IhackService
    {

        public async Task<string> throwex()
        {
            throw new Exception();
        }
    }
}
