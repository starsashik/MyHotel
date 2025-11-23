using hotel_backend.Models;
using hotel_backend.Models.Filters;

namespace hotel_backend.Abstractions.Services;

public interface IRoomsService
{
    Task<Guid> CreateRoomAsync(Room room, CancellationToken ct);
    Task<Room> GetRoomAsync(Guid roomId, CancellationToken ct);
    Task<List<Room>> GetFilteredRoomsAsync(RoomFilter? roomFilter, CancellationToken ct);
    Task<Guid> UpdateRoomAsync(Guid roomId, Room newRoom, CancellationToken ct);
    Task<Guid> DeleteRoomAsync(Guid roomId, CancellationToken ct);
}