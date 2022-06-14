using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Phone number length should be 10 digits")]
        public string PhoneNumber { get; set; }
    }
}
