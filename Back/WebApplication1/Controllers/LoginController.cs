using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/[action]")]
public class LoginController : ControllerBase
{
    // Запрос на создание ключа с ролью и именем пользователя
    [HttpGet("{role}/{username}")]
    public IActionResult Login(string role, string username)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult AdminData()
    {
        return Ok("Administrator data");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Data()
    {
        return Ok("Usual data");
    }
}