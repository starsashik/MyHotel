namespace firstapp.Contracts.Requests.RoomsRequests;

public record GetFilteredRoomsRequest(
    Guid? HotelId,
    string? RoomType
    );