using System.ComponentModel.DataAnnotations;

namespace Antemis.Models
{
	public class RoomModel
	{
		[Required(ErrorMessage = "*")]
		public int HotelId { get; set; }

		[Required(ErrorMessage = "*")]
		public int Number { get; set; }

		[Required(ErrorMessage = "*")]
		public int PlAmount { get; set; }

		[Required(ErrorMessage = "*")]
		public int DaylyPrice { get; set; }

		[Required(ErrorMessage = "*")]
		public string Img { get; set; }

		public string? Descryption { get; set; } = "";
	}
}
