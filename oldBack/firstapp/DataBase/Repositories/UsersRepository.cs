using firstapp.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using firstapp.Models;
using firstapp.DataBase.Contexts;
using firstapp.Exceptions.SpecificExceptions;

namespace firstapp.DataBase.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly MyDbContext _context;

    public UsersRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
    {
        var userEntity = new DataBase.Entities.User
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            RoleId = user.Role.Id,
            ImgUrl = user.ImgUrl
        };

        await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return userEntity.Id;
    }

    public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken)
    {
        var userEntities = await _context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var users = userEntities
            .Select(userEntity =>
            {
                var (role, roleError) = Role
                    .Create(userEntity.Role.Id,
                        userEntity.Role.Name,
                        userEntity.Role.LevelAccess);

                if (!string.IsNullOrEmpty(roleError))
                    throw new IntegrityException($"Incorrect data format in the database, unable to create a " +
                                                 $"role model: {roleError}");

                var (user, userError) = User
                    .Create(userEntity.Id,
                        userEntity.Name,
                        userEntity.Email,
                        userEntity.PasswordHash,
                        true,
                        role,
                        userEntity.ImgUrl);

                if (!string.IsNullOrEmpty(userError))
                    throw new IntegrityException($"Incorrect data format in the database, unable to create a " +
                                                 $"user model: {userError}");

                return user;
            })
            .ToList();

        return users;
    }

    public async Task<Guid> UpdateUser(Guid userId, User newUser, CancellationToken cancellationToken)
    {
        var oldUserEntity = await _context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (oldUserEntity is null)
            throw new UnknownIdentifierException("Unknown user id");

        await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.Name, u => newUser.Name)
                    .SetProperty(u => u.Email, u => newUser.Email)
                    .SetProperty(u => u.PasswordHash, u => newUser.PasswordHash)
                    .SetProperty(u => u.RoleId, u => newUser.Role.Id)
                    .SetProperty(u => u.ImgUrl, u => newUser.ImgUrl),
                cancellationToken);

        return oldUserEntity.Id;
    }

    public async Task<Guid> DeleteUser(Guid userId, CancellationToken cancellationToken)
    {
        var numDeleted = await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);

        if (numDeleted == 0)
            throw new UnknownIdentifierException("Unknown user id");

        return userId;
    }
}
