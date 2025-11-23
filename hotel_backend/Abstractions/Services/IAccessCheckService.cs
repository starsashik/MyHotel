using hotel_backend.Models;

namespace hotel_backend.Abstractions.Services;

public interface IAccessCheckService
{
    Task<User> CheckAccessLevel(HttpContext context, int minAccessLevel, CancellationToken ct);
}