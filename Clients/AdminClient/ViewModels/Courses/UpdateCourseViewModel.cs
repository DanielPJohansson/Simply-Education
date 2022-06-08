using System.ComponentModel.DataAnnotations;

namespace AdminClient.ViewModels
{
    public class UpdateCourseViewModel
    {
        [Display(Name = "Course code")]
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        [Display(Name = "Duration in hours")]
        [Required(ErrorMessage = "Course duration is required")]
        public double DurationInHours { get; set; }
        [Required(ErrorMessage = "Course category is required")]
        public string? Category { get; set; }
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Desription field is required")]
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        [Required(ErrorMessage = "Details field is required")]
        public string? Details { get; set; }
    }
}