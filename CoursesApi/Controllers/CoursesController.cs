using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository _repository;

        public CoursesController(ICoursesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetCourses()
        {
            var response = await _repository.GetCoursesAsync();
            // return Ok(response);
            return Ok(new ResponseViewModel(
                statusCode: 200,
                count: response.Count(),
                data: JsonSerializer.Serialize(response)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            var response = await _repository.GetCourseAsync(id);
            if (response is null)
            {
                return NotFound($"Could not find course with Id: {id}");
            }
            return Ok(new ResponseViewModel(
                statusCode: 200,
                data: JsonSerializer.Serialize(response)));
        }

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategoriesForActiveCourses()
        {
            var response = await _repository.GetCategoriesForActiveCoursesAsync();
            // return Ok(response);
            return Ok(new ResponseViewModel(
                statusCode: 200,
                count: response.Count(),
                data: JsonSerializer.Serialize(response)));
        }

        [HttpGet("{id}/list")]
        public async Task<ActionResult> GetCourseWithStudentsAndTeachers(int id)
        {
            var response = await _repository.GetCourseWithStudentsAndTeachersAsync(id);
            if (response is null)
            {
                return NotFound($"Could not find course with Id: {id}");
            }
            return Ok(new ResponseViewModel(
                statusCode: 200,
                data: JsonSerializer.Serialize(response)));
        }

        [HttpPost("{id}/students")]
        public async Task<ActionResult> AddStudentToCourse(PostStudentCourseViewModel model)
        {
            try
            {
                await _repository.AddStudentToCourseAsync(model);
                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(201);
                }
                return StatusCode(500, "The changes could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/teachers")]
        public async Task<ActionResult> AddTeacherToCourse(PostTeacherToCourseViewModel model)
        {
            try
            {
                await _repository.AddTeacherToCourseAsync(model);
                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(201);
                }
                return StatusCode(500, "The changes could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost()]
        public async Task<ActionResult> AddCourse(PostCourseViewModel model)
        {
            try
            {
                await _repository.AddCourseAsync(model);
                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(201);
                }
                return StatusCode(500, "The course could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, PostCourseViewModel model)
        {
            try
            {
                await _repository.UpdateCourseAsync(id, model);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, "The changes could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> ArchiveCourse(int id)
        {
            try
            {
                await _repository.ArchiveCourseAsync(id);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, "The changes could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}/teachers/{teacherId}")]
        public async Task<ActionResult> RemoveTeacherFromCourse(int id, int teacherId)
        {
            try
            {
                await _repository.RemoveTeacherFromCourseAsync(id, teacherId);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, "The changes could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}/students/{studentId}")]
        public async Task<ActionResult> RemoveStudentFromCourse(int id, int studentId)
        {
            try
            {
                await _repository.RemoveStudentFromCourseAsync(id, studentId);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, "The changes could not be saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}