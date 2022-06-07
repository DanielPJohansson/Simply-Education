using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Students
{
    public class DeleteStudent : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public StudentViewModel? Student { get; set; }
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public DeleteStudent(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            StudentId = id;

            var teachersUrl = $"{_baseUrl}/students/{id}";

            using var http = new HttpClient();

            var teacherResponseModel = await http.GetFromJsonAsync<ResponseViewModel>(teachersUrl);

            Student = JsonSerializer.Deserialize<StudentViewModel>(teacherResponseModel!.Data);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var url = $"{_baseUrl}/students/{StudentId}";

            using var http = new HttpClient();
            var response = await http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Students/ListStudents");
            }

            string reason = await response.Content.ReadAsStringAsync();
            StatusMessage.Message = reason;
            StatusMessage.IsSuccess = false;

            return Page();
        }
    }
}