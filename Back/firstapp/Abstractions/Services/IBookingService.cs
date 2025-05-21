﻿using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Abstractions.Services;

public interface IBookingService
{
    Task<Guid> CreateBookingAsync(Booking booking, CancellationToken ct);
    Task<Booking> GetBookingAsync(Guid bookingId, CancellationToken ct);
    Task<List<Booking>> GetFilteredBookingsAsync(BookingFilter? bookingFilter, CancellationToken ct);
    Task<Guid> UpdateBookingAsync(Guid bookingId, Booking newBooking, CancellationToken ct);
    Task<Guid> DeleteBookingAsync(Guid bookingId, CancellationToken ct);
}