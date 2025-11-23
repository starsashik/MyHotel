using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.BookingsResponces;

public record GetBookingResponse(
    BookingDto Booking
    );