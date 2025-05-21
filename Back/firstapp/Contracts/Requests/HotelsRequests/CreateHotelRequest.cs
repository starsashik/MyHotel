namespace firstapp.Contracts.Requests.HotelsRequests;

public record CreateHotelRequest(
    string Name,
    string Location,
    string Description,
    IFormFile? ImageFile
    );