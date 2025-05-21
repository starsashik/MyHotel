using firstapp.Abstractions.Repositories;
using firstapp.Abstractions.Services;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Services;

public class RoomsService : IRoomsService
{
    private readonly IRoomsRepository _roomsRepository;

    public RoomsService(IRoomsRepository roomsRepository)
    {
        _roomsRepository = roomsRepository;
    }

    public async Task<Guid> CreateRoomAsync(Room room, CancellationToken cancellationToken)
    {
        var createdRoomId = await _roomsRepository.CreateRoom(room, cancellationToken);

        return createdRoomId;
    }

    public async Task<Room> GetRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var allRooms = await _roomsRepository.GetAllRooms(cancellationToken);

        var room = allRooms.FirstOrDefault(u => u.Id == roomId);

        if (room == null)
            throw new UnknownIdentifierException("Unknown room id");

        return room;
    }

    public async Task<List<Room>> GetFilteredRoomsAsync(RoomFilter? roomFilter, CancellationToken cancellationToken)
    {
        var rooms = await _roomsRepository.GetAllRooms(cancellationToken);

        if (roomFilter is null)
            return rooms;

        if (roomFilter.HotelId is not null)
        {
            rooms = rooms
                .Where(u => u.HotelId == roomFilter.HotelId)
                .ToList();
        }

        if (roomFilter.RoomType is not null)
        {
            rooms = rooms
                .Where(u => u.RoomType == roomFilter.RoomType)
                .ToList();
        }

        return rooms.OrderBy(u => u.HotelId).ToList();
    }

    public async Task<Guid> UpdateRoomAsync(Guid roomId, Room newRoom, CancellationToken cancellationToken)
    {
        var updatedRoomId = await _roomsRepository.UpdateRoom(roomId, newRoom, cancellationToken);

        return updatedRoomId;
    }

    public async Task<Guid> DeleteRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        var deletedRoomId = await _roomsRepository.DeleteRoom(roomId, cancellationToken);

        return deletedRoomId;
    }
}