namespace hotel_backend.Contracts.Requests.UsersRequests;

public record UpdateUserRequestSmallParam(
    string UserEmail,
    string? NewName,
    IFormFile? ImageFile
);