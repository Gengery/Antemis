using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class User
{
    public string Inn { get; set; } = null!;

    public string Userlogin { get; set; } = null!;

    public int Userid { get; set; }

    public string? Imagename { get; set; }

    public string Userpassword { get; set; } = null!;

    public virtual Person InnNavigation { get; set; } = null!;
}
