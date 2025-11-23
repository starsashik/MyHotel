namespace hotel_backend.Contracts.Other;

public record RoomDto(
    Guid Id,
    int RoomNumber,
    int RoomType,
    int PricePerNight,
    string ImgUrl
);