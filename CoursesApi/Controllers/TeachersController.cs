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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTeacher(int id)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetTeachers()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddTeacher()
        {
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTeacher()
        {
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTeacher()
        {
            return NoContent();
        }

    }
}