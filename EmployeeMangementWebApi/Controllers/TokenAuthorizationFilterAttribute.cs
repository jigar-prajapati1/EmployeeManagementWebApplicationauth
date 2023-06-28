using Microsoft.AspNetCore.Mvc.Filters;


namespace EmployeeManagement.Controllers
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenAuthorizationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var requestHeaders = _httpContextAccessor.HttpContext.Request.Headers;
            if (!requestHeaders.ContainsKey("Authorization"))
            {
                var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    requestHeaders.Add("Authorization", "Bearer " + token);
                }
            }

        }
    }
}