using Antemis.Database;
using Antemis.Models;

namespace Antemis.ComplexModels
{
	public class RoomsComplexModel
	{
		public List<Room> Rooms { get; set; }
		public RoomsSortComponents Sort { get; set; }
		public int Hid { get; set; }
	}
}
