using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : Controller
    {
        private readonly IStudentsRepository _repository;

        public StudentsController(IStudentsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetStudent(int id)
        {
            var response = await _repository.GetStudentAsync(id);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(new ResponseViewModel(
                statusCode: 200,
                data: JsonSerializer.Serialize(response)));
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetStudents()
        {
            var response = await _repository.GetStudentsAsync();
            return Ok(new ResponseViewModel(
                statusCode: 200,
                count: response.Count(),
                data: JsonSerializer.Serialize(response)));
        }

        [HttpPost]
        public async Task<ActionResult> AddStudent(PostStudentViewModel model)
        {
            try
            {
                await _repository.AddStudentAsync(model);
                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(201);
                }

                return StatusCode(500, "Could not save changes when adding student.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(int id, PostStudentViewModel model)
        {
            try
            {
                await _repository.UpdateStudentAsync(id, model);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return StatusCode(500, "Could not save changes when updating student.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            try
            {
                await _repository.DeleteStudentAsync(id);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, "Det gick inte att spara ändringarna.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}