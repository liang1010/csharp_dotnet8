//Command for migration db in Package Manager Console
// dotnet tool install --global dotnet-ef --version 6
//     add-migration MyFirstMigration
//    Update-Database

//dotnet ef migrations add InitialDatabase --project Run
//dotnet ef database update --project Run

using ht_csharp_dotnet8.Entities;
using ht_csharp_dotnet8.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// For Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.ConfigureWarnings(warning =>
    {
        warning.Ignore(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
    });
    options.UseSqlServer(builder.Configuration.GetConnectionString("MainDb"), b => b.MigrationsAssembly("ht-csharp-dotnet8"));
});

// For Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // 密码设置
    options.Password.RequireDigit = false; // 是否必须包含数字
    options.Password.RequiredLength = 4;  // 最小长度
    options.Password.RequireNonAlphanumeric = false; // 是否必须包含特殊字符
    options.Password.RequireUppercase = false; // 是否必须包含大写
    options.Password.RequireLowercase = false; // 是否必须包含小写
    options.Password.RequiredUniqueChars = 0; // 最少不同字符数量

    // 锁定设置（可选）
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.AllowedForNewUsers = true;

    // 用户设置（可选）
    options.User.RequireUniqueEmail = false;
});

// Register serilog 
builder.Services.AddCustomSeriLog(builder.Configuration);
builder.Host.UseSerilog(); // <-- 注册 Serilog

// Add JWT
builder.Services.AddCustomJWT(builder.Configuration);

// Register Dependencies
builder.Services.AddCustomServiceDependencies();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Catch thorw exception
app.UseCustomExceptionHandler();

app.UseHttpsRedirection();
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options => options
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithOrigins("*")
                     );
app.MapControllers();

app.Run();