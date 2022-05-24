using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : Controller
    {
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ILogger<StudentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudent(int id)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetStudents()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddStudent()
        {
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateStudent()
        {
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteStudent()
        {
            return NoContent();
        }
    }
}