using System.ComponentModel.DataAnnotations;

namespace CommonModels.DbModels
{
    public class EmployeeDesignation
    {
        [Key]
        public int DesignationId { get; set; }
        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }
    }
}
