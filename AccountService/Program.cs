using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AccountService.Services;
using AccountService.Data;
using AccountService.Repositories;
using Serilog;
using AccountService.Messaging.RabbitMQ;
using AccountService.Messaging.Consumers;
using AccountService.Messaging.Abstractions;

var builder = WebApplication.CreateBuilder(args);
//con docker se usara el appsettings.json  y localmente se usara appsettings.development.json
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",optional: true)
    .AddEnvironmentVariables();
builder.Services.AddDbContext<CuentaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AccountConnection")));
builder.Services.AddControllers();

builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddSingleton<IMessagePublisher,RabbitMqPublisher>();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

//builder.Services.Configure<RabbitMqOptions> mapea la seccion al objeto RabbitMqOptions
//builder.configuration.GetSection("RabbitMQ") toma la seccion llamada "RabbitMQ" del appsettings.json y la mapea al objeto que se quiera (RabbitMqOptions)
var rabbit = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));
Console.WriteLine($"RABBITMQ host: {rabbit.Host}");

//Registras que solo va a existir una instancia que usaran todas las clases de ser necesario.
builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<RabbitMqOptions>();
builder.Services.AddHostedService<UsuarioCreadoConsumer>();

builder.Host.UseSerilog();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CuentaDbContext>();
    db.Database.Migrate();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
