using System.ComponentModel.DataAnnotations;

namespace AdminClient.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Display(Name = "Course code")]
        public string? CourseCode { get; set; }
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Display(Name = "Duration in hours")]
        public double DurationInHours { get; set; }
        [Display(Name = "Category")]
        public string? Category { get; set; }
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }
    }
}