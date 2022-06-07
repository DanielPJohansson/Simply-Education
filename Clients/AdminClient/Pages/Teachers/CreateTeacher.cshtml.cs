using System.Text.Json;
using AdminClient.Services;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminClient.Pages.Teachers
{
    public class CreateTeacher : PageModel
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        [BindProperty]
        public PostTeacherViewModel Teacher { get; set; } = new PostTeacherViewModel();
        [BindProperty]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public StatusMessage StatusMessage { get; set; } = new StatusMessage();

        public CreateTeacher(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var selectListBuilder = new SelectListBuilder(_baseUrl);
            Categories = await selectListBuilder.PopulateCategorySelectListAsync();

            return Page();
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var url = $"{_baseUrl}/teachers/";

                using var http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, Teacher);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage.Message = "Teacher successfully added.";
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