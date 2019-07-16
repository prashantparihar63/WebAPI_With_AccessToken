using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAPI_With_Token.DAL;

namespace WebAPI_With_Token.Models
{
    public class EmployeeModel
    {
        public long id { get; set; }

        public string userId { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string confirmPassword { get; set; }

        public string department { get; set; }

        public long departmentId { get; set; }

        public string gender { get; set; }

        public int cityId { get; set; }
        public string city { get; set; }

        public int countryId { get; set; }
        public string country { get; set; }
    }


    public class EmployeeRegistrationModel
    {
        public long id { get; set; }

        public string userId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be in between 2 to 50 charactors")]
        public string firstName { get; set; }

        public string lastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Email must be in between 8 to 50 charactors")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "password")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be in between 8 to 30 charactors")]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmPassword")]
        [Compare("password", ErrorMessage = "New password and confirm password do not match.")]
        [Required(ErrorMessage = "Confirm password is required")]
        public string confirmPassword { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public long departmentId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string gender { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int cityId { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int countryId { get; set; }

    }

    public class EmployeeUpdateModel
    {
        public long id { get; set; }

        public string userId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be in between 2 to 50 charactors")]
        public string firstName { get; set; }

        public string lastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Email must be in between 8 to 50 charactors")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string email { get; set; }
        
        [Required(ErrorMessage = "Department is required")]
        public long departmentId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string gender { get; set; }

        [Required(ErrorMessage = "City is required")]
        public int cityId { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int countryId { get; set; }

    }
}