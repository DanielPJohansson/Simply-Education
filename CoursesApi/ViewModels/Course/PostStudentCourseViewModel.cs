using System.ComponentModel.DataAnnotations;

namespace CoursesApi.ViewModels
{
    public class PostStudentCourseViewModel
    {
        [Required(ErrorMessage = "Course Id is required.")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Student Id is required.")]
        public int StudentId { get; set; }
    }
}