using ht_csharp_dotnet8.Extensions;
using ht_csharp_dotnet8.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register serilog 
builder.Services.AddCustomSeriLog(builder.Configuration);
builder.Host.UseSerilog(); // <-- 注册 Serilog

builder.Services.AddScoped<IhackService, hackService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Catch thorw exception
app.UseCustomExceptionHandler();

app.UseHttpsRedirection();
app.UseCors(options => options
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithOrigins("*")
                     );
app.MapControllers();

app.Run();