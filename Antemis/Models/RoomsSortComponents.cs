using Antemis.Database;

namespace Antemis.Models
{
	public class RoomsSortComponents
	{
		public int RoomsAmountFilter { get; set; }
		public int? LowerPriceFilter { get; set; } = null;
		public int? UpperPriceFilter { get; set; } = null;
		public string Case {  get; set; }
		public bool IsDescending { get; set; }
		
		public DateOnly? ADate { get; set; }
		public DateOnly? LDate { get; set; }
	}
}
