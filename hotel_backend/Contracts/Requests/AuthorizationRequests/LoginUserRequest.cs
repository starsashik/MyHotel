namespace hotel_backend.Contracts.Requests.AuthorizationRequests;

public record LoginUserRequest(
    string Email,
    string Password
    );