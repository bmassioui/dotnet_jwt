using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WeatherForecast.API.Data;

public class WeatherForecastDbContext : IdentityDbContext<IdentityUser>
{
    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
    : base(options)
    { }
}