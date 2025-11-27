using AccountService.Data;
using AccountService.Repositories;
using Microsoft.EntityFrameworkCore;
using TransactionService.Data;
using TransactionService.Repositories;
using TransactionService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CuentaDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("AccountConnection")));
builder.Services.AddDbContext<TransaccionDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("TransactionConnection")));



builder.Services.AddControllers();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
