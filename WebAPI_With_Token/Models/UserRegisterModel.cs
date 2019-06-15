using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI_With_Token.Models
{
    public class UserRegisterModel
    {
        public long userId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Username must be in between 8 to 50 charactors")]
        public string userName { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be in between 6 to 50 charactors")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password should contain 1 capital letter, 1 special charactor and 1 numeric value.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
        public string email { get; set; }
        public string departmentId { get; set; }
    }
}