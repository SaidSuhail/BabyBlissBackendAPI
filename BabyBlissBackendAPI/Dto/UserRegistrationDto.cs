using System.ComponentModel.DataAnnotations;

namespace BabyBlissBackendAPI.Dto
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(30, ErrorMessage = "Name should not exceed 30 characters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required!.")]
        [EmailAddress(ErrorMessage = "Invalid email address!.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required!.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long!.")]
        public string? Password { get; set; }

        // Ensure role is optional, as the default will be assigned if not provided
        //public string Role { get; set; }
    }
}
