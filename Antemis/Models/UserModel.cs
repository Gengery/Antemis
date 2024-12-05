using System.ComponentModel.DataAnnotations;

namespace Antemis.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Введите фамилию")]
        public string? Surname { get; set; } = null;

        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        public string? Patronimic { get; set; } = null;

		[Length(12, 12, ErrorMessage = "ИНН содержит 12 символов")]
		[RegularExpression(@"\d*", ErrorMessage = "ИНН может состоять только из цифр")]
		[Required(ErrorMessage = "Введите ИНН")]
        public string INN { get; set; }

        [Required(ErrorMessage = "Выберите пол")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Выберите дату рождения")]
        public DateOnly? BirthDate { get; set; } = null;

        [Required(ErrorMessage = "Выберите картинку")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public string ID { get; set; } = "000000000000";
		public bool HasPerson = false;

    }
}
