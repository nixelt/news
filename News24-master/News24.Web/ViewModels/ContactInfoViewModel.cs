using System.ComponentModel.DataAnnotations;

namespace News24.Web.ViewModels
{
    public class ContactInfoViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Электронный адрес")]
        [EmailAddress(ErrorMessage = "Неправильный формат поля")]
        public string Email { get; set; }

        public byte[] Image { get; set; }
    }
}