namespace hotel_backend.Models.Filters;

public class HotelFilter
{
    private HotelFilter(string? partOfName, string? partOfLocation, string? partOfDescription)
    {
        PartOfName = partOfName;
        PartOfLocation = partOfLocation;
        PartOfDescription = partOfDescription;
    }

    public string? PartOfName { get; }
    public string? PartOfLocation { get; }
    public string? PartOfDescription { get; }

    private static string BasicChecks()
    {
        var error = string.Empty;

        return error;
    }

    public static (HotelFilter HotelFilter, string Error) Create(string? partOfName, string? partOfLocation, string? partOfDescription)
    {
        var error = BasicChecks();

        var hotelFilter = new HotelFilter(partOfName, partOfLocation, partOfDescription);

        return (hotelFilter, error);
    }
}