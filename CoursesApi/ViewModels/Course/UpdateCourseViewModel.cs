namespace CoursesApi.ViewModels
{
    public class UpdateCourseViewModel
    {
        [Required(ErrorMessage = "Course duration is required")]
        public double DurationInHours { get; set; }
        [Required(ErrorMessage = "Course category is required")]
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Desription field is required")]
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        [Required(ErrorMessage = "Details field is required")]
        public string? Details { get; set; }
    }
}