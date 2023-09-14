using Login.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Login.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(ILogger<LoginController> logger, IHttpContextAccessor _httpContextAccessor)
        {
            _logger = logger;            
            httpContextAccessor = _httpContextAccessor;
        }

        public static string Hash(string key)
        {
            var Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(key));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        [HttpPost]
        [Route("Login")]
        public async void Login([FromBody] Credentials credentials)
        {

            var context = httpContextAccessor.HttpContext;
            var username = credentials.Username;
            var passwordHash = Hash(credentials.Password);

            //var user = new User
            //{
            //    Email = username,
            //    PasswordHash = passwordHash,
            //};

            //if (!userRepository.IsValidUser(user))
            //{
            //    throw new Exception();
            //}

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credentials.Username)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
                );
        }
    }
}