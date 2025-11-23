using hotel_backend.Abstractions.Repositories;
using hotel_backend.DataBase.Contexts;
using hotel_backend.Models;
using Microsoft.EntityFrameworkCore;
using hotel_backend.Exceptions.SpecificExceptions;

namespace hotel_backend.DataBase.Repositories;

public class HotelsRepository : IHotelsRepository
{
    private readonly MyDbContext _context;

    public HotelsRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateHotel(Hotel hotel, CancellationToken cancellationToken)
    {
        var hotelEntity = new DataBase.Entities.Hotel
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Location = hotel.Location,
            Description = hotel.Description,
            ImgUrl = hotel.ImgUrl
        };

        await _context.Hotels.AddAsync(hotelEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return hotelEntity.Id;
    }

    public async Task<List<Hotel>> GetAllHotels(CancellationToken cancellationToken)
    {
        var hotelEntities = await _context.Hotels
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var hotels = hotelEntities
            .Select(hotelEntity =>
            {
                var (hotel, hotelError) = Hotel
                    .Create(hotelEntity.Id,
                        hotelEntity.Name,
                        hotelEntity.Location,
                        hotelEntity.Description,
                        hotelEntity.ImgUrl);

                if (!string.IsNullOrEmpty(hotelError))
                    throw new IntegrityException($"Incorrect data format in the database, unable to create a " +
                                                 $"hotel model: {hotelError}");

                return hotel;
            })
            .ToList();

        return hotels;
    }

    public async Task<Guid> UpdateHotel(Guid hotelId, Hotel newHotel, CancellationToken cancellationToken)
    {
        var oldHotelEntity = await _context.Hotels
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == hotelId, cancellationToken);

        if (oldHotelEntity is null)
            throw new UnknownIdentifierException("Unknown hotel id");

        await _context.Hotels
            .Where(u => u.Id == hotelId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.Name, u => newHotel.Name)
                    .SetProperty(u => u.Location, u => newHotel.Location)
                    .SetProperty(u => u.Description, u => newHotel.Description)
                    .SetProperty(u => u.ImgUrl, u => newHotel.ImgUrl),
                cancellationToken);

        return oldHotelEntity.Id;
    }

    public async Task<Guid> DeleteHotel(Guid hotelId, CancellationToken cancellationToken)
    {
        var numDeleted = await _context.Hotels
            .Where(u => u.Id == hotelId)
            .ExecuteDeleteAsync(cancellationToken);

        if (numDeleted == 0)
            throw new UnknownIdentifierException("Unknown hotel id");

        return hotelId;
    }
}