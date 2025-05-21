using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Abstractions.Services;

public interface IHotelsService
{
    Task<Guid> CreateHotelAsync(Hotel hotel, CancellationToken ct);
    Task<Hotel> GetHotelAsync(Guid hotelId, CancellationToken ct);
    Task<List<Hotel>> GetFilteredHotelsAsync(HotelFilter? hotelFilter, CancellationToken ct);
    Task<Guid> UpdateHotelAsync(Guid hotelId, Hotel newHotel, CancellationToken ct);
    Task<Guid> DeleteHotelAsync(Guid hotelId, CancellationToken ct);
}