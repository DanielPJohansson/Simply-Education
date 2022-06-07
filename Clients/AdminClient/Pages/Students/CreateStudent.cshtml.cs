using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Students
{
    public class CreateStudent : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public PostStudentViewModel Student { get; set; } = new PostStudentViewModel();
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public CreateStudent(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/students";

                using var http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, Student);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage.Message = "Student successfully added.";
                    StatusMessage.IsSuccess = true;
                }
                else
                {
                    string reason = await response.Content.ReadAsStringAsync();
                    StatusMessage.Message = reason;
                    StatusMessage.IsSuccess = false;
                }
            }
        }
    }
}