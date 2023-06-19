using CommonModels.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;

namespace EmployeeMangementWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
      
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> useManager)
        {
            
            this.userManager = useManager;
        }
        //POST:api/Auth/UserRegister
        [HttpPost]
        [Route("Register")]
         public async Task<IActionResult> Register([FromBody]RegisterRequestDto userRegistration)
        {
            var identityUser = new IdentityUser
            {
                UserName = userRegistration.UserName,
                Email = userRegistration.UserName
              
            };
            var identityResult=await userManager.CreateAsync(identityUser,userRegistration.Password);
            if (identityResult.Succeeded)
            {
                //Add roles to this User
                if(userRegistration.Roles != null && userRegistration.Roles.Any()) 
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, userRegistration.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User Was Registered! Please login");
                    }
                }
            }
            return BadRequest("Something Went Wrong");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDto userLogin)
        {
            var user = await userManager.FindByEmailAsync(userLogin.UserName);
            if (user != null)
            {
              var checkPasswordResult= await userManager.CheckPasswordAsync(user, userLogin.Password);
                if(checkPasswordResult)
                {
                    //Create Token
                    return Ok();
                }
            }
            return BadRequest("UserName and Passworg is incorrect");
        }

    }
}
