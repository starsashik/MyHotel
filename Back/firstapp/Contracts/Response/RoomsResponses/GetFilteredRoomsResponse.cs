using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.RoomsResponse;

public record GetFilteredRoomsResponse(
    List<RoomDto> Rooms
    );