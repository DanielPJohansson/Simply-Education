using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        public double DurationInHours { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = new Category();
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<CourseStudent> StudentCourses { get; set; } = new List<CourseStudent>();
        public bool IsDeprecated { get; set; } = false;
    }
}