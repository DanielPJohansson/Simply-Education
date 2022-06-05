using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Students
{
    public class UpdateStudent : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public PostStudentViewModel Student { get; set; }

        public UpdateStudent(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var coursesUrl = $"{_baseUrl}/students/{id}";

            using var http = new HttpClient();

            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(coursesUrl);
            var studentToUpdate = JsonSerializer.Deserialize<StudentViewModel>(responseModel.Data);


            StudentId = studentToUpdate.Id;

            Student = new PostStudentViewModel
            {
                FirstName = studentToUpdate.FirstName,
                LastName = studentToUpdate.LastName,
                Street = studentToUpdate.Street,
                ZipCode = studentToUpdate.ZipCode,
                City = studentToUpdate.City,
                Email = studentToUpdate.Email,
                PhoneNumber = studentToUpdate.PhoneNumber,
            };

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/students/{StudentId}";

                using var http = new HttpClient();
                var response = await http.PutAsJsonAsync(url, Student);

                if (!response.IsSuccessStatusCode)
                {
                    string reason = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(reason);
                }
            }
        }
    }
}