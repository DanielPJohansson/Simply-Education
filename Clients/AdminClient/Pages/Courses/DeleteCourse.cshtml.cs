using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Courses
{
    public class DeleteCourse : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int CourseId { get; set; }
        [BindProperty]
        public CourseViewModel? Course { get; set; }
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public DeleteCourse(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CourseId = id;

            var coursessUrl = $"{_baseUrl}/courses/{id}";

            using var http = new HttpClient();

            var courseResponseModel = await http.GetFromJsonAsync<ResponseViewModel>(coursessUrl);

            Course = JsonSerializer.Deserialize<CourseViewModel>(courseResponseModel!.Data);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var url = $"{_baseUrl}/courses/{CourseId}";

            using var http = new HttpClient();
            var response = await http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Courses/ListCourses");
            }

            string reason = await response.Content.ReadAsStringAsync();
            StatusMessage.Message = reason;
            StatusMessage.IsSuccess = false;

            return Page();
        }
    }
}