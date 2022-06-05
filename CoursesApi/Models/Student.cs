using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<CourseStudent> Courses { get; set; } = new List<CourseStudent>();
    }
}