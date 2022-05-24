using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Interfaces;
using CoursesApi.ViewModels;
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

        [HttpGet()]
        public async Task<ActionResult> GetCourses()
        {
            var response = await _repository.GetCoursesAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            var response = await _repository.GetCourseAsync(id);
            if (response is null)
            {
                return NotFound($"Det finns ingen kurs med id: {id}");
            }
            return Ok(response);
        }

        [HttpGet("{id}/students")]
        public async Task<ActionResult> GetCourseWithStudents(int id)
        {
            return Ok();
        }

        [HttpPost("{id}/students")]
        public async Task<ActionResult> AddStudentToCourse(int id)
        {
            return StatusCode(201);
        }

        [HttpDelete("{id}/students/{studentId}")]
        public async Task<ActionResult> RemoveStudentFromCourse(int id, int studentId)
        {
            return NoContent();
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
                return StatusCode(500, "Det gick inte att spara den tillagda kursen.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, PostCourseViewModel model)
        {
            try
            {
                await _repository.UpdateCourseAsync(id, model);
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
                return StatusCode(500, "Det gick inte att spara ändringarna.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}