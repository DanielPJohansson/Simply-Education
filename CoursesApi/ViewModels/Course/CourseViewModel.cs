using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public int CourseCode { get; set; }
        public string? Name { get; set; }
        public int Duration { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }
    }
}