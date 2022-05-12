using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    public class CoursesController : ControllerBase
    {
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
        public async Task<ActionResult> AddCourse()
        {
            return StatusCode(201);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateCourse(int id)
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