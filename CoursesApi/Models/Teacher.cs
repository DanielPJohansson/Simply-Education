using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Category> Competences { get; set; } = new List<Category>();
    }
}