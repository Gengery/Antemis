using System.ComponentModel.DataAnnotations;

namespace Antemis.Models
{
	public class ReservationModel
	{
		public int HotelID { get; set; }
		public int RoomNumber { get; set; }

		[Required]
		public DateOnly ArrivalDate { get; set; }

		[Required]
		public DateOnly LeavingDate { get; set; }

		[Required]
		public int Prepayment {  get; set; }

		[Required]
		public string INN {  get; set; }
	}
}
