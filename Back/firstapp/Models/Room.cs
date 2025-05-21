namespace firstapp.Models;

public class Room
{
    public const int MaxNumber = 1000;
    public const int MinNumber = 1;
    public const int MaxType = 3;
    public const int MinType = 1;
    public const int MaxPrice = 10000;
    public const int MinPrice = 1;

    private Room(Guid id, Guid hotelId, int roomNumber, int roomType, int pricePerNight, string imgUrl = "ImgRoom/default.png")
    {
        Id = id;
        HotelId = hotelId;
        RoomNumber = roomNumber;
        RoomType = roomType;
        PricePerNight = pricePerNight;
        ImgUrl = imgUrl;
    }

    public Guid Id { get; }
    public Guid HotelId { get; }
    public int RoomNumber { get; }
    public int RoomType { get; }
    public int PricePerNight { get; }
    public string ImgUrl { get; }

    private string BasicChecks()
    {
        var error = string.Empty;

        if (RoomNumber > MaxNumber)
        {
            error = $"Room number can't be more than {MaxNumber}.";
        }
        else if (RoomNumber < MinNumber)
        {
            error = $"Room number can't be less than {MinNumber}.";
        }
        else  if (RoomType > MaxType)
        {
            error = $"Room type can't be more than {MaxType}.";
        }
        else if (RoomType < MinType)
        {
            error = $"Room type can't be less than {MinType}.";
        }
        else  if (PricePerNight > MaxPrice)
        {
            error = $"Price Per Night can't be more than {MaxPrice}.";
        }
        else if (PricePerNight < MinPrice)
        {
            error = $"Price Per Night can't be less than {MinPrice}.";
        }

        return error;
    }

    public static (Room Room, string Error) Create(Guid id, Guid hotelId, int roomNumber, int roomType,
        int pricePerNight, string imgUrl = "ImgRoom/default.png")
    {
        var room = new Room(id, hotelId, roomNumber, roomType, pricePerNight, imgUrl);

        var error = room.BasicChecks();

        return (room, error);
    }
}