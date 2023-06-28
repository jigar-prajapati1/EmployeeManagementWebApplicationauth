
using CommonModels.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfaces;
using static EmployeeManagement.Controllers.EmployeeDetailController;

namespace EmployeeMangementWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeDetailController : Controller
    {
        private readonly IEmployeeDetailService employeeDetailService;

        public EmployeeDetailController(IEmployeeDetailService employeeDetailService)
        {
            this.employeeDetailService = employeeDetailService;
        }
        /// <summary>Gets all.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<EmployeeDetailViewModel> employees = employeeDetailService.GetAllEmployee();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        /// <summary>Gets the employee by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                var employee = employeeDetailService.GetEmployeeById(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>Adds the employee.</summary>
        /// <param name="empDetail">The emp detail.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        [Route("AddEmployee")]
        public IActionResult AddEmployee(EmployeeDetailViewModel empDetail)
        {
            try
            {
                //if (imageFile != null && imageFile.Length > 0)
                //{
                //    // Save the image file to the desired location
                //    string fileName = Path.GetFileName(imageFile.FileName);
                //    // ...
                //}
                employeeDetailService.AddEmployeeDetail(empDetail);
                return Ok(empDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>Updates the employee.</summary>
        /// <param name="empDetail">The emp detail.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateEmployee(EmployeeDetailViewModel empDetail, int id)
        {
            try
            {
                employeeDetailService.UpdateEmployeeDetail(empDetail, id);
                return Ok(empDetail);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                employeeDetailService.DeleteEmployee(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// </returns>
        [HttpGet]
        [Route("GetDesignation")]
        public IActionResult GetDesignation()
        {
            try
            {
                List<EmployeeDesignationViewModel> employees = employeeDetailService.GetDesignations();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
