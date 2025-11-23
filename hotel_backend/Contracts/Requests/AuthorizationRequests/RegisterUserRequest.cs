namespace hotel_backend.Contracts.Requests.AuthorizationRequests;

public record RegisterUserRequest(
    string Name,
    string Email,
    string Password
);