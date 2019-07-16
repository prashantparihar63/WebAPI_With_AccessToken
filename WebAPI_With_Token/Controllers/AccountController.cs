using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using WebAPI_With_Token.BAL;
using WebAPI_With_Token.DAL;
using WebAPI_With_Token.Mapping;
using WebAPI_With_Token.Models;
using WebAPI_With_Token.Providers;
using WebAPI_With_Token.Results;

namespace WebAPI_With_Token.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        ILog logger = LogManager.GetLogger("ErrorLog");
        #region Private members and constructor

        private const string LocalLoginProvider = "Local";

        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }
        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #endregion

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(LoginModel model)
        {
            logger.Info("Controller:Accountr, Method:Login, Parameters={Username:" + model.userName + ", Password" + model.password + "}");
            ResponseStatusModel<BearerTokenModel> response = new ResponseStatusModel<BearerTokenModel>();
            response.data = new BearerTokenModel();
            if (ModelState.IsValid)
            {
                string tokenUrl = CommonHelper.GetSiteUrl() + "token";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", model.userName),
                        new KeyValuePair<string, string>("password", model.password)
                    });

                    HttpResponseMessage result = await httpClient.PostAsync(tokenUrl, content);
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    var token = JsonConvert.DeserializeObject<BearerTokenModel>(resultContent);
                    if (!string.IsNullOrEmpty(token.access_token))
                    {

                        response.code = Convert.ToInt32(HttpStatusCode.OK);
                        response.status = true;
                        response.data = token;
                        response.message = "Success";
                        return Content(HttpStatusCode.OK, response);
                    }
                    else
                    {
                        response.code = Convert.ToInt32(HttpStatusCode.BadRequest);
                        response.status = false;
                        response.data = null;
                        response.message = "Invalid username or password.";
                        return Content(HttpStatusCode.BadRequest, response);
                    }
                }
            }
            else
            {
                response.code = Convert.ToInt32(HttpStatusCode.BadRequest);
                response.status = false;
                response.message = CommonHelper.GetModalErrorResult(ModelState);
                return Content(HttpStatusCode.BadRequest, response);
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(EmployeeRegistrationModel model)
        {
            ResponseStatusModel<EmployeeRegistrationModel> response = new ResponseStatusModel<EmployeeRegistrationModel>();
            try
            {
                if (ModelState.IsValid)
                {
                    var isEmailExist = await UserBAL.isEmailExist(model.email);
                    if (!isEmailExist)
                    {
                        var newApplicationUser = new ApplicationUser() { UserName = model.email, Email = model.email };
                        IdentityResult result = await UserManager.CreateAsync(newApplicationUser, model.password);
                        if (result.Succeeded)
                        {
                            model.userId = newApplicationUser.Id;
                            long employeeId = await UserBAL.addEmployee(model);
                            if (employeeId > 0)
                            {
                                model.id = employeeId;
                                response.code = Convert.ToInt32(HttpStatusCode.OK);
                                response.status = true;
                                response.data = model;
                                response.message = "Record saved successfully.";
                                return Content(HttpStatusCode.OK, response);
                            }
                            else
                            {
                                await UserManager.DeleteAsync(newApplicationUser);
                                response.code = Convert.ToInt32(HttpStatusCode.BadRequest);
                                response.status = false;
                                response.message = "Email is already exist.";
                                return Content(HttpStatusCode.BadRequest, response);
                            }
                        }
                        else
                        {
                            var applicationUser = await UserManager.FindByEmailAsync(model.email);
                            if (applicationUser != null)
                            {
                                await UserManager.DeleteAsync(applicationUser);
                            }
                            response.code = Convert.ToInt32(HttpStatusCode.Conflict);
                            response.status = false;
                            response.message = "Email is already exist.";
                            return Content(HttpStatusCode.Conflict, response);
                        }
                    }
                    else
                    {
                        response.code = Convert.ToInt32(HttpStatusCode.Conflict);
                        response.status = false;
                        response.message = "User email already exist.";
                        return Content(HttpStatusCode.Conflict, response);
                    }
                }
                else
                {
                    response.code = Convert.ToInt32(HttpStatusCode.BadRequest);
                    response.status = false;
                    response.message = CommonHelper.GetModalErrorResult(ModelState);
                    return Content(HttpStatusCode.BadRequest, response);
                }
            }
            catch (Exception ex)
            {
                var applicationUser = await UserManager.FindByEmailAsync(model.email);
                if (applicationUser != null)
                {
                    await UserManager.DeleteAsync(applicationUser);
                }

                response.code = Convert.ToInt32(HttpStatusCode.BadRequest);
                response.status = false;
                response.message = ex.InnerException.Message;
                return Content(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IHttpActionResult> GetUserList()
        {
            logger.Info("Controller:Accountr, Method:GetUserList()");
            ResponseStatusModel<List<EmployeeModel>> response = new ResponseStatusModel<List<EmployeeModel>>();
            if (ModelState.IsValid)
            {
                response.data = new List<EmployeeModel>();
                var users = UserManager.Users;
                response.data.AddRange(await UserBAL.listEmployee());
                response.code = Convert.ToInt32(HttpStatusCode.OK);
                response.status = true;
                response.message = "";
                return Content(HttpStatusCode.OK, response);

            }
            else
            {
                response.code = Convert.ToInt32(HttpStatusCode.BadRequest);
                response.status = false;
                response.message = CommonHelper.GetModalErrorResult(ModelState);
                return Content(HttpStatusCode.BadRequest, response);
            }
        }

        #region Helpers

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        #endregion
    }
}
