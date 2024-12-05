using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Worker
{
    public int Hotelid { get; set; }

    public string Inn { get; set; } = null!;

    public string? Imagename { get; set; }

    public int Workid { get; set; }

    public virtual Hotel Hotel { get; set; } = null!;

    public virtual Person InnNavigation { get; set; } = null!;

    public virtual Work Work { get; set; } = null!;
}
