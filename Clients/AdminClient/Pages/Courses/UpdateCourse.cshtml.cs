using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminClient.Pages.Courses
{
    public class UpdateCourse : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int CourseId { get; set; }
        [BindProperty]
        public PostCourseViewModel Course { get; set; }
        [BindProperty]
        public List<SelectListItem> Categories { get; set; }

        public UpdateCourse(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {


            var coursesUrl = $"{_baseUrl}/courses/{id}";
            var categoriesUrl = $"{_baseUrl}/categories/list";

            using var http = new HttpClient();

            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(coursesUrl);
            var courseToUpdate = JsonSerializer.Deserialize<CourseViewModel>(responseModel.Data);


            CourseId = courseToUpdate.CourseId;

            Course = new PostCourseViewModel
            {
                CourseCode = courseToUpdate.CourseCode,
                Name = courseToUpdate.Name,
                DurationInHours = courseToUpdate.DurationInHours,
                Category = courseToUpdate.Category,
                ImageUrl = courseToUpdate.ImageUrl,
                Description = courseToUpdate.Description,
                Details = courseToUpdate.Details,
            };

            responseModel = await http.GetFromJsonAsync<ResponseViewModel>(categoriesUrl);
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(responseModel.Data);

            Categories = categories.Select(cat => new SelectListItem
            {
                Value = cat.Name,
                Text = cat.Name
            }).ToList();

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/courses/{CourseId}";

                using var http = new HttpClient();
                var response = await http.PutAsJsonAsync(url, Course);

                if (!response.IsSuccessStatusCode)
                {
                    string reason = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(reason);
                }
            }
        }
    }
}