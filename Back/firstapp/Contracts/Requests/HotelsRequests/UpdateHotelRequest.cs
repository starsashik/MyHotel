namespace firstapp.Contracts.Requests.HotelsRequests;

public record UpdateHotelRequest(
    Guid HotelId,
    string? NewName,
    string? NewLocation,
    string? NewDescription,
    IFormFile? ImageFile
    );