using System.ComponentModel.DataAnnotations;

namespace CommonModels.DbModels
{
    public class UserRegistration
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = " Date is required.")]
        public DateTime Date { get; set; }
    }
}
