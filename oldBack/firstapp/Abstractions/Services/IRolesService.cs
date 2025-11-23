using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Abstractions.Services;

public interface IRolesService
{
    Task<Guid> CreateRoleAsync(Role role, CancellationToken ct);

    Task<Role> GetRoleAsync(Guid roleId, CancellationToken ct);

    Task<List<Role>> GetFilteredRolesAsync(RoleFilter? roleFilter, CancellationToken ct);

    Task<Guid> UpdateRoleAsync(Guid roleId, Role newRole, CancellationToken ct);

    Task<Guid> DeleteRoleAsync(Guid roleId, CancellationToken ct);
}