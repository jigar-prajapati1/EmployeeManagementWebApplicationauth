using EmployeeServices.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeServices.Implementations
{
    public class TokenService : ITokenservice
    {
        private readonly IConfiguration configuration;
        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreatejWTToken(ClaimsIdentity claimsIdentity)
        {
            try
            {
                // Create claims
                var Authclaim = new List<Claim>(claimsIdentity.Claims);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims: Authclaim,
                expires: DateTime.Now.AddHours(15),
                signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Went Wrong" + ex.Message);
            }
        }
    }
}
