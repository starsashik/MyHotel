﻿using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Abstractions.Repositories;

public interface IRolesRepository
{
    Task<Guid> CreateRole(Role role, CancellationToken ct);

    Task<List<Role>> GetAllRoles(CancellationToken ct);

    Task<Guid> UpdateRole(Guid roleId, Role newRole, CancellationToken ct);

    Task<Guid> DeleteRole(Guid roleId, CancellationToken ct);
}