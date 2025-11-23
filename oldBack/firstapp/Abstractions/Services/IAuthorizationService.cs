using firstapp.Contracts.Other;
using firstapp.Models;

namespace firstapp.Abstractions.Services;

public interface IAuthorizationService
{
    Task<string> LoginUser(string email, string password, CancellationToken ct);

    Task<string> RegisterUser(string name, string email, string password, CancellationToken ct);
    Task<TokenDto> ValidateToken(string token, CancellationToken ct);
}