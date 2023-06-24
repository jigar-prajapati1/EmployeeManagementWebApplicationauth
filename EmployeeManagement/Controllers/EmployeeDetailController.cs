using CommonModels.DbModels;
using CommonModels.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace EmployeeManagement.Controllers
{
    public class EmployeeDetailController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IWebHostEnvironment _hostEnvironment;
        public IConfiguration configuration;
        public EmployeeDetailController(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment, IConfiguration iConfig)
        {
            this.httpClientFactory = httpClientFactory;
            _hostEnvironment = hostEnvironment;
            configuration = iConfig;
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
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
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict
                            };
                            Response.Cookies.Append("access_token", token, cookieOptions);
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token))
            {
                // Handle the case when the access_token cookie is null or empty
                return RedirectToAction("Login");
            }
            try
            {
            List<EmployeeDetailViewModel> response = new List<EmployeeDetailViewModel>();

                //Get All EmployeeDetail from WebApi
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var apiUrl = "https://localhost:7296/api/EmployeeDetail/GetAll";
                var httpResponseMessage = await client.GetAsync(apiUrl);
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login");
                }
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<EmployeeDetailViewModel>>());
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return View(response);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving employee details: " + ex.Message);
            }
            return View("Error");
        }
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            try
            {
                var token = Request.Cookies["access_token"];
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
        [HttpPost]
        public async Task<IActionResult> AddEmployeeDetail(EmployeeDetailViewModel employee)
        {
            try
            {
                var token = Request.Cookies["access_token"];

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

        [HttpGet]

        public async Task<IActionResult> EditEmployee(int id)
        {
            try
            {
                
                var designation = GetDesignation();
                ViewBag.Designations = new SelectList(designation, "DesignationId", "Designation");
                var token = Request.Cookies["access_token"].ToString();
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
    [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeDetailViewModel employee)
        {
            try
            {
                var token = Request.Cookies["access_token"].ToString();
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

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(EmployeeDetailViewModel employee)
        {
          try 
          { 
                var token = Request.Cookies["access_token"].ToString();
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
        [NonAction]
        public List<EmployeeDetailViewModel> GetDesignation()
        {
            var token = Request.Cookies["access_token"].ToString();
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
