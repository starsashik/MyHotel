namespace firstapp.Contracts.Other;

public record HotelDto(
    Guid Id,
    string Name,
    string Location,
    string Description,
    string ImgUrl
);