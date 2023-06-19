using CommonModels.ViewModel;

namespace Services.Interfaces
{
    public interface IEmployeeDetailService
    {
        List<EmployeeDetailViewModel> GetAllEmployee();
        List<EmployeeDesignationViewModel> GetDesignations();
        EmployeeDetailViewModel GetEmployeeById(int id);
        void AddEmployeeDetail(EmployeeDetailViewModel employeeDetail);
        void UpdateEmployeeDetail(EmployeeDetailViewModel employeeDetail,int id);
        void DeleteEmployee(int id);
        //void NewEmployeeLogin(UsersLoginViewModel _usersLogin);
        //void NewEmployeeRegistration(UserRegistrationViewModel _registration);

    }
}