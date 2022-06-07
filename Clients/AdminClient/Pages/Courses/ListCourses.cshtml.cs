using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Courses
{
    public class ListCourses : PageModel
    {
        private readonly IConfiguration _config;

        [BindProperty]
        public IEnumerable<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();

        public ListCourses(IConfiguration config)
        {
            _config = config;
        }

        public async Task OnGetAsync()
        {
            var baseUrl = _config.GetValue<string>("apiUrl");
            var url = $"{baseUrl}/courses/list";

            using var http = new HttpClient();
            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(url);

            if (!string.IsNullOrEmpty(responseModel!.Data))
            {
                Courses = JsonSerializer.Deserialize<IEnumerable<CourseViewModel>>(responseModel.Data)!;
            }
        }
    }
}