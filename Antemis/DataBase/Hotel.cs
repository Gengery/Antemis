using System;
using System.Collections.Generic;

namespace Antemis.Database;

public partial class Hotel
{
    public int Hotelid { get; set; }

    public string Hotelname { get; set; } = null!;

    public string Hotelinn { get; set; } = null!;

    public string Hoteldirectorinn { get; set; } = null!;

    public string Hotelownerinn { get; set; } = null!;

    public string? Hotelimage { get; set; }

    public string Hotelpassword { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual Person HoteldirectorinnNavigation { get; set; } = null!;

    public virtual Person HotelownerinnNavigation { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
