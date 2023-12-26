
using CafeHub.Services.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CafeHub.Services.Helpers;
using CafeHub.Services.Background;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<ContextDB>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();
builder.Services.AddHostedService<BackgroundAnalyticsProcessor>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        options.TokenValidationParameters = AuthOptions.NewOptionsAuth());

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
