using System.Text;
using System.Text.Json;
using AdminClient.Services;
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
        public UpdateCourseViewModel Course { get; set; } = new UpdateCourseViewModel();
        [BindProperty]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public UpdateCourse(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CourseId = id;
            var coursesUrl = $"{_baseUrl}/courses/{id}";

            using var http = new HttpClient();

            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(coursesUrl);
            var courseToUpdate = JsonSerializer.Deserialize<CourseViewModel>(responseModel!.Data);

            if (courseToUpdate is not null)
            {
                Course = new UpdateCourseViewModel
                {
                    CourseCode = courseToUpdate.CourseCode,
                    Name = courseToUpdate.Name,
                    DurationInHours = courseToUpdate.DurationInHours,
                    Category = courseToUpdate.Category,
                    ImageUrl = courseToUpdate.ImageUrl,
                    Description = courseToUpdate.Description,
                    Details = courseToUpdate.Details,
                };
            }

            var selectListBuilder = new SelectListBuilder(_baseUrl);
            Categories = await selectListBuilder.PopulateCategorySelectListAsync();

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/courses/{CourseId}";

                var serializedStudentData = JsonSerializer.Serialize(new
                {
                    DurationInHours = Course.DurationInHours,
                    Category = Course.Category,
                    ImageUrl = Course.ImageUrl,
                    Description = Course.Description,
                    Details = Course.Details,
                });

                using var http = new HttpClient();
                var requestContent = new StringContent(serializedStudentData, Encoding.UTF8, "application/json-patch+json");
                var response = await http.PatchAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage.Message = "Course information updated.";
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