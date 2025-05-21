namespace firstapp.Contracts.Requests.UsersRequests;

public record GetFilteredUsersRequest(
    string? PartOfName,
    string? PartOfEmail,
    Guid? Role
);