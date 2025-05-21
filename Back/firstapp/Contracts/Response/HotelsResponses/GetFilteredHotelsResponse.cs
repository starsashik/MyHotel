using firstapp.Contracts.Other;

namespace firstapp.Contracts.Response.HotelsResponses;

public record GetFilteredHotelsResponse(
    List<HotelDto> Hotels
);