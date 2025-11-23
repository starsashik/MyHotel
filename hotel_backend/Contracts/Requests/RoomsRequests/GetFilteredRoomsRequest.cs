namespace hotel_backend.Contracts.Requests.RoomsRequests;

public record GetFilteredRoomsRequest(
    Guid? HotelId,
    string? RoomType
    );