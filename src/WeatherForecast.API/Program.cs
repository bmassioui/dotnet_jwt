using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.API.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext EF
builder.Services.AddDbContext<WeatherForecastDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WeatherForecastConnectionStrings")));

// Identity EF
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<WeatherForecastDbContext>()
.AddDefaultTokenProviders();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
