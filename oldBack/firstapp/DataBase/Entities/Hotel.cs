using System;
using System.Collections.Generic;

namespace firstapp.DataBase.Entities;

public partial class Hotel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
