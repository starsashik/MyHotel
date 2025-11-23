namespace hotel_backend.Models.Filters;

public class BookingFilter
{
    private BookingFilter(Guid? userId, DateOnly? dateInRange)
    {
        UserId = userId;
        DateInRange = dateInRange;
    }

    public Guid? UserId { get; }
    public DateOnly? DateInRange { get; }

    private static string BasicChecks()
    {
        var error = string.Empty;

        return error;
    }

    public static (BookingFilter BookingFilter, string Error) Create(Guid? userId, DateOnly? dateInRange)
    {
        var error = BasicChecks();

        var bookingFilter = new BookingFilter(userId, dateInRange);

        return (bookingFilter, error);
    }
}