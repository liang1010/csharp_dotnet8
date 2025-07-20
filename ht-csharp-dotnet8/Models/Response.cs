using System.Net;

namespace ht_csharp_dotnet8.Models
{
    public class Response
    {
        public Response(string message = "Successful.") // fix typo
        {
            Status = HttpStatusCode.OK;
            Message = message;
        }

        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }

    public class Response<T> : Response
    {
        public Response(T? data = default, string message = "Successful.") : base(message)
        {
            Data = data;
        }

        public T? Data { get; set; }
    }

}
