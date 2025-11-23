using hotel_backend.Models;

namespace hotel_backend.Abstractions.Repositories;

public interface IUsersRepository
{
    Task<Guid> CreateUser(User user, CancellationToken ct);
    Task<List<User>> GetAllUsers(CancellationToken ct);
    Task<Guid> UpdateUser(Guid userId, User newUser, CancellationToken ct);
    Task<Guid> DeleteUser(Guid userId, CancellationToken ct);
}