namespace firstapp.Contracts.Requests.BookingsRequests;

public record GetFilteredBookingsRequest(
    Guid? UserId,
    string? DateInRange
    );