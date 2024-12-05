using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Customer
{
    public int Roomnumber { get; set; }

    public int Hotelid { get; set; }

    public DateOnly Arrivaldate { get; set; }

    public DateOnly Leavingdate { get; set; }

    public int? Prepayment { get; set; }

    public string Customerinn { get; set; } = null!;

    public virtual Person CustomerinnNavigation { get; set; } = null!;

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
