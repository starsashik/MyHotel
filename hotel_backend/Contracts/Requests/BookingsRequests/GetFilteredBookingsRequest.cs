namespace hotel_backend.Contracts.Requests.BookingsRequests;

public record GetFilteredBookingsRequest(
    Guid? UserId,
    string? DateInRange
    );