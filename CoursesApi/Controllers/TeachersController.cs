using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeachersController : Controller
    {
        private readonly ILogger<TeachersController> _logger;

        public TeachersController(ILogger<TeachersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}