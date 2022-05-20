using CoursesApi.Interfaces;
using CoursesApi.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _repository;
        public CategoriesController(ICategoriesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}/courses")]
        public async Task<ActionResult> GetCoursesInCategory(int id)
        {
            var response = await _repository.GetCoursesInCategoryAsync(id);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet()]
        public async Task<ActionResult> GetCategories()
        {
            var response = await _repository.GetCategoriesAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var response = await _repository.GetCategoryAsync(id);

            if (response is null)
            {
                return NotFound($"Det finns inget objekt med id: {id}");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(PostCategoryViewModel model)
        {
            try
            {
                await _repository.AddCategoryAsync(model);

                if (await _repository.SaveChangesAsync())
                {
                    return StatusCode(201);
                }

                return StatusCode(500, "Det gick inte att spara kategorin");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, PostCategoryViewModel model)
        {
            try
            {
                await _repository.UpdateCategoryAsync(id, model);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, $"Det gick inte att spara uppdateringen av kategorin till: {id}, {model.Name}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                await _repository.DeleteCategoryAsync(id);
                if (await _repository.SaveChangesAsync())
                {
                    return NoContent();
                }
                return StatusCode(500, $"Det gick inte att spara borttagningen av kategorin med id: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}