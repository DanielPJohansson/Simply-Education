using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? PersonId { get; set; }
        [ForeignKey("PersonId")]
        [Required]
        public Person? Person { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Category> Competences { get; set; } = new List<Category>();
    }
}