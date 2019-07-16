using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebAPI_With_Token.DAL
{
    public static class DepartmentDAL
    {
        public static async Task<List<Department>> GetDepartments()
        {
            using(SampleEntities entity = new SampleEntities())
            {
                return await entity.Departments.ToListAsync();
            }
        }
    }
}