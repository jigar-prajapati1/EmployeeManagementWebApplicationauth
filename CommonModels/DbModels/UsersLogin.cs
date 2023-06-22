using System.ComponentModel.DataAnnotations;

namespace CommonModels.DbModels
{
    public class UsersLogin
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string password { get; set; }
        [Required(ErrorMessage = "Created Date is required.")]
        public DateTime CreatedDate { get; set; }
    }
}
