using firstapp.Abstractions.Repositories;
using firstapp.Abstractions.Services;
using firstapp.Exceptions.SpecificExceptions;
using firstapp.Models;
using firstapp.Models.Filters;

namespace firstapp.Services;

public class HotelsService : IHotelsService
{
    private readonly IHotelsRepository _hotelsRepository;

    public HotelsService(IHotelsRepository repository)
    {
        _hotelsRepository = repository;
    }

     public async Task<Guid> CreateHotelAsync(Hotel hotel, CancellationToken cancellationToken)
    {
        var createdHotelId = await _hotelsRepository.CreateHotel(hotel, cancellationToken);

        return createdHotelId;
    }

    public async Task<Hotel> GetHotelAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        var allHotels = await _hotelsRepository.GetAllHotels(cancellationToken);

        var hotel = allHotels.FirstOrDefault(u => u.Id == hotelId);

        if (hotel == null)
            throw new UnknownIdentifierException("Unknown hotel id");

        return hotel;
    }

    public async Task<List<Hotel>> GetFilteredHotelsAsync(HotelFilter? hotelFilter, CancellationToken cancellationToken)
    {
        var hotels = await _hotelsRepository.GetAllHotels(cancellationToken);

        if (hotelFilter is null)
            return hotels;

        if (hotelFilter.PartOfName is not null)
        {
            hotels = hotels
                .Where(u => u.Name.ToLower().Contains(hotelFilter.PartOfName.ToLower()))
                .ToList();
        }

        if (hotelFilter.PartOfLocation is not null)
        {
            hotels = hotels
                .Where(u => u.Location.ToLower().Contains(hotelFilter.PartOfLocation.ToLower()))
                .ToList();
        }

        if (hotelFilter.PartOfDescription is not null)
        {
            hotels = hotels
                .Where(u => u.Description.ToLower().Contains(hotelFilter.PartOfDescription.ToLower()))
                .ToList();
        }

        return hotels.OrderBy(u => u.Name).ToList();
    }

    public async Task<Guid> UpdateHotelAsync(Guid hotelId, Hotel newHotel, CancellationToken cancellationToken)
    {
        var updatedHotelId = await _hotelsRepository.UpdateHotel(hotelId, newHotel, cancellationToken);

        return updatedHotelId;
    }

    public async Task<Guid> DeleteHotelAsync(Guid hotelId, CancellationToken cancellationToken)
    {
        var deletedHotelId = await _hotelsRepository.DeleteHotel(hotelId, cancellationToken);

        return deletedHotelId;
    }
}