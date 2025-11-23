using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.BookingsResponces;

public record GetBookingResponse(
    BookingDto Booking
    );