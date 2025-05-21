namespace firstapp.Models;

public class Hotel
{
    public const int MaxNameLength = 128;
    public const int MinNameLength = 5;
    public const int MaxLocationLength = 128;
    public const int MinLocationLength = 5;
    public const int MaxDescriptionLength = 128;
    public const int MinDescriptionLength = 5;

    private Hotel(Guid id, string name, string location, string description, string imgUrl = "ImgHotel/default.png")
    {
        Id = id;
        Name = name;
        Location = location;
        Description = description;
        ImgUrl = imgUrl;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Location { get; }
    public string Description { get; }
    public string ImgUrl { get; }

    private string BasicChecks()
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(Name) || Name.Length > MaxNameLength)
        {
            error = $"Name can't be longer than {MaxNameLength} characters or empty";
        }
        else if (Name.Length < MinNameLength)
        {
            error = $"Name can't be shorter than {MinNameLength} characters";
        }
        else if (string.IsNullOrWhiteSpace(Name))
        {
            error = "Name can't be line only with whitespaces";
        }
        else if (string.IsNullOrEmpty(Location) || Location.Length > MaxLocationLength)
        {
            error = $"Location can't be longer than {MaxLocationLength} characters or empty";
        }
        else if (Location.Length < MinLocationLength)
        {
            error = $"Location can't be shorter than {MinLocationLength} characters";
        }
        else if (string.IsNullOrWhiteSpace(Location))
        {
            error = "Location can't be line only with whitespaces";
        }
        else if (string.IsNullOrEmpty(Description) || Description.Length > MaxDescriptionLength)
        {
            error = $"Description can't be longer than {MaxDescriptionLength} characters or empty";
        }
        else if (Description.Length < MinDescriptionLength)
        {
            error = $"Description can't be shorter than {MinDescriptionLength} characters";
        }
        else if (string.IsNullOrWhiteSpace(Description))
        {
            error = "Description can't be line only with whitespaces";
        }

        return error;
    }

    public static (Hotel Hotel, string Error) Create(Guid id, string name, string location,
        string description, string imgUrl = "ImgHotel/default.png")
    {
        var hotel = new Hotel(id, name, location, description, imgUrl);

        var error = hotel.BasicChecks();

        return (hotel, error);
    }
}