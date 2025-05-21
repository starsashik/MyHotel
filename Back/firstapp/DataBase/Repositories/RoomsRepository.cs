using firstapp.Abstractions.Repositories;
using firstapp.DataBase.Contexts;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using Microsoft.EntityFrameworkCore;

namespace firstapp.DataBase.Repositories;

public class RoomsRepository : IRoomsRepository
{
    private readonly MyDbContext _context;

    public RoomsRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateRoom(Room room, CancellationToken cancellationToken)
    {
        var roomEntity = new DataBase.Entities.Room
        {
            Id = room.Id,
            HotelId = room.HotelId,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            PricePerNight = room.PricePerNight,
            ImgUrl = room.ImgUrl
        };

        await _context.Rooms.AddAsync(roomEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return roomEntity.Id;
    }

    public async Task<List<Room>> GetAllRooms(CancellationToken cancellationToken)
    {
        var roomEntities = await _context.Rooms
            .Include(u => u.Hotel)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var rooms = roomEntities
            .Select(roomEntity =>
            {
                var (room, roomError) = Room
                    .Create(roomEntity.Id,
                        roomEntity.HotelId,
                        roomEntity.RoomNumber,
                        roomEntity.RoomType,
                        roomEntity.PricePerNight,
                        roomEntity.ImgUrl);

                if (!string.IsNullOrEmpty(roomError))
                    throw new IntegrityException($"Incorrect data format in the database, unable to create a " +
                                                 $"room model: {roomError}");

                return room;
            })
            .ToList();

        return rooms;
    }

    public async Task<Guid> UpdateRoom(Guid roomId, Room newRoom, CancellationToken cancellationToken)
    {
        var oldRoomEntity = await _context.Rooms
            .Include(u => u.Hotel)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == roomId, cancellationToken);

        if (oldRoomEntity is null)
            throw new UnknownIdentifierException("Unknown room id");

        await _context.Rooms
            .Where(u => u.Id == roomId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.HotelId, u => oldRoomEntity.HotelId)
                    .SetProperty(u => u.RoomNumber, u => newRoom.RoomNumber)
                    .SetProperty(u => u.RoomType, u => newRoom.RoomType)
                    .SetProperty(u => u.PricePerNight, u => newRoom.PricePerNight)
                    .SetProperty(u => u.ImgUrl, u => newRoom.ImgUrl),
                cancellationToken);

        return oldRoomEntity.Id;
    }

    public async Task<Guid> DeleteRoom(Guid roomId, CancellationToken cancellationToken)
    {
        var numDeleted = await _context.Rooms
            .Where(u => u.Id == roomId)
            .ExecuteDeleteAsync(cancellationToken);

        if (numDeleted == 0)
            throw new UnknownIdentifierException("Unknown room id");

        return roomId;
    }
}