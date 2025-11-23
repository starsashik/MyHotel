using firstapp.Abstractions.Repositories;
using firstapp.DataBase.Contexts;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using Microsoft.EntityFrameworkCore;


namespace firstapp.DataBase.Repositories;

public class RolesRepository : IRolesRepository
{
    private readonly MyDbContext _context;

    public RolesRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateRole(Role role, CancellationToken cancellationToken)
    {
        var roleEntity = new DataBase.Entities.Role
        {
            Id = role.Id,
            Name = role.Name,
            LevelAccess = role.AccessLevel
        };

        await _context.Roles.AddAsync(roleEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return roleEntity.Id;
    }

    public async Task<List<Role>> GetAllRoles(CancellationToken cancellationToken)
    {
        var roleEntities = await _context.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var roles = roleEntities
            .Select(roleEntity =>
            {
                var (role, roleError) = Role
                    .Create(roleEntity.Id,
                        roleEntity.Name,
                        roleEntity.LevelAccess);

                if (!string.IsNullOrEmpty(roleError))
                    throw new IntegrityException($"Incorrect data format in the database, unable to create a " +
                                                 $"role model: {roleError}");

                return role;
            })
            .ToList();

        return roles;
    }

    public async Task<Guid> UpdateRole(Guid roleId, Role newRole, CancellationToken cancellationToken)
    {
        var oldRoleEntity = await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

        if (oldRoleEntity is null)
            throw new UnknownIdentifierException("Unknown role id");

        await _context.Roles
            .Where(r => r.Id == roleId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(r => r.Name, r => newRole.Name)
                    .SetProperty(r => r.LevelAccess, r => newRole.AccessLevel),
                cancellationToken);

        return oldRoleEntity.Id;
    }

    public async Task<Guid> DeleteRole(Guid roleId, CancellationToken cancellationToken)
    {
        var numDeleted = await _context.Roles
            .Where(r => r.Id == roleId)
            .ExecuteDeleteAsync(cancellationToken);

        if (numDeleted == 0)
            throw new UnknownIdentifierException("Unknown role id");

        return roleId;
    }
}