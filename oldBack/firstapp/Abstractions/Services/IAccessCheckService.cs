using firstapp.Models;

namespace firstapp.Abstractions.Services;

public interface IAccessCheckService
{
    Task<User> CheckAccessLevel(HttpContext context, int minAccessLevel, CancellationToken ct);
}