namespace CoursesApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}