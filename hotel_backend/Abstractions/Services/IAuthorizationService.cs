using hotel_backend.Contracts.Other;
using hotel_backend.Models;

namespace hotel_backend.Abstractions.Services;

public interface IAuthorizationService
{
    Task<string> LoginUser(string email, string password, CancellationToken ct);

    Task<string> RegisterUser(string name, string email, string password, CancellationToken ct);
    Task<TokenDto> ValidateToken(string token, CancellationToken ct);
}