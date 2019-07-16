using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebAPI_With_Token.BAL;
using WebAPI_With_Token.Models;

namespace WebAPI_With_Token.DAL
{
    public static class UserDAL
    {
        public static async Task<bool> isEmailExist(string email)
        {
            using (SampleEntities entity = new SampleEntities())
            {
                bool isExsit = true;
                Employee emp = await entity.Employees.Where(x => x.EmailId == email).FirstOrDefaultAsync();
                if (emp == null)
                {
                    isExsit = false;
                }
                return isExsit;
            }
        }

        public static async Task<long> addEmployee(Employee employee)
        {
            long employeeId = 0;
            using (SampleEntities entity = new SampleEntities())
            {
                if (!await isEmailExist(employee.EmailId))
                {
                    entity.Employees.Add(employee);
                    await entity.SaveChangesAsync();
                    employeeId = employee.Id;
                }
            }
            return employee.Id;
        }

        public static async Task<List<sp_GetEmployee_Result>> getAllEmployee()
        {
            using (SampleEntities entity = new SampleEntities())
            {
                var employeeList = await entity.Database.SqlQuery<sp_GetEmployee_Result>("exec sp_GetEmployee").ToListAsync();
                return employeeList;
            }
        }

        public static async Task<List<sp_GetEmployee_for_datatable_Result>> getEmployees(string SearchValue, int FirstRow, int LastRow, string SortColumn, string SortOrder)
        {
            using (SampleEntities entity = new SampleEntities())
            {
                var employeeList = await entity.Database.SqlQuery<sp_GetEmployee_for_datatable_Result>
                    ("exec sp_GetEmployee_for_datatable @SearchValue, @FirstRow, @LastRow, @SortColumn, @SortOrder ", new SqlParameter[]{
                new SqlParameter("@SearchValue",SearchValue),
                new SqlParameter("@FirstRow",FirstRow),
                new SqlParameter("LastRow",LastRow),
                new SqlParameter("@SortColumn",SortColumn),
                new SqlParameter("@SortOrder",SortOrder)
                }).ToListAsync();
                return employeeList;
            }
        }

        public static async Task<sp_GetEmployeeById_Result> getEmployeeById(long id)
        {
            using (SampleEntities entity = new SampleEntities())
            {
                SqlParameter paraId = new SqlParameter("@Id", System.Data.SqlDbType.BigInt);
                paraId.Value = id;
                var employeeList = await entity.Database.SqlQuery<sp_GetEmployeeById_Result>("exec sp_GetEmployeeById @Id", paraId).FirstOrDefaultAsync();
                return employeeList;
            }
        }

        public static async Task<bool> updateEmployee(Employee employee)
        {
            bool isSuccess = false;
            using (SampleEntities entity = new SampleEntities())
            {
                var emp = await entity.Employees.Where(x => x.Id != employee.Id && x.EmailId == employee.EmailId).FirstOrDefaultAsync();
                if (emp == null)
                {
                    emp = await entity.Employees.Where(x => x.Id == employee.Id).FirstOrDefaultAsync();
                    emp.FirstName = employee.FirstName;
                    emp.LastName = employee.LastName;
                    emp.EmailId = employee.EmailId;
                    emp.DepartmentId = employee.DepartmentId;
                    emp.CityId = employee.CityId;
                    emp.Gender = employee.Gender;

                    await entity.SaveChangesAsync();
                    isSuccess = true;
                    return isSuccess;
                }
                else
                {
                    return isSuccess;
                }
            }
        }
    }
}