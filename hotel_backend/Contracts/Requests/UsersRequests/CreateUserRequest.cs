namespace hotel_backend.Contracts.Requests.UsersRequests;

public record CreateUserRequest(
    string Name,
    string Email,
    string Password,
    Guid Role
);