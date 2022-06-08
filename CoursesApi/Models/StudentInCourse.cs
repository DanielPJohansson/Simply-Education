using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class StudentInCourse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }
    }
}