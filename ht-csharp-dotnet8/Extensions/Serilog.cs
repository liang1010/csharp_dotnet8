using ht_csharp_dotnet8.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Net;

namespace ht_csharp_dotnet8.Extensions
{
    public static class Serilog
    {
        public static IServiceCollection AddCustomSeriLog(this IServiceCollection services, ConfigurationManager configuration)
        {

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration) // from appsettings
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithClientIp()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.MSSqlServer(
                    connectionString: configuration.GetConnectionString("MainDb"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = false
                    },
                    columnOptions: GetSqlColumnOptions()
                )
                .CreateLogger();
            return services;

        }
        private static ColumnOptions GetSqlColumnOptions()
        {
            var columnOptions = new ColumnOptions();

            // 移除不需要的默认列
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.LogEvent); // 如果你不想要原始 JSON

            // 修改标准列名
            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.Level.ColumnName = "Level";
            columnOptions.Message.ColumnName = "Message";

            // 添加额外字段为独立列（注意：这些字段必须存在于 log context 中）
            columnOptions.AdditionalColumns = new List<SqlColumn>
    {
        new SqlColumn { ColumnName = "ActionName", DataType = SqlDbType.NVarChar, DataLength = 256 },
        new SqlColumn { ColumnName = "MachineName", DataType = SqlDbType.NVarChar, DataLength = 128 },
        new SqlColumn { ColumnName = "Application", DataType = SqlDbType.NVarChar, DataLength = 128 },
        new SqlColumn { ColumnName = "ClientIp", DataType = SqlDbType.NVarChar, DataLength = 64 } // 若你有加 Enrich.WithClientIp()
    };

            return columnOptions;
        }



        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = feature?.Error;

                    // Default to 500
                    HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                    string message = "Something went wrong. Please check with the system administrator.";

                    if (exception is DbUpdateException)
                    {
                        statusCode = HttpStatusCode.BadRequest;
                        message = "Database update failed.";
                    }
                    else if (exception is DbUpdateConcurrencyException)
                    {
                        statusCode = HttpStatusCode.Conflict;
                        message = "Concurrency conflict occurred.";
                    }

                    context.Response.StatusCode = (int)statusCode;

                    Log.Error(exception, "Unhandled exception at {Path}", context.Request.Path);

                    await context.Response.WriteAsJsonAsync(new Response()
                    {
                        Status = statusCode,
                        Message = $"{message} Error: {exception?.Message}"
                    });
                });
            });

            return app;
        }
    }
}
