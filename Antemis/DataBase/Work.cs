using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Work
{
    public int Workid { get; set; }

    public string Workname { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
