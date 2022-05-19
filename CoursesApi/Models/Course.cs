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
        public int CourseNo { get; set; }
        public string? Name { get; set; }
        public int Length { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public bool IsReprecated { get; set; } = false;
    }
}