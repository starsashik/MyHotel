using firstapp.Abstractions.Repositories;
using firstapp.DataBase.Contexts;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using Microsoft.EntityFrameworkCore;

namespace firstapp.DataBase.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly MyDbContext _context;

    public BookingRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateBooking(Booking booking, CancellationToken cancellationToken)
    {
        var bookingEntity = new DataBase.Entities.Booking
        {
            Id = booking.Id,
            UserId = booking.UserId,
            RoomId = booking.RoomId,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
        };

        await _context.Bookings.AddAsync(bookingEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return bookingEntity.Id;
    }

    public async Task<List<Booking>> GetAllBookings(CancellationToken cancellationToken)
    {
        var bookingEntities = await _context.Bookings
            .Include(u => u.User)
            .Include(u => u.Room)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var bookings = bookingEntities
            .Select(bookingEntity =>
            {
                var (booking, bookingError) = Booking
                    .Create(bookingEntity.Id,
                        bookingEntity.UserId,
                        bookingEntity.RoomId,
                        bookingEntity.CheckInDate,
                        bookingEntity.CheckOutDate,
                        false
                    );

                if (!string.IsNullOrEmpty(bookingError))
                    throw new IntegrityException($"Incorrect data format in the database, unable to create a " +
                                                 $"room model: {bookingError}");

                return booking;
            })
            .ToList();

        return bookings;
    }

    public async Task<Guid> UpdateBooking(Guid bookingId, Booking newBooking, CancellationToken cancellationToken)
    {
        var oldBookingEntity = await _context.Bookings
            .Include(u => u.User)
            .Include(u => u.Room)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == bookingId, cancellationToken);

        if (oldBookingEntity is null)
            throw new UnknownIdentifierException("Unknown booking id");

        await _context.Bookings
            .Where(u => u.Id == bookingId)
            .ExecuteUpdateAsync(x => x
                    .SetProperty(u => u.UserId, newBooking.UserId)
                    .SetProperty(u => u.RoomId, newBooking.RoomId)
                    .SetProperty(u => u.CheckInDate, newBooking.CheckInDate)
                    .SetProperty(u => u.CheckOutDate, newBooking.CheckOutDate),
                cancellationToken);

        return oldBookingEntity.Id;
    }

    public async Task<Guid> DeleteBooking(Guid bookingId, CancellationToken cancellationToken)
    {
        var numDeleted = await _context.Bookings
            .Where(u => u.Id == bookingId)
            .ExecuteDeleteAsync(cancellationToken);

        if (numDeleted == 0)
            throw new UnknownIdentifierException("Unknown booking id");

        return bookingId;
    }
}