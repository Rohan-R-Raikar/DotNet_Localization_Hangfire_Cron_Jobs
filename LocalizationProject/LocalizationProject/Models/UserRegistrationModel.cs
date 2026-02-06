using System.ComponentModel.DataAnnotations;

namespace LocalizationProject.Models
{
    public class UserRegistrationModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "FirstNameLabel")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "LastNameLabel")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "EmailLabel")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordLabel")]
        public string Password { get; set; }
    }
}
