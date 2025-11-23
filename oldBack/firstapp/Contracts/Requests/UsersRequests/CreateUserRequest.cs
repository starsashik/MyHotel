namespace firstapp.Contracts.Requests.UsersRequests;

public record CreateUserRequest(
    string Name,
    string Email,
    string Password,
    Guid Role
);