namespace hotel_backend.Models;

public class Booking
{
    public static DateOnly NowTime = DateOnly.FromDateTime(DateTime.Now);
    public static DateOnly MaxTime = NowTime.AddYears(1);

    private Booking(Guid id, Guid userId, Guid roomId, DateOnly checkIn, DateOnly checkOut, bool bIsCreat = true)
    {
        Id = id;
        UserId = userId;
        RoomId = roomId;
        CheckInDate = checkIn;
        CheckOutDate = checkOut;
        BIsCreat = bIsCreat;
    }

    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid RoomId { get; }
    public DateOnly CheckInDate { get; }
    public DateOnly CheckOutDate { get; }
    private bool BIsCreat {get;}

    private string BasicChecks()
    {
        var error = string.Empty;

        // if (BIsCreat && (CheckInDate < NowTime || CheckOutDate < NowTime))
        // {
        //     error = $" Dates can't be less than now {NowTime}.";
        // }
        // else if (BIsCreat && (CheckInDate > MaxTime || CheckOutDate > MaxTime))
        // {
        //     error = $" Dates can't be greater than {MaxTime}.";
        // }
        // else
        if (CheckOutDate < CheckInDate)
        {
            error = $" Date OUT can't be lese then date IN.";
        }

        return error;
    }

    public static (Booking Booking, string Error) Create(Guid id, Guid userId, Guid roomId, DateOnly checkIn, DateOnly checkOut, bool bIsCreat = true)
    {
        var booking = new Booking(id, userId, roomId, checkIn, checkOut,  bIsCreat);

        var error = booking.BasicChecks();

        return (booking, error);
    }
}