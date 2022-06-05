using System.Text.Json;
using AdminClient.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminClient.Pages.Courses
{
    public class ListStudents : PageModel
    {
        private readonly IConfiguration _config;

        [BindProperty]
        public IEnumerable<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();


        public ListStudents(IConfiguration config)
        {
            _config = config;
        }

        public async Task OnGetAsync()
        {
            var baseUrl = _config.GetValue<string>("apiUrl");
            var url = $"{baseUrl}/students/list";

            using var http = new HttpClient();
            var responseModel = await http.GetFromJsonAsync<ResponseViewModel>(url);

            if (!string.IsNullOrEmpty(responseModel!.Data))
            {
                Students = JsonSerializer.Deserialize<IEnumerable<StudentViewModel>>(responseModel.Data)!;
            }
        }
    }
}