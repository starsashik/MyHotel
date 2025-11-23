namespace firstapp.Contracts.Response.RoomsResponse;

public record TestRooms(
    string RoomId,
    int RoomNumber,
    int RoomType,
    int PricePerNight,
    string ImgUrl,
    string HotelId
    );