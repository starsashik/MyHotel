namespace firstapp.Contracts.Response.HotelsResponses;

public record TestHotels(
    string HotelId,
    string Name,
    string Location,
    string Description,
    string ImgUrl
);