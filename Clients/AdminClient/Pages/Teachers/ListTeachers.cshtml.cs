using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Courses
{
    public class ListTeachers : PageModel
    {
        private readonly IConfiguration _config;

        [BindProperty]
        public IEnumerable<TeacherViewModel> Teachers { get; set; } = new List<TeacherViewModel>();
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public ListTeachers(IConfiguration config)
        {
            _config = config;
        }

        public async Task OnGetAsync()
        {
            var baseUrl = _config.GetValue<string>("apiUrl");

            var url = String.IsNullOrEmpty(SearchString) ? $"{baseUrl}/teachers/list" : $"{baseUrl}/teachers/category/{SearchString}";

            using var http = new HttpClient();
            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(url);

            if (!string.IsNullOrEmpty(responseModel!.Data))
            {
                Teachers = JsonSerializer.Deserialize<IEnumerable<TeacherViewModel>>(responseModel.Data)!;
            }
        }
    }
}