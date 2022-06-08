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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IStudentsRepository _studentsRepository;

        public AuthenticationController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("register/student")]
        public async Task<ActionResult> RegisterUserAsStudent(RegisterUserViewModel model)
        {
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded && await CreateStudent(model.Email!))
            {
                await _userManager.AddClaimsAsync(newUser, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, newUser.Email!),
                    new Claim("Student", "true")
                });

                var userData = new UserViewModel
                {
                    UserName = newUser.UserName,
                    Expires = DateTime.Now.AddDays(1),
                    Token = await CreateJwt(newUser)
                };

                return StatusCode(201, userData);
            }
            else if (result.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
            }

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

        private async Task<string> CreateJwt(ApplicationUser user)
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

        private async Task<bool> CreateStudent(string email)
        {
            try
            {
                await _studentsRepository.CreateStudentForUserAsync(email);
                if (await _studentsRepository.SaveChangesAsync())
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}