using CommonModels.DbModels;
using CommonModels.ViewModel;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorys.Implements
{
    public class EmployeeDetailRepository : IEmployeeDetailRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string conn;
        /// <summary>Initializes a new instance of the <see cref="EmployeeDetailRepository" /> class.</summary>
        /// <param name="configuration">The configuration.</param>
        public EmployeeDetailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = _configuration.GetConnectionString("DefaultConnection");
        }
        //public List<UserRegistration> EmployeeRegistration(UserRegistration _registration)
        //{
        //    var parameter = new DynamicParameters();
        //    using IDbConnection connection = new SqlConnection(conn);
        //    parameter.Add("@Email", _registration.Email);
        //    parameter.Add("@Password", _registration.Password);
        //    return connection.Query<UserRegistration>("UserRegistration", parameter, commandType: CommandType.StoredProcedure).ToList();

        //}
        //public List<UsersLogin> EmployeeLogin(UsersLogin _login)
        //{
        //    var parameter = new DynamicParameters();
        //    using IDbConnection connection = new SqlConnection(conn);
        //    parameter.Add("@Email", _login.Email);
        //    parameter.Add("@Password", _login.password);
            
        //        return connection.Query<UsersLogin>("UsersLogin", parameter, commandType: CommandType.StoredProcedure).ToList();
            
        //}
        public List<EmployeeDesignation> GetDesignations()
        {
            try
            {
                using IDbConnection connection = new SqlConnection(conn);
                return connection.Query<EmployeeDesignation>("GetDesignation", commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeDetail> GetAllEmployee()
        {
            try
            {
                using IDbConnection connection = new SqlConnection(conn);
                return connection.Query<EmployeeDetail>("GetAllEmployee", commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public EmployeeDetail GetEmployeeById(int id)
        {
            try
            {
                var parameter = new DynamicParameters();
                using IDbConnection connection = new SqlConnection(conn);
                parameter.Add("@Id", id);
                return connection.QuerySingleOrDefault<EmployeeDetail>("GetEmployeeById", parameter, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<EmployeeDetail> AddEmployee(EmployeeDetail employee)
        {
            try
            {
                var parameter = new DynamicParameters();
                using IDbConnection connection = new SqlConnection(conn);
                {
                    parameter.Add("@Name", employee.Name);
                    parameter.Add("@DesignationId", employee.DesignationId);
                    parameter.Add("@ProfilePicture", employee.ProfilePicture);
                    parameter.Add("@Salary", employee.Salary);
                    parameter.Add("@DataOfBirth", employee.DataOfBirth);
                    parameter.Add("@Email", employee.Email);
                    parameter.Add("@Address", employee.Address);
                    return connection.Query<EmployeeDetail>("AddEmployee", parameter, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<EmployeeDetail> UpdateEmployee(EmployeeDetail employee, int id)
        {
            try
            {
                var parameter = new DynamicParameters();
                using IDbConnection connection = new SqlConnection(conn);
                {
                    parameter.Add("@Id", employee.Id);
                    parameter.Add("@Name", employee.Name);
                    parameter.Add("@DesignationId", employee.DesignationId);
                    parameter.Add("@ProfilePicture", employee.ProfilePicture);
                    parameter.Add("@Salary", employee.Salary);
                    parameter.Add("@DataOfBirth", employee.DataOfBirth);
                    parameter.Add("@Email", employee.Email);
                    parameter.Add("@Address", employee.Address);
                    return connection.Query<EmployeeDetail>("UpdateEmployee", parameter, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeDetail DeleteEmployee(int id)
        {
            try
            {
                var parameter = new DynamicParameters();
                using IDbConnection connection = new SqlConnection(conn);
                parameter.Add("@Id", id);
                return connection.QuerySingleOrDefault<EmployeeDetail>("DeleteEmployee", parameter, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
