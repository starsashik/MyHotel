using hotel_backend.Models;

namespace hotel_backend.Abstractions.Repositories;

public interface IRoomsRepository
{
    Task<Guid> CreateRoom(Room room, CancellationToken ct);
    Task<List<Room>> GetAllRooms(CancellationToken ct);
    Task<Guid> UpdateRoom(Guid roomId, Room newRoom, CancellationToken ct);
    Task<Guid> DeleteRoom(Guid roomId, CancellationToken ct);
}