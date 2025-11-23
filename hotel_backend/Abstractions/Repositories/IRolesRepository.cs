using hotel_backend.Models;
using hotel_backend.Models.Filters;

namespace hotel_backend.Abstractions.Repositories;

public interface IRolesRepository
{
    Task<Guid> CreateRole(Role role, CancellationToken ct);

    Task<List<Role>> GetAllRoles(CancellationToken ct);

    Task<Guid> UpdateRole(Guid roleId, Role newRole, CancellationToken ct);

    Task<Guid> DeleteRole(Guid roleId, CancellationToken ct);
}