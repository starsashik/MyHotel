using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.BookingsResponces;

public record GetFilteredBookingsResponse(
    List<BookingDto> Bookings
    );