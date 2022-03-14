using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public partial class AuthenticateController : ControllerBase
{
    private IUserService _userService;

    public AuthenticateController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest model)
    {
        var response = await _userService.Login(model);
        if (response is null)
        {
            return Unauthorized();
        }

        return Ok(response);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        var errors = await _userService.Register(model);
        if (errors.Any())
        {
            return BadRequest(errors);
        }

        return Ok(new { message = "Registration successful. Please verify your email" });
    }

    [HttpGet]
    [Route("verify")]
    public async Task<IActionResult> EmailVerification(string email, string token)
    {
        var errors = await _userService.VerifyEmail(email, token);
        if (errors.Any())
        {
            return Redirect($"/?errors=${string.Join(",", errors.Select(e => e.Description))}"); ;
        }
        return Redirect("/?emailVerifiedNotice=true");
    }
}