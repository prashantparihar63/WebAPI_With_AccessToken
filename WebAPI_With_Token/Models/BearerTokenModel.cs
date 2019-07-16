using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_With_Token.Models
{
    public class BearerTokenModel
    {
        public string access_token { get; set; }
    }
}