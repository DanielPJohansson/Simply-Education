using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Students
{
    public class AddStudent : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public PostStudentViewModel Student { get; set; }

        public AddStudent(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/students";

                using var http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, Student);

                if (!response.IsSuccessStatusCode)
                {
                    string reason = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(reason);
                }
            }
        }
    }
}