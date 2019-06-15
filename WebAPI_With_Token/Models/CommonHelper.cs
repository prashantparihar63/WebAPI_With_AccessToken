using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace WebAPI_With_Token.Models
{
    public class CommonHelper
    {
        public static string GetModalErrorResult(ModelStateDictionary modelState)
        {
            string message = "";
            foreach (ModelState state in modelState.Values)
            {
                foreach (ModelError error in state.Errors)
                {
                    if (message == "")
                    {
                        message = error.ErrorMessage;
                    }
                    else
                    {
                        message = message + "/" + error.ErrorMessage;
                    }
                }
            }
            return message;
        }
        public static string GetSiteUrl()
        {
            string url = string.Empty;
            HttpRequest request = HttpContext.Current.Request;

            if (request.IsSecureConnection)
                url = "https://";
            else
                url = "http://";

            url += request["HTTP_HOST"] + "/";

            return url;
        }
    }
}