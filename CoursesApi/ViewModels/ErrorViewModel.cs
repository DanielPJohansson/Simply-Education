using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.ViewModels
{
    public class ErrorViewModel
    {
        public int StatusCode { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
        public string? StatusMessage { get; set; }
    }
}