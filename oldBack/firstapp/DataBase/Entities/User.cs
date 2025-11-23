using System;
using System.Collections.Generic;

namespace firstapp.DataBase.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public Guid RoleId { get; set; }

    public string ImgUrl { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Role Role { get; set; } = null!;
}
