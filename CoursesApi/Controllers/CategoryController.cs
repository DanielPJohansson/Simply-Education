using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/category")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("{id}/list")]
        public async Task<ActionResult> GetCoursesInCategory(int id)
        {
            return Ok();
        }
    }
}