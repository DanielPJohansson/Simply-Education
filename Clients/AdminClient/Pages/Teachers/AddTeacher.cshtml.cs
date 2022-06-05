using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AdminClient.Pages.Teachers
{
    public class AddTeacher : PageModel
    {
        private readonly ILogger<AddTeacher> _logger;

        public AddTeacher(ILogger<AddTeacher> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}