using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommonModels.DbModels
{
    public class EmployeeDetail
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Designation ID is required.")]
        public int DesignationId { get; set; }

        public string ProfilePicture { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string Designation { get; set; }
    }
}
