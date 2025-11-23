namespace hotel_backend.Contracts.Requests.BookingsRequests;

public record CreateBookingRequest(
    Guid UserId,
    Guid RoomId,
    string CheckIn,
    string CheckOut
    );