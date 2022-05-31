using System.ComponentModel.DataAnnotations;

namespace CoursesApi.ViewModels
{
    public class PostCourseViewModel
    {
        [Required(ErrorMessage = "Course code is required")]
        public string? CourseCode { get; set; }
        [Required(ErrorMessage = "Course Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Course duration is required")]
        public double DurationInHours { get; set; }
        [Required(ErrorMessage = "Course category is required")]
        public string? Category { get; set; }
        [Required(ErrorMessage = "Desription field is required")]
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        [Required(ErrorMessage = "Details field is required")]
        public string? Details { get; set; }
    }
}