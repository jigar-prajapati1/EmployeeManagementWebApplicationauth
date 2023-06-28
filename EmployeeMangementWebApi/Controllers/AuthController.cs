using System.Security.Claims;
using CommonModels.ViewModel;
using EmployeeServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace EmployeeMangementWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : Controller
    {
        private readonly IEmployeeDetailService employeeDetailService;
        private readonly ITokenservice tokenservice;
        public AuthController(IEmployeeDetailService employeeDetailService, ITokenservice tokenservice)
        {
            this.employeeDetailService = employeeDetailService;
            this.tokenservice = tokenservice;
        }
        /// <summary>Registers the specified registration.</summary>
        /// <param name="_registration">The registration.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        [Route("EmployeeRegistration")]
        public async Task<IActionResult> Register(UserRegistrationViewModel _registration)
        {
            try
            {
                employeeDetailService.NewEmployeeRegistration(_registration);
                return Ok("New Registration successfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>Logins the specified users login.</summary>
        /// <param name="usersLogin">The users login.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UsersLoginViewModel usersLogin)
        {
            try
            {
                employeeDetailService.NewEmployeeLogin(usersLogin);
                // Create ClaimsIdentity
                var claims = new List<Claim>
            {
                 new Claim(Microsoft.IdentityModel.Claims.ClaimTypes.Email, usersLogin.Email)
            };
                var claimsIdentity = new ClaimsIdentity(claims, "EmployeeAuthentication");

                // Create Token
                var jwtToken = tokenservice.CreatejWTToken(claimsIdentity);

                var response = new LoginResponseDto
                {
                    JwtToken = jwtToken
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
