using System.ComponentModel.DataAnnotations;

namespace Antemis.Models
{
    public class HotelModel
    {
        // Отель
        [Required(ErrorMessage ="* Обязательное поле")]
        [Length(12, 12, ErrorMessage ="ИНН состоит из 12 символов")]
        [RegularExpression(@"\d*", ErrorMessage = "ИНН может состоять только из цифр")]
        public string HotelINN { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string HotelNam { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string Password { get; set; }

        // Директор отеля
        [Required(ErrorMessage = "* Обязательное поле")]
        [Length(12, 12, ErrorMessage = "ИНН состоит из 12 символов")]
        [RegularExpression(@"\d*", ErrorMessage = "ИНН может состоять только из цифр")]
        public string DirectorINN { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string DSurname { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string DName { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string DPatronimic { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public DateOnly DBirthDate { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public char DGender { get; set; }

        // Владелец
        [Required(ErrorMessage = "* Обязательное поле")]
        [Length(12, 12, ErrorMessage = "ИНН состоит из 12 символов")]
        [RegularExpression(@"\d*", ErrorMessage = "ИНН может состоять только из цифр")]
        public string OwnerINN { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string OSurname { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string OName { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public string OPatronimic { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public DateOnly OBirthDate { get; set; }

        [Required(ErrorMessage = "* Обязательное поле")]
        public char OGender { get; set; }

        public string ID { get; set; } = "0";
    }
}
