using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.ViewModels.Course;

namespace CoursesApi.ViewModels.Category
{
    public class CategoryWithCoursesViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();
    }
}