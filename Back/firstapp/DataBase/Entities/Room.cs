using System;
using System.Collections.Generic;

namespace firstapp.DataBase.Entities;

public partial class Room
{
    public Guid Id { get; set; }

    public Guid HotelId { get; set; }

    public int RoomNumber { get; set; }

    public int RoomType { get; set; }

    public int PricePerNight { get; set; }

    public string ImgUrl { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Hotel Hotel { get; set; } = null!;
}
