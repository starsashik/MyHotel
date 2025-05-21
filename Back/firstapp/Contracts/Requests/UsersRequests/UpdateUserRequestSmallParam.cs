namespace firstapp.Contracts.Requests.UsersRequests;

public record UpdateUserRequestSmallParam(
    string UserEmail,
    string? NewName,
    IFormFile? ImageFile
);