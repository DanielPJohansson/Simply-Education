namespace CoursesApi.ViewModels
{
    public class CategoryWithCoursesViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();
    }
}