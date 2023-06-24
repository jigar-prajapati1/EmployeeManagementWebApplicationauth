using CommonModels.DbModels;
using CommonModels.ViewModel;
using Repositorys.Interfaces;
using Services.Interfaces;

namespace Services.Implements
{
    public class EmployeeDetailService : IEmployeeDetailService
    {
        private readonly IEmployeeDetailRepository _empDetailRepo;

        public EmployeeDetailService(IEmployeeDetailRepository empDetailRepo)
        {
            _empDetailRepo = empDetailRepo;
        }

        public void NewEmployeeRegistration(UserRegistrationViewModel _registration)
        {
            try
            {
                var registration = new UserRegistration
                {
                    Email = _registration.Email,
                    Password = _registration.Password,
                    ConfirmPassword = _registration.ConfirmPassword
                };
                _empDetailRepo.EmployeeRegistration(registration);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
        public void NewEmployeeLogin(UsersLoginViewModel _usersLogin)
        {
            try
            {
                var usersLogin = new UsersLogin
                {
                    Email = _usersLogin.Email,
                    password = _usersLogin.password
                };
                _empDetailRepo.EmployeeLogin(usersLogin);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
        public List<EmployeeDetailViewModel> GetAllEmployee()
        {
            try
            {
                List<EmployeeDetailViewModel> Employeelist = new List<EmployeeDetailViewModel>();
                var Dbmodellist = _empDetailRepo.GetAllEmployee();
                if (Dbmodellist != null && Dbmodellist.Count > 0)
                {
                    foreach (var items in Dbmodellist)
                    {
                        Employeelist.Add(new EmployeeDetailViewModel
                        {
                            Id = items.Id,
                            Name = items.Name,
                            DesignationId = items.DesignationId,
                            ProfilePicture = items.ProfilePicture,
                            Salary = items.Salary,
                            DataOfBirth = items.DataOfBirth,
                            Email = items.Email,
                            Address = items.Address,
                            Designation = items.Designation,
                        });
                    }
                }
                return Employeelist;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
        public List<EmployeeDesignationViewModel> GetDesignations()
        {
            try
            {
                List<EmployeeDesignationViewModel> Employeelist = new List<EmployeeDesignationViewModel>();
                var Dbmodellist = _empDetailRepo.GetDesignations();
                if (Dbmodellist != null && Dbmodellist.Count > 0)
                {
                    foreach (var items in Dbmodellist)
                    {
                        Employeelist.Add(new EmployeeDesignationViewModel
                        {
                            DesignationId = items.DesignationId,
                            Designation = items.Designation
                        });
                    }
                }
                return Employeelist;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }

        }
        public EmployeeDetailViewModel GetEmployeeById(int id)
        {
            try
            {
                EmployeeDetail employeeDetail = _empDetailRepo.GetEmployeeById(id);
                EmployeeDetailViewModel employee = new EmployeeDetailViewModel
                {
                    Id = employeeDetail.Id,
                    Name = employeeDetail.Name,
                    DesignationId = employeeDetail.DesignationId,
                    ProfilePicture = employeeDetail.ProfilePicture,
                    DataOfBirth = employeeDetail.DataOfBirth,
                    Salary = employeeDetail.Salary,
                    Email = employeeDetail.Email,
                    Address = employeeDetail.Address,
                };
                return employee;
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }

        }

        public void AddEmployeeDetail(EmployeeDetailViewModel employeeDetail)
        {
            try
            {
                //mapping Dbmode to view model
                var empDbModel = new EmployeeDetail
                {
                    //Id = employeeDetail.Id,
                    Name = employeeDetail.Name,
                    DesignationId = employeeDetail.DesignationId,
                    ProfilePicture = employeeDetail.ProfilePicture,
                    DataOfBirth = employeeDetail.DataOfBirth,
                    Salary = employeeDetail.Salary,
                    Email = employeeDetail.Email,
                    Address = employeeDetail.Address,
                };
                _empDetailRepo.AddEmployee(empDbModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
        public void UpdateEmployeeDetail(EmployeeDetailViewModel employeeDetail, int id)
        {
            try
            {
                EmployeeDetail employee = _empDetailRepo.GetEmployeeById(employeeDetail.Id);

                employee.Name = employeeDetail.Name;
                employee.DesignationId = employeeDetail.DesignationId;
                employee.ProfilePicture = employeeDetail.ProfilePicture;
                employee.Salary = employeeDetail.Salary;
                employee.DataOfBirth = employeeDetail.DataOfBirth;
                employee.Email = employeeDetail.Email;
                employee.Address = employeeDetail.Address;
                _empDetailRepo.UpdateEmployee(employee, id);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
        public void DeleteEmployee(int id)
        {
            try
            {
                _empDetailRepo.DeleteEmployee(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
        
    }
}
