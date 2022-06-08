using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public ApplicationUser? User { get; set; }
        public ICollection<StudentInCourse> Courses { get; set; } = new List<StudentInCourse>();
    }
}