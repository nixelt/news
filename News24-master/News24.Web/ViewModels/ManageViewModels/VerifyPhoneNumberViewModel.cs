
using System.ComponentModel.DataAnnotations;


namespace News24.Web.ViewModels.ManageViewModels
{
    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}