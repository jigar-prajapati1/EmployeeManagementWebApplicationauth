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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UsersLoginViewModel _login)
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
        [HttpGet]
        public async Task<IActionResult> Index()

        {
            var token = Request.Cookies["access_token"];

            List<EmployeeDetailViewModel> response = new List<EmployeeDetailViewModel>();
            try
            {
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
                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Login");
                }
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<EmployeeDetailViewModel>>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
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
            var designationList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeDesignationViewModel>>(content);

            ViewBag.Designations = new SelectList(designationList, "DesignationId", "Designation");

            return View("AddEmployee");


        }
        [HttpPost]
        public async Task<IActionResult> AddEmployeeDetail(EmployeeDetailViewModel employee)
        {
            var token = Request.Cookies["access_token"];

            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (employee.ImageFile != null && employee.ImageFile.FileName != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                string extension = Path.GetExtension(employee.ImageFile.FileName);
                employee.ProfilePicture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
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

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var token = Request.Cookies["access_token"].ToString();
            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Newtonsoft.Json.Linq.JObject.Parse(token)["jwtToken"].ToString());
                var response = await client.GetFromJsonAsync<EmployeeDetailViewModel>($"https://localhost:7296/api/EmployeeDetail/{id.ToString()}");
                if (response != null)
                {
                    return View(response);
                }
                return View(null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeDetailViewModel employee)
        {
            var token = Request.Cookies["access_token"].ToString();
            try
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (employee.ImageFile != null && employee.ImageFile.FileName != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    employee.ProfilePicture = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
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
            var token = Request.Cookies["access_token"].ToString();
            try
            {
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
    }
}
