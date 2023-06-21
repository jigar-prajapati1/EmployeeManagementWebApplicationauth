using CommonModels.DbModels;
using CommonModels.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EmployeeManagement.Controllers
{
    public class EmployeeDetailController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IWebHostEnvironment _hostEnvironment;
        public EmployeeDetailController(IHttpClientFactory httpClientFactory, IWebHostEnvironment hostEnvironment)
        {
            this.httpClientFactory = httpClientFactory;
            _hostEnvironment = hostEnvironment;
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
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var apiUrl = "https://localhost:7296/api/EmployeeDetail";
                var httpResponseMessage = await client.GetAsync(apiUrl);
                httpResponseMessage.EnsureSuccessStatusCode();
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployeeDetail(EmployeeDetailViewModel employee)
        {
            try
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (employee.ImageFile != null && employee.ImageFile.FileName != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    employee.ProfilePicture = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "Images", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await employee.ImageFile.CopyToAsync(fileStream);
                    }
                }
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7296/api/EmployeeDetail/");
                var json = JsonSerializer.Serialize(employee);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("AddEmployee", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var postResponse = JsonSerializer.Deserialize<EmployeeDetailViewModel>(responseContent);
                    TempData["success"] = "EmployeeDetail Created Successfully";
                    return RedirectToAction("Index"); // Adjust the action name and parameters as needed
                }
                else
                {
                    TempData["error"] = "Failed to create EmployeeDetail.";
                    return RedirectToAction("Index"); // Adjust the action name and parameters as needed
                }

                return View();
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
                var client = httpClientFactory.CreateClient();
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
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7296/api/EmployeeDetail/{employee.Id}"),
                    Content = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json")
                };
                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
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
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7296/api/EmployeeDetail/{employee.Id}");
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
