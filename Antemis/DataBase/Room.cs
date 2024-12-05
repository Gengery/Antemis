using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Room
{
    public int Roomnumber { get; set; }

    public int Hotelid { get; set; }

    public string? Roomdescryprion { get; set; }

    public int? Placesamount { get; set; }

    public int Priceforday { get; set; }

    public bool? Isavaible { get; set; }

    public string? Imagename { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Hotel Hotel { get; set; } = null!;
}
