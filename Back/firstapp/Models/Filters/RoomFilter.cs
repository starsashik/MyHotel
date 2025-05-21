namespace firstapp.Models.Filters;

public class RoomFilter
{
    private RoomFilter(Guid? hotelId, int? roomType)
    {
        HotelId = hotelId;
        RoomType = roomType;
    }

    public Guid? HotelId { get; }
    public int? RoomType { get; }

    private static string BasicChecks()
    {
        var error = string.Empty;

        return error;
    }

    public static (RoomFilter RoomFilter, string Error) Create(Guid? hotelId, int? roomType)
    {
        var error = BasicChecks();

        var roomFilter = new RoomFilter(hotelId, roomType);

        return (roomFilter, error);
    }
}