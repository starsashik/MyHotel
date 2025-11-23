namespace firstapp.Contracts.Requests.RoomsRequests;

public record CreateRoomRequest(
    Guid HotelId,
    string RoomNumber,
    string RoomType,
    string PricePerNight,
    IFormFile? ImageFile
    );