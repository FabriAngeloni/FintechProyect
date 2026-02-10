using Microsoft.EntityFrameworkCore;
using Serilog;
using TransactionService.Clients;
using TransactionService.Data;
using TransactionService.Repositories;
using TransactionService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<TransaccionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TransactionConnection")));


builder.Services.AddHttpClient<IAccountClient,AccountHttpClient>();
builder.Services.AddControllers();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransaccionDbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

