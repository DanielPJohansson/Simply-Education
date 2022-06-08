using System.Text;
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
        public UpdateStudentViewModel Student { get; set; } = new UpdateStudentViewModel();
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public UpdateStudent(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            StudentId = id;
            var coursesUrl = $"{_baseUrl}/students/{id}";

            using var http = new HttpClient();

            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(coursesUrl);

            var studentToUpdate = JsonSerializer.Deserialize<StudentViewModel>(responseModel!.Data);

            if (studentToUpdate is not null)
            {
                Student = new UpdateStudentViewModel
                {
                    FirstName = studentToUpdate.FirstName,
                    LastName = studentToUpdate.LastName,
                    Street = studentToUpdate.Street,
                    ZipCode = studentToUpdate.ZipCode,
                    City = studentToUpdate.City,
                    Email = studentToUpdate.Email,
                    PhoneNumber = studentToUpdate.PhoneNumber
                };
            }

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/students/{StudentId}";

                var serializedStudentData = JsonSerializer.Serialize(new
                {
                    Street = Student.Street,
                    ZipCode = Student.ZipCode,
                    City = Student.City,
                    PhoneNumber = Student.PhoneNumber
                });

                using var http = new HttpClient();
                // var response = await http.PutAsJsonAsync(url, Student);
                var requestContent = new StringContent(serializedStudentData, Encoding.UTF8, "application/json-patch+json");
                var response = await http.PatchAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage.Message = "Student information updated.";
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