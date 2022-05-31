namespace CoursesApi.ViewModels
{
    public class CourseWithStudentsAndTeachersViewModel
    {
        public int CourseId { get; set; }
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        public IEnumerable<StudentViewModel> Students { get; set; } = new List<StudentViewModel>();
        public IEnumerable<TeacherViewModel> Teachers { get; set; } = new List<TeacherViewModel>();
    }
}