using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public int CourseCode { get; set; }
        public string? Name { get; set; }
        public int Duration { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; } = new Category();
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public bool IsDeprecated { get; set; } = false;
    }
}