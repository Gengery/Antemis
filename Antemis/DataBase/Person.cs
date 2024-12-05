using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Person
{
    public string Inn { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public string? Patronimic { get; set; }

    public char Gender { get; set; }

    public DateOnly? Birthdate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Hotel> HotelHoteldirectorinnNavigations { get; set; } = new List<Hotel>();

    public virtual ICollection<Hotel> HotelHotelownerinnNavigations { get; set; } = new List<Hotel>();

    public virtual User? User { get; set; }

    public virtual Worker? Worker { get; set; }
}
