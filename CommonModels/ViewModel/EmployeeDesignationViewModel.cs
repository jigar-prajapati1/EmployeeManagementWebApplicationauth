using System.ComponentModel.DataAnnotations;

namespace CommonModels.ViewModel
{
    public class EmployeeDesignationViewModel
    {
        [Key]
        public int DesignationId { get; set; }
        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }
    }
}
