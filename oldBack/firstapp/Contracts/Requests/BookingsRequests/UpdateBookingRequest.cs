namespace firstapp.Contracts.Requests.BookingsRequests;

public record UpdateBookingRequest(
    Guid Id,
    Guid? UserId,
    Guid? RoomId,
    string? CheckInDate,
    string? CheckOutDate
    );