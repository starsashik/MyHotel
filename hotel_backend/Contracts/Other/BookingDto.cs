namespace hotel_backend.Contracts.Other;

public record BookingDto(
    string BookingId,
    string UserId,
    string RoomId,
    DateOnly CheckIn,
    DateOnly CheckOut
    );