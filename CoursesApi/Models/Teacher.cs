using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public ApplicationUser? User { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Category> Competences { get; set; } = new List<Category>();
    }
}