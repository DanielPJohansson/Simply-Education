namespace CoursesApi.ViewModels
{
    public class CategoryWithTeachersViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public IEnumerable<TeacherViewModel> Teachers { get; set; } = new List<TeacherViewModel>();
    }
}