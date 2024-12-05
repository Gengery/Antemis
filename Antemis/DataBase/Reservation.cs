using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Reservation
{
    public int Roomnumber { get; set; }

    public int Hotelid { get; set; }

    public string Customerinn { get; set; } = null!;

    public virtual Customer CustomerinnNavigation { get; set; } = null!;

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
