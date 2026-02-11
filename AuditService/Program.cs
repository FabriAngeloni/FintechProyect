using AuditService.Data;
using AuditService.RabbitMQ;
using AuditService.RabbitMQ.Consumers;
using AuditService.Repositories;
using AuditService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",optional: true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<AuditDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AuditConnection"))); 
builder.Services.AddControllers();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddScoped<IAuditServices, AuditServices>();
builder.Services.AddScoped<IAuditRepository,AuditRepository>();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));
var rabbit = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();
Console.WriteLine($"Host: {rabbit.Host}");
builder.Services.AddSingleton<RabbitMqOptions>();
builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddHostedService<UsuarioCreadoConsumer>();

builder.Host.UseSerilog();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
    db.Database.Migrate();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
