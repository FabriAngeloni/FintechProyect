using AssetService.Data;
using AssetService.Models;
using AssetService.Models.Factory_Creator;
using AssetService.Models.Factory_Creator.Modifier;
using AssetService.Repositories;
using AssetService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<AssetDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AssetConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IActivoService, ActivoService>();
builder.Services.AddScoped<IActivoRepository, ActivoRepository>();

builder.Services.AddScoped<IActivoFactory, ActivoFactory>();
builder.Services.AddScoped<IActivoCreator, AccionCreator>();
builder.Services.AddScoped<IActivoCreator, BonoCreator>();
builder.Services.AddScoped<IActivoCreator, FondoCreator>();

builder.Services.AddScoped<IActivoModifierFactory, ActivoModifierFactory>();
builder.Services.AddScoped<IActivoModifier, AccionModifier>();
builder.Services.AddScoped<IActivoModifier, BonoModifier>();
builder.Services.AddScoped<IActivoModifier, FondoModifier>();


builder.Configuration.GetConnectionString("AssetConnection");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AssetDbContext>();
    db.Database.Migrate();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
