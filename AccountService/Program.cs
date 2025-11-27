using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AccountService.Services;
using AccountService.Data;
using AccountService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CuentaDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("AccountConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<ICuentaServices, CuentaServices>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.MapControllers();
app.Run();
