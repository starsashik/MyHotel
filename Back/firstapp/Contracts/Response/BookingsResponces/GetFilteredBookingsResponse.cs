using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.BookingsResponces;

public record GetFilteredBookingsResponse(
    List<BookingDto> Bookings
    );