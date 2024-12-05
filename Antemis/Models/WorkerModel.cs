using System.ComponentModel.DataAnnotations;

namespace Antemis.Models
{
    public class WorkerModel
    {
        [Required(ErrorMessage = "*")]
        public int Id { get; set; }

		[Required(ErrorMessage = "*")]
		[Length(12, 12, ErrorMessage = "ИНН состоит из 12 символов")]
		[RegularExpression(@"\d*", ErrorMessage = "ИНН может состоять только из цифр")]
		public string INN { get; set; }


		[Required(ErrorMessage = "*")]
		public string Surname { get; set; }


		[Required(ErrorMessage = "*")]
		public string Name { get; set; }


		[Required(ErrorMessage = "*")]
		public string Patronimic { get; set; }


		[Required(ErrorMessage = "*")]
		public char Gender { get; set; }


		[Required(ErrorMessage = "*")]
		public DateOnly? Birth {  get; set; }

		[Required(ErrorMessage = "*")]
		public int HotelID { get; set; }

		[Required(ErrorMessage = "*")]
		public string Img { get; set; }
	}
}
