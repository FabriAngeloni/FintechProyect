using NotificationService.Consumers.Identity;
using NotificationService.Events.Identity;
using NotificationService.Messaging;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine($"ENV = {builder.Environment.EnvironmentName}");
Console.WriteLine(
    File.Exists("appsettings.Development.json")
        ? "DEV FILE FOUND"
        : "DEV FILE MISSING"
);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
var rabbit = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();
Console.WriteLine($"RabbitMQ Host = {rabbit.Host}");

builder.Services.AddControllers();
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddHostedService<UsuarioCreadoConsumer>();



var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
