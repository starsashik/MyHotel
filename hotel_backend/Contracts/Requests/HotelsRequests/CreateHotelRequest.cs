namespace hotel_backend.Contracts.Requests.HotelsRequests;

public record CreateHotelRequest(
    string Name,
    string Location,
    string Description,
    IFormFile? ImageFile
    );