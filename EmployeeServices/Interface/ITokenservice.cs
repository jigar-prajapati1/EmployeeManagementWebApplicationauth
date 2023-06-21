using System.Security.Claims;

namespace EmployeeServices.Interface
{
    public interface ITokenservice
    {
        string CreatejWTToken(ClaimsIdentity claimsIdentity);
    }
}
