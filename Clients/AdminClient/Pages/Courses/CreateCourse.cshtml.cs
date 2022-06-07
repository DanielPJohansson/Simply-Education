using System.Text.Json;
using AdminClient.Services;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminClient.Pages.Courses
{
    public class CreateCourse : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public PostCourseViewModel Course { get; set; } = new PostCourseViewModel();
        [BindProperty]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public CreateCourse(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var selectListBuilder = new SelectListBuilder(_baseUrl);
            Categories = await selectListBuilder.PopulateCategorySelectListAsync();

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/courses";

                using var http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, Course);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage.Message = "Course successfully added.";
                    StatusMessage.IsSuccess = true;
                }
                else
                {
                    string reason = await response.Content.ReadAsStringAsync();
                    StatusMessage.Message = reason;
                    StatusMessage.IsSuccess = false;
                }
            }

            var selectListBuilder = new SelectListBuilder(_baseUrl);
            Categories = await selectListBuilder.PopulateCategorySelectListAsync();
        }
    }
}