using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace ht_csharp_dotnet8.Middlewares
{
    public class HeaderCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HeaderCheckMiddleware> _logger;

        public HeaderCheckMiddleware(RequestDelegate next, ILogger<HeaderCheckMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //// Skip Swagger endpoints
            //if (context.Request.Path.StartsWithSegments("/swagger"))
            //{
            //    await _next(context);
            //    return;
            //}

            // 你想检查的 Header 名称
            const string headerKey = "X-Api-Key";

            if (!context.Request.Headers.TryGetValue(headerKey, out var extractedApiKey))
            {
                _logger.LogError("Missing X-Api-Key header");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsJsonAsync(new Response()
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = $"Missing X-Api-Key header"
                });
                return;
            }

            // 你可以在这里进行 Key 校验
            if (extractedApiKey != "expected-value")
            {
                _logger.LogError("Invalid X-Api-Key header value");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await context.Response.WriteAsJsonAsync(new Response()
                {
                    Status = HttpStatusCode.Unauthorized,
                    Message = $"Invalid X-Api-Key header"
                });
                return;
            }

            // 调用下一个中间件
            await _next(context);
        }
    }

}
