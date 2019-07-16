using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebAPI_With_Token.DAL;
using WebAPI_With_Token.Models;

namespace WebAPI_With_Token.BAL
{
    public static class DepartmentBAL
    {
        public static async Task<List<DepartmentModel>> getDepartment()
        {
            var departmentList = await DepartmentDAL.GetDepartments();
            return Mapping.DepartmentModelListMap(departmentList);
        }

    }
}