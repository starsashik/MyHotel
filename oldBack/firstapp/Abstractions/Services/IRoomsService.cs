using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Abstractions.Services;

public interface IRoomsService
{
    Task<Guid> CreateRoomAsync(Room room, CancellationToken ct);
    Task<Room> GetRoomAsync(Guid roomId, CancellationToken ct);
    Task<List<Room>> GetFilteredRoomsAsync(RoomFilter? roomFilter, CancellationToken ct);
    Task<Guid> UpdateRoomAsync(Guid roomId, Room newRoom, CancellationToken ct);
    Task<Guid> DeleteRoomAsync(Guid roomId, CancellationToken ct);
}