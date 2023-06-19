using CommonModels.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorys.Interfaces
{
   public interface IEmployeeDetailRepository
    {
        List<EmployeeDesignation> GetDesignations();
        List<EmployeeDetail> GetAllEmployee();
        EmployeeDetail GetEmployeeById(int id);
        List<EmployeeDetail> AddEmployee(EmployeeDetail employee);
        List<EmployeeDetail> UpdateEmployee(EmployeeDetail employee,int id);
        EmployeeDetail DeleteEmployee(int id);
        //List<UsersLogin> EmployeeLogin(UsersLogin _login);
        //List<UserRegistration> EmployeeRegistration(UserRegistration _registration);
    }
}
