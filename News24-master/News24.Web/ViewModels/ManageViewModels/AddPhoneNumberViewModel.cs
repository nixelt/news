
using System.ComponentModel.DataAnnotations;


namespace News24.Web.ViewModels.ManageViewModels
{
    public class AddPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}