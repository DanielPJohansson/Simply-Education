using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoursesApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IConfiguration _config;
        public AuthenticationController(IConfiguration config, UserManager<Person> userManager, SignInManager<Person> signInManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserViewModel model)
        {
            var user = new Person
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(user, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("Student", "true")
                });

                var userData = new UserViewModel
                {
                    UserName = user.UserName,
                    Expires = DateTime.Now.AddDays(1),
                    Token = await CreateJwt(user)
                };

                return StatusCode(201, userData);
            }
            else
            {
                var errorModel = new ErrorViewModel
                {
                    StatusCode = 500,
                    StatusMessage = "Error during registration of user"
                };
                foreach (var error in result.Errors)
                {
                    errorModel.Errors.Add(error.Code, error.Description);
                }

                return StatusCode(500, errorModel);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null)
            {
                return Unauthorized("Incorrect username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                var userData = new UserViewModel
                {
                    UserName = user.UserName,
                    Expires = DateTime.Now.AddDays(1),
                    Token = await CreateJwt(user)
                };

                return StatusCode(201, userData);
            }
            else
            {
                return Unauthorized("Incorrect password");
            }
        }

        private async Task<string> CreateJwt(Person user)
        {
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("apiKey"));
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);
            var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();

            var jwt = new JwtSecurityToken(
                claims: userClaims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}