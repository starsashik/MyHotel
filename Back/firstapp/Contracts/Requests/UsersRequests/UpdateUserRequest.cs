namespace firstapp.Contracts.Requests.UsersRequests;

public record UpdateUserRequest(
    Guid UserId,
    string NewName,
    string NewEmail,
    string NewPassword,
    Guid NewRoleId
);