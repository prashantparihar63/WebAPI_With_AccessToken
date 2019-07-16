using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAPI_With_Token.BAL;
using WebAPI_With_Token.DAL;
using WebAPI_With_Token.Models;


namespace WebAPI_With_Token.Controllers
{
    public class EmployeeController : Controller
    {
        private SampleEntities entity = new SampleEntities();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await UserBAL.listEmployee());
        }


        [HttpGet]
        public ActionResult DataTable()
        {
            return View();
        }

        public async Task<ActionResult> LoadData()
        {
            try
            {
                var draws = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0;

                // Getting all Customer data    
                var customerData = await UserDAL.getAllEmployee();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    var propertyInfo = typeof(sp_GetEmployee_Result).GetProperty(sortColumn);
                    if (sortColumnDir == "desc")
                    {
                        customerData = customerData.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        customerData = customerData.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                    }
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => (m.FirstName.ToLower().Contains(searchValue.ToLower()) || m.LastName.ToLower().Contains(searchValue.ToLower()) || m.EmailId.ToLower().Contains(searchValue.ToLower()) || m.Gender.ToLower().Contains(searchValue.ToLower()) || m.CityName.ToLower().Contains(searchValue.ToLower()) || m.CountryName.ToLower().Contains(searchValue.ToLower()))).ToList();
                }

                //total number of rows count     
                totalRecords = customerData.Count();

                //Paging     
                var employeeList = customerData.Skip(skip).Take(pageSize).ToList();
                //var data = customerData.ToList();

                //Returning Json Data    
                var result = Json(new { draw = draws, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = employeeList }, JsonRequestBehavior.AllowGet);
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DataTableWithStoredProcedure()
        {
            return View();
        }
        public async Task<ActionResult> LoadDataWithStoredProcedure()
        {
            try
            {
                var draws = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                long totalRecords = 0;

                // Getting all Customer data    
                var customerData = await UserDAL.getEmployees(searchValue, Convert.ToInt32(start), Convert.ToInt32(start) + Convert.ToInt32(length), sortColumn, sortColumnDir);
                if (customerData.Count > 0)
                {
                    totalRecords = Convert.ToInt64(customerData[0].Total);
                }
                //Returning Json Data    
                var result = Json(new { draw = draws, recordsTotal = totalRecords, recordsFiltered = totalRecords, data = customerData }, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            ViewBag.Added = "false";
            ViewBag.DepartmentId = new SelectList(await DepartmentBAL.getDepartment(), "Id", "DepartmentName");
            ViewBag.CountryId = new SelectList(entity.Countries, "Id", "CountryName");
            ViewBag.CityId = new SelectList(entity.Cities, "Id", "CityName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(EmployeeRegistrationModel employee)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string endPoint = CommonHelper.GetSiteUrl() + "API/Account/register";
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("firstName", employee.firstName),
                        new KeyValuePair<string, string>("lastName", employee.lastName),
                        new KeyValuePair<string, string>("email", employee.email),
                        new KeyValuePair<string, string>("password", employee.password),
                        new KeyValuePair<string, string>("confirmPassword", employee.confirmPassword),
                        new KeyValuePair<string, string>("departmentId", employee.departmentId.ToString()),
                        new KeyValuePair<string, string>("gender", employee.gender),
                        new KeyValuePair<string, string>("cityId", employee.cityId.ToString()),
                        new KeyValuePair<string, string>("countryId", employee.countryId.ToString()),
                    });

                    HttpResponseMessage result = await httpClient.PostAsync(endPoint, content);
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    var response = JsonConvert.DeserializeObject<AddEmployeeResponse>(resultContent);
                    if (!string.IsNullOrEmpty(response.data.id.ToString()))
                    {
                        ViewBag.Added = "true";
                    }
                }
            }
            ViewBag.DepartmentId = new SelectList(await DepartmentBAL.getDepartment(), "Id", "DepartmentName");
            ViewBag.CountryId = new SelectList(entity.Countries, "Id", "CountryName");
            ViewBag.CityId = new SelectList(entity.Cities, "Id", "CityName");
            return View(employee);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long id)
        {
            ViewBag.Updated = "false";
            EmployeeUpdateModel employee;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                employee = await UserBAL.getEmployeeById(id);
            }
            ViewBag.DepartmentId = new SelectList(await DepartmentBAL.getDepartment(), "Id", "DepartmentName", employee.departmentId);
            ViewBag.CountryId = new SelectList(entity.Countries, "Id", "CountryName", employee.countryId);
            ViewBag.CityId = new SelectList(entity.Cities, "Id", "CityName", employee.cityId);
            return View(employee);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EmployeeUpdateModel employee)
        {
            if (ModelState.IsValid)
            {
                bool isUpdated = await UserBAL.updateEmployee(employee);
                if (isUpdated)
                {
                    ViewBag.Updated = "true";

                }
            }
            ViewBag.DepartmentId = new SelectList(await DepartmentBAL.getDepartment(), "Id", "DepartmentName");
            ViewBag.CountryId = new SelectList(entity.Countries, "Id", "CountryName");
            ViewBag.CityId = new SelectList(entity.Cities, "Id", "CityName");
            return View(employee);
        }
    }
}