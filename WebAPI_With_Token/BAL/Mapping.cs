using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_With_Token.DAL;
using WebAPI_With_Token.Models;

namespace WebAPI_With_Token.BAL
{
    public static class Mapping
    {
        public static Employee EmployeMap(EmployeeRegistrationModel model)
        {
            Employee employee = new Employee();

            employee.FirstName = model.firstName;
            employee.LastName = model.lastName;
            employee.UserId = model.userId;
            employee.EmailId = model.email;
            employee.Password = model.password;
            employee.DepartmentId = model.departmentId;
            employee.Gender = model.gender;
            employee.CityId = model.cityId;
            employee.CountryId = model.countryId;
            return employee;
        }

        public static Employee EmployeMapWithEmployeeUpdateModel(EmployeeUpdateModel model)
        {
            Employee employee = new Employee();
            employee.Id = model.id;
            employee.FirstName = model.firstName;
            employee.LastName = model.lastName;
            employee.UserId = model.userId;
            employee.EmailId = model.email;
            employee.DepartmentId = model.departmentId;
            employee.Gender = model.gender;
            employee.CityId = model.cityId;
            employee.CountryId = model.countryId;
            return employee;
        }
        
        public static List<EmployeeModel> EmployeModelListMap(List<sp_GetEmployee_Result> employees)
        {
            List<EmployeeModel> employeesModel = new List<EmployeeModel>();
            foreach (sp_GetEmployee_Result emp in employees)
            {
                employeesModel.Add(new EmployeeModel()
                {
                    id=emp.Id,
                    userId = emp.UserId,
                    firstName = emp.FirstName,
                    lastName = emp.LastName,
                    email = emp.EmailId,
                    departmentId = (long)emp.DepartmentId,
                    department = emp.DepartmentName,
                    gender = emp.Gender,
                    city = emp.CityName,
                    cityId = (int)emp.CityId,
                    country = emp.CountryName,
                    countryId = (int)emp.CountryId
                });
            }
            return employeesModel;
        }

        public static EmployeeModel EmployeModelMapWithEmployee(sp_GetEmployeeById_Result emp)
        {
            EmployeeModel employee = new EmployeeModel()
            {
                userId = emp.UserId,
                firstName = emp.FirstName,
                lastName = emp.LastName,
                email = emp.EmailId,
                departmentId = (long)emp.DepartmentId,
                department = emp.DepartmentName,
                gender = emp.Gender,
                city = emp.CityName,
                cityId = (int)emp.CityId,
                country = emp.CountryName,
                countryId = (int)emp.CountryId
            };
            return employee;
        }

        public static EmployeeUpdateModel EmployeModelMapWithEmployeeUpdateModel(sp_GetEmployeeById_Result emp)
        {
            EmployeeUpdateModel employee = new EmployeeUpdateModel()
            {
                id = emp.Id,
                userId = emp.UserId,
                firstName = emp.FirstName,
                lastName = emp.LastName,
                email = emp.EmailId,
                departmentId = (long)emp.DepartmentId,
                gender = emp.Gender,
                cityId = (int)emp.CityId,
                countryId = (int)emp.CountryId
            };
            return employee;
        }

        public static List<DepartmentModel> DepartmentModelListMap(List<Department> departments)
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            foreach (Department department in departments)
            {
                model.Add(
                    new DepartmentModel()
                    {
                        id = department.Id,
                        departmentName = department.DepartmentName
                    });
            }
            return model;
        }
        
    }
}