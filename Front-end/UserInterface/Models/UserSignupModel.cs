using System.ComponentModel.DataAnnotations;

namespace UserInterface.Models
{
    public class UserSignupModel
    {
        [Required]
        [Display(Name = "İsim ve Soyisim giriniz")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email giriniz")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Şifre giriniz")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Şehir giriniz")]
        public string City { get; set; }

    }
}
