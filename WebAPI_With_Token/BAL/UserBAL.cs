using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebAPI_With_Token.DAL;
using WebAPI_With_Token.Models;

namespace WebAPI_With_Token.BAL
{
    public static class UserBAL
    {
        public static async Task<bool> isEmailExist(string email)
        {
            return await UserDAL.isEmailExist(email);
        }

        public static async Task<long> addEmployee(EmployeeRegistrationModel model)
        {
            Employee employee = Mapping.EmployeMap(model);
            return await UserDAL.addEmployee(employee);
        }

        public static async Task<List<EmployeeModel>> listEmployee()
        {
            List<sp_GetEmployee_Result> employees = await UserDAL.getAllEmployee();
            return Mapping.EmployeModelListMap(employees);
        }

        public static async Task<EmployeeModel> getEmployeeDetails(long id)
        {
            sp_GetEmployeeById_Result employees = await UserDAL.getEmployeeById(id);
            return Mapping.EmployeModelMapWithEmployee(employees);
        }

        public static async Task<EmployeeUpdateModel> getEmployeeById(long id)
        {
            sp_GetEmployeeById_Result employees = await UserDAL.getEmployeeById(id);
            return Mapping.EmployeModelMapWithEmployeeUpdateModel(employees);
        }

        public static async Task<bool> updateEmployee(EmployeeUpdateModel employee)
        {
            Employee emp = Mapping.EmployeMapWithEmployeeUpdateModel(employee);
            return await UserDAL.updateEmployee(emp);
        }


    }
}