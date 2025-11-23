using firstapp.Models;

namespace firstapp.Abstractions.Repositories;

public interface IHotelsRepository
{
    Task<Guid> CreateHotel(Hotel hotel, CancellationToken ct);
    Task<List<Hotel>> GetAllHotels(CancellationToken ct);
    Task<Guid> UpdateHotel(Guid hotelId, Hotel newHotel, CancellationToken ct);
    Task<Guid> DeleteHotel(Guid hotelId, CancellationToken ct);
}