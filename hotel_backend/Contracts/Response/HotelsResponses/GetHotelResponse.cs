using hotel_backend.Contracts.Other;

namespace hotel_backend.Contracts.Response.HotelsResponses;

public record GetHotelResponse(
    HotelDto Hotel
    );