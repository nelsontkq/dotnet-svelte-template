using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DotnetSvelteAuthApp.Models;
using DotnetSvelteAuthApp.Models.Data;

public interface IUserService
{
    public Task<LoginResponse?> Login(LoginRequest model);
    public Task<IEnumerable<IdentityError>> Register(RegisterRequest model);
    public Task<IEnumerable<IdentityError>> VerifyEmail(string email, string token);
}

public class UserService : IUserService
{
    private UserManager<ApplicationUser> _userManager;
    private RoleManager<IdentityRole> _roleManager;

    private IEmailService _emailService;
    private Jwt _jwtSettings;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailService emailService,
        IOptions<Jwt> jwtSettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailService = emailService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<LoginResponse?> Login(LoginRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            var token = GetToken(authClaims);
            var response = new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email,
                UserName = user.UserName,
                Expiration = token.ValidTo
            };
            return response;
        }

        return null;
    }

    public async Task<IEnumerable<IdentityError>> Register(RegisterRequest model)
    {
        var userExists = await _userManager.Users.CountAsync(u => u.Email == model.Email && u.EmailConfirmed) > 1;
        if (userExists)
        {
            return new[] { new IdentityError { Description = "User already exists" } };
        }
        var user = new ApplicationUser
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.UserName,
        };
        var identityResult = await _userManager.CreateAsync(user, model.Password);

        if (!identityResult.Succeeded)
        {
            return identityResult.Errors;
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _emailService.SendVerificationEmail(user, token);

        return Enumerable.Empty<IdentityError>();
    }

    public async Task<IEnumerable<IdentityError>> VerifyEmail(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result == null) return Enumerable.Empty<IdentityError>();
        if (result.Succeeded)
        {
            var addRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User);
            return addRoleResult.Errors;
        }

        return result.Errors;
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var secret = _jwtSettings.Secret;
        if (secret is null)
        {
            throw new Exception("JWT:Secret environment variable is not set");
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}