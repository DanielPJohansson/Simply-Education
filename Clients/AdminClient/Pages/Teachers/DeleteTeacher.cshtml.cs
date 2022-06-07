using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Teachers
{
    public class DeleteTeacher : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int TeacherId { get; set; }
        [BindProperty]
        public TeacherViewModel? Teacher { get; set; }
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public DeleteTeacher(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            TeacherId = id;

            var teachersUrl = $"{_baseUrl}/teachers/{id}";

            using var http = new HttpClient();

            var teacherResponseModel = await http.GetFromJsonAsync<ResponseViewModel>(teachersUrl);

            Teacher = JsonSerializer.Deserialize<TeacherViewModel>(teacherResponseModel!.Data);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var url = $"{_baseUrl}/teachers/{TeacherId}";

            using var http = new HttpClient();
            var response = await http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Teachers/ListTeachers");
            }

            string reason = await response.Content.ReadAsStringAsync();
            StatusMessage.Message = reason;
            StatusMessage.IsSuccess = false;

            return Page();
        }
    }
}