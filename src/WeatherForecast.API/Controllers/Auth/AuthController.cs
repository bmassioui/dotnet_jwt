using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.API.Dtos.Auth;

namespace WeatherForecast.API.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Invalid UserManager");
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager), "Invalid RoleManager");
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Invalid Configuration");
    }

    /// <summary>
    /// Login - Async
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Register - Async
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Register Admin - Async
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("register-admin")]
    public async Task<IActionResult> RegisterAdminAsync([FromBody] RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    #region Jwt Generator
    private JwtSecurityToken GenerateJwt()
    {
        throw new NotImplementedException();
    }
    #endregion Jwt Generator


}