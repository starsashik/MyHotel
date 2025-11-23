namespace hotel_backend.Contracts.Requests.UsersRequests;

public record GetFilteredUsersRequest(
    string? PartOfName,
    string? PartOfEmail,
    Guid? Role
);