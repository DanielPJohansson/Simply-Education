using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeachersController : Controller
    {
        private readonly ITeachersRepository _repository;

        public TeachersController(ITeachersRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTeacher(int id)
        {
            var response = await _repository.GetTeacherAsync(id);

            if (response is null)
            {
                return NotFound($"Could not find teacher with Id: {id}");
            }

            return Ok(new ResponseViewModel(
                statusCode: 200,
                data: JsonSerializer.Serialize(response)));
        }

        [HttpGet("category/{name}")]
        public async Task<ActionResult> GetTeacher(string name)
        {
            var response = await _repository.GetTeachersAsync(name);

            if (response is null)
            {
                return NotFound($"Could not find teachers matching category: {name}");
            }

            return Ok(new ResponseViewModel(
                statusCode: 200,
                data: JsonSerializer.Serialize(response)));
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetTeachers()
        {
            var response = await _repository.GetTeachersAsync();
            return Ok(new ResponseViewModel(
                statusCode: 200,
                count: response.Count(),
                data: JsonSerializer.Serialize(response)));
        }

        [HttpPost]
        public async Task<ActionResult> AddTeacher(PostTeacherViewModel model)
        {
            try
            {
                await _repository.AddTeacherAsync(model);
                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(201);
                }

                return StatusCode(500, "Could not save changes when adding teacher.");
            }
            catch (BadHttpRequestException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTeacher(int id, PostTeacherViewModel model)
        {
            try
            {
                await _repository.UpdateTeacherAsync(id, model);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }

                return StatusCode(500, "Could not save changes when updating teacher.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            try
            {
                await _repository.DeleteTeacherAsync(id);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, "Could not save the changes.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}