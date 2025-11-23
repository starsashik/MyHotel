namespace firstapp.Contracts.Other;

public record BookingDto(
    string BookingId,
    string UserId,
    string RoomId,
    DateOnly CheckIn,
    DateOnly CheckOut
    );