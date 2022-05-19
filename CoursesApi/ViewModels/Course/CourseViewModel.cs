using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.ViewModels.Course
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public int CourseNo { get; set; }
        public string? Name { get; set; }
        public int Length { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }
    }
}