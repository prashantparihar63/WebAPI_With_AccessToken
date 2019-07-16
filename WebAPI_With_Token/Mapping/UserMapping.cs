using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI_With_Token.Models;

namespace WebAPI_With_Token.Mapping
{
    public static class EmployeeMapping
    {
        public static List<LoginModel> userListMapping(IQueryable<ApplicationUser> users)
        {
            List<LoginModel> userList = new List<LoginModel>();
            foreach(var user in users)
            {
                userList.Add(new LoginModel() { password = user.PasswordHash, userName = user.UserName });
            }
            return userList;
        }
    }
}