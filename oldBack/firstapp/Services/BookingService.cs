using firstapp.Abstractions.Repositories;
using firstapp.Abstractions.Services;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingsRepository;

    public BookingService(IBookingRepository bookingsRepository)
    {
        _bookingsRepository = bookingsRepository;
    }
    
    public async Task<Guid> CreateBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var createdBookingId = await _bookingsRepository.CreateBooking(booking, cancellationToken);

        return createdBookingId;
    }

    public async Task<Booking> GetBookingAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var allBookings = await _bookingsRepository.GetAllBookings(cancellationToken);

        var room = allBookings.FirstOrDefault(u => u.Id == bookingId);

        if (room == null)
            throw new UnknownIdentifierException("Unknown booking id");

        return room;
    }

    public async Task<List<Booking>> GetFilteredBookingsAsync(BookingFilter? bookingFilter, CancellationToken cancellationToken)
    {
        var bookings = await _bookingsRepository.GetAllBookings(cancellationToken);

        if (bookingFilter is null)
            return bookings;

        if (bookingFilter.UserId is not null)
        {
            bookings = bookings
                .Where(u => u.UserId == bookingFilter.UserId)
                .ToList();
        }

        if (bookingFilter.DateInRange is not null)
        {
            bookings = bookings
                .Where(u => u.CheckInDate <= bookingFilter.DateInRange && u.CheckOutDate >= bookingFilter.DateInRange)
                .ToList();
        }

        return bookings.OrderBy(u => u.UserId).ToList();
    }

    public async Task<Guid> UpdateBookingAsync(Guid bookingId, Booking newBooking, CancellationToken cancellationToken)
    {
        var updatedBookingId = await _bookingsRepository.UpdateBooking(bookingId, newBooking, cancellationToken);

        return updatedBookingId;
    }

    public async Task<Guid> DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var deletedBookingId = await _bookingsRepository.DeleteBooking(bookingId, cancellationToken);

        return deletedBookingId;
    }
}