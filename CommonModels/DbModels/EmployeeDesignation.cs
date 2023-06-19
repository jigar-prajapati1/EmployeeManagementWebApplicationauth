using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
