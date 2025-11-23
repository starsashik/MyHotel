using firstapp.Models;

namespace firstapp.Abstractions.Repositories;

public interface IBookingRepository
{
    Task<Guid> CreateBooking(Booking booking, CancellationToken ct);
    Task<List<Booking>> GetAllBookings(CancellationToken ct);
    Task<Guid> UpdateBooking(Guid bookingId, Booking newBooking, CancellationToken ct);
    Task<Guid> DeleteBooking(Guid bookingId, CancellationToken ct);
}