using hotel_backend.Models;
using hotel_backend.Models.Filters;

namespace hotel_backend.Abstractions.Services;

public interface IHotelsService
{
    Task<Guid> CreateHotelAsync(Hotel hotel, CancellationToken ct);
    Task<Hotel> GetHotelAsync(Guid hotelId, CancellationToken ct);
    Task<List<Hotel>> GetFilteredHotelsAsync(HotelFilter? hotelFilter, CancellationToken ct);
    Task<Guid> UpdateHotelAsync(Guid hotelId, Hotel newHotel, CancellationToken ct);
    Task<Guid> DeleteHotelAsync(Guid hotelId, CancellationToken ct);
}