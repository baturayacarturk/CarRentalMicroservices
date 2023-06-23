using System.ComponentModel.DataAnnotations;

namespace UserInterface.Models
{
    public class SignInCredentials
    {
        [Required]
        [Display(Name ="Email address")]
        public string Email { get; set; }
        [Display(Name = "Your password")]
        [Required]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
