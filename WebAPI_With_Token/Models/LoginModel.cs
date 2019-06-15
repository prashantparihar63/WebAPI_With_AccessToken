using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI_With_Token.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Username is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Username must be between 6 to 50 charactors")]
        public string userName { get; set; }
        public string password { get; set; }
    }
}