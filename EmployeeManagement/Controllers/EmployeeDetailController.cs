﻿using CommonModels.DbModels;
using CommonModels.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Azure.Core;
using System.Net.Http;

namespace EmployeeManagement.Controllers
{

    public class EmployeeDetailController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly TokenInterceptor _tokenInterceptor;
        public IConfiguration configuration;
        public EmployeeDetailController(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment, IConfiguration iConfig, TokenInterceptor tokenInterceptor)
        {
            this.httpClientFactory = httpClientFactory;
            _apiClient = httpClientFactory.CreateClient("API");
            _hostEnvironment = hostEnvironment;
            configuration = iConfig;
            _tokenInterceptor = tokenInterceptor;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        /// <summary>Registrations the specified registration.</summary>
        /// <param name="_registration">The registration.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistrationViewModel _registration)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync("https://localhost:7296/api/Auth/EmployeeRegistration", _registration);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                }
                return RedirectToAction("Registration");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>Logins the specified login.</summary>
        /// <param name="_login">The login.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Login(UsersLoginViewModel _login)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    List<UsersLogin> loginDetails = new List<UsersLogin>();
                    var response = await client.PostAsJsonAsync("https://localhost:7296/api/Auth/Login", _login);
                    if (response.IsSuccessStatusCode)
                    {
                        // Token generation is successful
                        var token = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(token))
                        {
                            // Store Token in a secure cookie:
                            HttpContext.Session.SetString("JWToken", token);
                            return RedirectToAction("Index");
                        }
                    }
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return BadRequest("Login Unsuccessful: " + ex.Message);
            }
            
        }

        /// <summary>Indexes this instance.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(token))
                {
                    // Handle the case when the token is null or empty
                    return RedirectToAction("Login");
                }

                List<EmployeeDetailViewModel> response = new List<EmployeeDetailViewModel>();

                // Get All EmployeeDetail from WebApi
             
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = _tokenInterceptor.GetAuthorizationHeader();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var apiUrl = "https://localhost:7296/api/EmployeeDetail/GetAll";
                var httpResponseMessage = await client.GetAsync(apiUrl);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login");
                }

                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<EmployeeDetailViewModel>>());

                return View(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving employee details: " + ex.Message);
            }
        }

        /// <summary>Adds the employee.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                List<EmployeeDesignationViewModel> response = new List<EmployeeDesignationViewModel>();
                //Get All EmployeeDetail from WebApi
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var apiUrl = "https://localhost:7296/api/EmployeeDetail/GetDesignation";
                var httpResponseMessage = await client.GetAsync(apiUrl);
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login");
                }
                httpResponseMessage.EnsureSuccessStatusCode();
                var designationList = JsonConvert.DeserializeObject<List<EmployeeDesignationViewModel>>(content);

                ViewBag.Designations = new SelectList(designationList, "DesignationId", "Designation");

                return View("AddEmployee");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>Adds the employee detail.</summary>
        /// <param name="employee">The employee.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddEmployeeDetail(EmployeeDetailViewModel employee)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");

                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (employee.ImageFile != null && employee.ImageFile.FileName != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    employee.ProfilePicture = fileName = fileName + DateTime.Now.ToString("ddMMyyhhmmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "Images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await employee.ImageFile.CopyToAsync(fileStream);
                    }
                }

                employee.ImageFile = null;
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var response = await client.PostAsJsonAsync("https://localhost:7296/api/EmployeeDetail/AddEmployee", employee); // Await the response asynchronously
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "EmployeeDetail Created Successfully";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("AddEmployee");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Edits the employee.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            try
            {
                var designation = GetDesignation();
                ViewBag.Designations = new SelectList(designation, "DesignationId", "Designation");
                var token = HttpContext.Session.GetString("JWToken");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var response = await client.GetFromJsonAsync<EmployeeDetailViewModel>($"https://localhost:7296/api/EmployeeDetail/{id.ToString()}");

                if (response != null)
                {
                    return View(response);
                }
                return View(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        /// <summary>Edits the employee.</summary>
        /// <param name="employee">The employee.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeDetailViewModel employee)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (employee.ImageFile != null && employee.ImageFile.FileName != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    employee.ProfilePicture = fileName = fileName + DateTime.Now.ToString("ddMMyyhhmmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "Images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await employee.ImageFile.CopyToAsync(fileStream);
                    }
                }

                employee.ImageFile = null;
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7296/api/EmployeeDetail/{employee.Id}"),
                    Content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json")
                };
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login"); // Redirect to login page
                }
                httpResponseMessage.EnsureSuccessStatusCode();
                var response = await httpResponseMessage.Content.ReadFromJsonAsync<EmployeeDetailViewModel>();

                if (httpResponseMessage != null)
                {
                    TempData["success"] = "EmployeeDetail Updated Successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>Deletes the employee.</summary>
        /// <param name="employee">The employee.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(EmployeeDetailViewModel employee)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWToken");
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7296/api/EmployeeDetail/{employee.Id}");
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login");
                }
                httpResponseMessage.EnsureSuccessStatusCode();

                TempData["success"] = "EmployeeDetail Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not delete {ex.Message}");
            }
        }
        /// <summary>Gets the designation.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        [NonAction]
        public List<EmployeeDetailViewModel> GetDesignation()
        {
            var token = HttpContext.Session.GetString("JWToken");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var response = client.GetAsync("https://localhost:7296/api/EmployeeDetail/GetDesignation").Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsAsync<List<EmployeeDetailViewModel>>().Result;
                }
            }
            return new List<EmployeeDetailViewModel>();
        }
    }
}


