using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdminClient.Services;
using System.Text;

namespace AdminClient.Pages.Teachers
{
    public class UpdateTeacher : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public int TeacherId { get; set; }
        [BindProperty]
        public UpdateTeacherViewModel Teacher { get; set; } = new UpdateTeacherViewModel();
        [BindProperty]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public UpdateTeacher(IConfiguration config)
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

            var teacherToUpdate = JsonSerializer.Deserialize<TeacherViewModel>(teacherResponseModel!.Data);

            if (teacherToUpdate is not null)
            {
                Teacher = new UpdateTeacherViewModel
                {
                    FirstName = teacherToUpdate.FirstName,
                    LastName = teacherToUpdate.LastName,
                    Street = teacherToUpdate.Street,
                    ZipCode = teacherToUpdate.ZipCode,
                    City = teacherToUpdate.City,
                    Email = teacherToUpdate.Email,
                    PhoneNumber = teacherToUpdate.PhoneNumber,
                    Competences = teacherToUpdate.Competences
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
                var url = $"{_baseUrl}/teachers/{TeacherId}";

                var serializedTeacherData = JsonSerializer.Serialize(new
                {
                    Street = Teacher.Street,
                    ZipCode = Teacher.ZipCode,
                    City = Teacher.City,
                    PhoneNumber = Teacher.PhoneNumber,
                    Competences = Teacher.Competences
                });

                using var http = new HttpClient();
                var requestContent = new StringContent(serializedTeacherData, Encoding.UTF8, "application/json-patch+json");
                var response = await http.PatchAsync(url, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage.Message = "Teacher information updated.";
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