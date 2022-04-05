using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.API.Dtos;
using WeatherForecast.API.Dtos.Auth;
using static WeatherForecast.API.Utilities.Enums;

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
        var user = await _userManager.FindByNameAsync(loginDto.Username);

        if (user is null) return BadRequest("Invalid Username.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!isPasswordValid) return BadRequest("Invalid Password.");

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (userRoles.Any()) authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = GenerateJwt();

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });

    }

    /// <summary>
    /// Register - Async
    /// <remarks>User will be creatd with User Role</remarks>
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        var isUserAlreadyExists = await _userManager.FindByNameAsync(registerDto.Username) is not null;

        if (isUserAlreadyExists) return BadRequest($"User {registerDto.Username} already exists, Please try to login.");

        IdentityUser userToCreate = new()
        {
            Email = registerDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDto.Username
        };

        var registeringResult = await _userManager.CreateAsync(userToCreate, registerDto.Password);

        // CodeStatus: 500 should not be shown to consumer, only maintainer who should notified with what's going wrong during the registration
        if (!registeringResult.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User creation failed! Please check user details and try again." });

        // Mark the new user as simple User
        var addingUserToRoleResult = await _userManager.AddToRoleAsync(userToCreate, Roles.User.ToString());

        // CodeStatus: 500 should not be shown to consumer, only maintainer who should notified with what's going wrong during the registration
        if (!addingUserToRoleResult.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User creation failed!" });

        return Created(string.Empty, new ResponseDto { Status = "Success", Message = "User created successfully!" });
    }

    /// <summary>
    /// Add User to Role - Async
    /// </summary>
    /// <param name="username"></param>
    /// <param name="rolename">{Admin, User}</param>
    /// <returns></returns>
    [HttpPost]
    [Route("add/{username}/role")]
    public async Task<IActionResult> AddUserToRoleAssync(string username, [FromBody] string rolename)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null) return BadRequest($"{username} is invalid as Username.");

        var role = _roleManager.FindByNameAsync(rolename);

        if (role is null) return BadRequest($"{rolename} is invalid as RoleName, Please contact administrator for creating new role.");

        var addingUserToRoleResult = await _userManager.AddToRoleAsync(user, rolename);

        // CodeStatus: 500 should not be shown to consumer, only maintainer who should notified with what's going wrong during the registration
        if (!addingUserToRoleResult.Succeeded) return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User creation failed!" });

        return Ok(new ResponseDto { Status = "Success", Message = $"User:{username} added successfully to role:{rolename}." });

    }

    #region Jwt Generator
    private JwtSecurityToken GenerateJwt()
    {
        throw new NotImplementedException();
    }
    #endregion Jwt Generator


}