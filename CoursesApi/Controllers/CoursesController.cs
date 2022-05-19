using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Interfaces;
using CoursesApi.ViewModels.Course;
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
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCourse(int id)
        {
            return Ok();
        }

        [HttpPost()]
        public async Task<ActionResult> AddCourse(PostCourseViewModel model)
        {
            return StatusCode(201);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, PatchCourseViewModel model)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            return NoContent();
        }
    }
}