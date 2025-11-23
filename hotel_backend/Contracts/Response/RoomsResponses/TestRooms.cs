namespace hotel_backend.Contracts.Response.RoomsResponse;

public record TestRooms(
    string RoomId,
    int RoomNumber,
    int RoomType,
    int PricePerNight,
    string ImgUrl,
    string HotelId
    );