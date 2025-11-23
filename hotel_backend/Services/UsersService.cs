using hotel_backend.Abstractions.Repositories;
using hotel_backend.Abstractions.Services;
using hotel_backend.Exceptions.SpecificExceptions;
using hotel_backend.Models;
using hotel_backend.Models.Filters;

namespace hotel_backend.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Guid> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        var createdUserId = await _usersRepository.CreateUser(user, cancellationToken);

        return createdUserId;
    }

    public async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var allUsers = await _usersRepository.GetAllUsers(cancellationToken);

        var user = allUsers.FirstOrDefault(u => u.Id == userId);

        if (user == null)
            throw new UnknownIdentifierException("Unknown user id");

        return user;
    }

    public async Task<List<User>> GetFilteredUsersAsync(UserFilter? userFilter, CancellationToken cancellationToken)
    {
        var users = await _usersRepository.GetAllUsers(cancellationToken);

        if (userFilter is null)
            return users;

        if (userFilter.PartOfName is not null)
        {
            users = users
                .Where(u => u.Name.ToLower().Contains(userFilter.PartOfName.ToLower()))
                .ToList();
        }

        if (userFilter.PartOfEmail is not null)
        {
            users = users
                .Where(u => u.Email.ToLower().Contains(userFilter.PartOfEmail.ToLower()))
                .ToList();
        }

        if (userFilter.Role is not null)
        {
            users = users
                .Where(u => u.Role.Id == userFilter.Role)
                .ToList();
        }

        return users.OrderBy(u => u.Name).ToList();
    }

    public async Task<Guid> UpdateUserAsync(Guid userId, User newUser, CancellationToken cancellationToken)
    {
        var updatedUserId = await _usersRepository.UpdateUser(userId, newUser, cancellationToken);

        return updatedUserId;
    }

    public async Task<Guid> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var deletedUserId = await _usersRepository.DeleteUser(userId, cancellationToken);

        return deletedUserId;
    }
}