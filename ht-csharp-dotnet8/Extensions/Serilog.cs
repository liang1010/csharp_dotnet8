using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;

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
                    connectionString: configuration.GetConnectionString("LogDb"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: GetSqlColumnOptions()
                )
                .CreateLogger();
            return services;

        }
        private static ColumnOptions GetSqlColumnOptions()
        {
            var columnOptions = new ColumnOptions();

            // 默认不包含这些列，手动启用：
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            //columnOptions.Store.Add(StandardColumn.LogEvent);

            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.Level.ColumnName = "Level";
            columnOptions.Message.ColumnName = "Message";
            // 添加你要的字段为独立列
            columnOptions.AdditionalColumns = new List<SqlColumn>
{
    new SqlColumn { ColumnName = "ActionName", DataType = SqlDbType.NVarChar, DataLength = 256 },
    new SqlColumn { ColumnName = "MachineName", DataType = SqlDbType.NVarChar, DataLength = 128 },
    new SqlColumn { ColumnName = "Application", DataType = SqlDbType.NVarChar, DataLength = 128 }
};
            return columnOptions;
        }


        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder services)
        {
            services.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    if (exception != null)
                    {
                        Log.Error(exception, "Unhandled exception at {Path}", context.Request.Path);
                    }

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json"; // or 
                    await context.Response.WriteAsJsonAsync(new
                    {
                        status = context.Response.StatusCode,
                        message = "Something went wrong. Please check with system admin"
                    });
                });
            });
            return services;
        }
    }
}
