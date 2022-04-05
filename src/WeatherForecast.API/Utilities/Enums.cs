using System.ComponentModel;

namespace WeatherForecast.API.Utilities;

public static class Enums
{
    public enum Roles : ushort
    {
        [Description("Admin role")]
        Admin = 0,
        [Description("User role")]
        User = 1
    }
}