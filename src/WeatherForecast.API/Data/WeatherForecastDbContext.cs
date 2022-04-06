using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.API.Utilities;

namespace WeatherForecast.API.Data;

public class WeatherForecastDbContext : IdentityDbContext<IdentityUser>
{
    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
    : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        #region seeding preconfigured data
        SeedPreconfiguredRoles(builder);
        SeedPreconfiguredUser(builder);
        SeedUserRoles(builder);
        #endregion seeding preconfigured data
    }

    #region seeders
    /// <summary>
    /// Seed Preconfigured Roles
    /// </summary>
    /// <param name="builder"></param>
    private void SeedPreconfiguredRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = Constants.ADMIN_ROLE, ConcurrencyStamp = "1", NormalizedName = Constants.ADMIN_ROLE },
            new IdentityRole() { Id = "cbbf3ac1-c542-41de-abbc-a14fa6895724", Name = Constants.USER_ROLE, ConcurrencyStamp = "2", NormalizedName = Constants.USER_ROLE }
        );
    }

    /// <summary>
    /// Seed Preconfigured Users
    /// </summary>
    /// <param name="builder"></param>
    private void SeedPreconfiguredUser(ModelBuilder builder)
    {
        IdentityUser bobAsAdmin = new()
        {
            Id = "b74ddd14-6340-4840-95c2-db12554843e5",
            UserName = Constants.BOB_USERNAME,
            Email = Constants.BOB_EMAIL,
            LockoutEnabled = false
        };
        var passwordHasher = new PasswordHasher<IdentityUser>();
        bobAsAdmin.PasswordHash = passwordHasher.HashPassword(bobAsAdmin, Constants.BOB_PWD);

        builder.Entity<IdentityUser>().HasData(bobAsAdmin);
    }

    /// <summary>
    /// Seed Preconfigured User Roles
    /// </summary>
    /// <param name="builder"></param>
    private void SeedUserRoles(ModelBuilder builder)
    {
        var bobUserId = "fab4fac1-c546-41de-aebc-a14da6895711";
        var adminRoleId = "b74ddd14-6340-4840-95c2-db12554843e5";

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>() { RoleId = bobUserId, UserId = adminRoleId }
        );
    }
    #endregion seeders
}