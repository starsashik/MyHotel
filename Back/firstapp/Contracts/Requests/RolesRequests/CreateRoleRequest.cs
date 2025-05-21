namespace firstapp.Contracts.Requests.RolesRequests;

public record CreateRoleRequest(
    string Name,
    int AccessLevel
);