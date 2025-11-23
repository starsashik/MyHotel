namespace hotel_backend.Contracts.Requests.RolesRequests;

public record CreateRoleRequest(
    string Name,
    int AccessLevel
);