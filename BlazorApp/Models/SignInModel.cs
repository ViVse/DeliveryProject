using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class SignInModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
