namespace hotel_backend.Contracts.Requests.BookingsRequests;

public record DeleteBookingRequest(
    Guid BookingId
    );