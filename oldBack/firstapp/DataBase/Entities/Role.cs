using System;
using System.Collections.Generic;

namespace firstapp.DataBase.Entities;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int LevelAccess { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
