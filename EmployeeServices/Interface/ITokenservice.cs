using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeServices.Interface
{
    public interface ITokenservice
    {
        string CreatejWTToken(IdentityUser user, List<string> roles);
    }
}
