using RateLimiter.Api.Attributes;
using RateLimiter.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(builder.Configuration, opt =>
{
    opt.UseConfigurationOptions = true;
    opt.RedisConnection = builder.Configuration.GetConnectionString("Redis") ?? string.Empty;
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiterMiddleware();

app.MapGet("/test",
    [Domain("test")]
[Action("test")]
() => "ok");

app.MapControllers();

app.Run();
