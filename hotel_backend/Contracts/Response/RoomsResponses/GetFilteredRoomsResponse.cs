using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.RoomsResponse;

public record GetFilteredRoomsResponse(
    List<RoomDto> Rooms
    );