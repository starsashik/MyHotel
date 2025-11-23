namespace firstapp.Contracts.Requests.AuthorizationRequests;

public record LoginUserRequest(
    string Email,
    string Password
    );