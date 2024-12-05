using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Antemis.Models
{
	public class WorkTypeModel
	{
		[Required(ErrorMessage = "* обязательное поле")]
		public int Id { get; set; }

		[Required(ErrorMessage = "* обязательное поле")]
		public string Name { get; set; }
	}
}
