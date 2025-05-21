namespace firstapp.Contracts.Requests.RoomsRequests;

public record UpdateRoomRequest(
    Guid Id,
    string? NewRoomNumber,
    string? NewRoomType,
    string? NewPricePerNight,
    IFormFile? ImageFile
    );