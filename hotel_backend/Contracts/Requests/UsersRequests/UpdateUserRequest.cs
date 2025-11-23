namespace hotel_backend.Contracts.Requests.UsersRequests;

public record UpdateUserRequest(
    Guid UserId,
    string NewName,
    string NewEmail,
    string NewPassword,
    Guid NewRoleId
);