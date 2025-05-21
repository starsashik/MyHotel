using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using firstapp.Abstractions.Services;
using firstapp.Contracts.Other;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using firstapp.Models.Filters;
using firstapp.Models.Others;
using Microsoft.IdentityModel.Tokens;

namespace firstapp.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUsersService _usersService;
    private readonly IConfiguration _configuration;
    private readonly IRolesService _rolesService;

    public AuthorizationService(IUsersService usersService, IConfiguration configuration,IRolesService rolesService)
    {
        _usersService = usersService;
        _configuration = configuration;
        _rolesService = rolesService;
    }

    public async Task<string> LoginUser(string username, string password, CancellationToken cancellationToken)
    {
        var user = (await _usersService.GetFilteredUsersAsync(null, cancellationToken))
            .FirstOrDefault(u => u.Email == username);

        if (user is null)
            throw new AuthorizationException("Invalid login or password");

        if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
            throw new AuthorizationException("Invalid login or password");

        var token = await GenerateJwtToken(user, cancellationToken);

        return token;
    }


    public async Task<string> RegisterUser(string name, string email, string password, CancellationToken cancellationToken)
    {
        var (roleFilter, roleFilterError) = RoleFilter
            .Create(null,
                7,
                7);

        if (!string.IsNullOrEmpty(roleFilterError))
            throw new ConversionException($"Incorrect data format: {roleFilterError}");

        var filteredRoles = await _rolesService
            .GetFilteredRolesAsync(roleFilter, cancellationToken);

        if (filteredRoles.IsNullOrEmpty())
        {
            throw new IntegrityException("Invalid role - not good");
        }

        var (user, error) = User.Create(
            Guid.NewGuid(),
            name,
            email,
            password,
            false,
            filteredRoles[0]);

        if (!string.IsNullOrEmpty(error))
        {
            throw new ConversionException($"Unable to convert user dto to user model. " +
                                          $"--> {error}");
        }

        await _usersService.CreateUserAsync(user, cancellationToken);

        var token = await GenerateJwtToken(user, cancellationToken);

        return token;
    }



    public async Task<TokenDto> ValidateToken(string token, CancellationToken cancellationToken)
    {
        var principal = await ValidateJwtToken(token, cancellationToken);

        try
        {
            var tokenData = new TokenDto(
                Guid.Parse(principal.FindFirst("UserId")?.Value!),
                principal.FindFirst("Name")?.Value!,
                principal.FindFirst("Email")?.Value!,
                Guid.Parse(principal.FindFirst("RoleId")?.Value!),
                principal.FindFirst("RoleGroup")?.Value!);

            return tokenData;
        }
        catch (Exception e)
        {
            throw new IntegrityException("Invalid data format in the token", e);
        }
    }

    private Task<string> GenerateJwtToken(User user, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                                         ?? throw new ConfigurationException("Jwt-key is missing"));

        var roleGroup = user.Role.AccessLevel switch
        {
            (int)AccessLevelEnumerator.SuperUser =>
                "SuperUser",
            >= (int)AccessLevelEnumerator.AdministratorMax
                and <= (int)AccessLevelEnumerator.AdministratorMin =>
                "Administrator",
            (int)AccessLevelEnumerator.Editor =>
                "Editor",
            (int)AccessLevelEnumerator.CommonUser =>
                "CommonUser",
            _ => string.Empty
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new("UserId", user.Id.ToString()),
                new("Name", user.Name),
                new("Email", user.Email),
                new("RoleId", user.Role.Id.ToString()),
                new("RoleGroup", roleGroup)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Task.FromResult(tokenHandler.WriteToken(token));
    }


    private Task<ClaimsPrincipal> ValidateJwtToken(string token, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                                         ?? throw new ConfigurationException("Jwt-key is missing"));

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateLifetime = true
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

        return Task.FromResult(principal);
    }
}
