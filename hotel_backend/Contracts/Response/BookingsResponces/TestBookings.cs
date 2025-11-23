namespace hotel_backend.Contracts.Response.BookingsResponces;

public record TestBookings(
    string BookingId,
    string UserId,
    string RoomId,
    DateOnly CheckIn,
    DateOnly CheckOut
    );