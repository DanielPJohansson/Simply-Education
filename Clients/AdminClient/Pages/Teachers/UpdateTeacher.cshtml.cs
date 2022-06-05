using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AdminClient.Pages.Teachers
{
    public class UpdateTeacher : PageModel
    {
        private readonly ILogger<UpdateTeacher> _logger;

        public UpdateTeacher(ILogger<UpdateTeacher> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}