using System.ComponentModel.DataAnnotations;

namespace AdminClient.ViewModels
{
    public class PostCourseViewModel
    {
        [Display(Name = "Course code")]
        [RegularExpression(@"^([0-9]{4})$", ErrorMessage = "The course code can only be a four digit code.")]
        [Required(ErrorMessage = "Course code is required")]
        public string? CourseCode { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Course Name is required")]
        public string? Name { get; set; }
        [Display(Name = "Duration in hours")]
        [Required(ErrorMessage = "Course duration is required")]
        public double DurationInHours { get; set; }
        [Display(Name = "Category")]
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