using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI_With_Token.Models
{
    public class ResponseStatusModel<T>
    {
        public bool status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}