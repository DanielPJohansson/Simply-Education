using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ICollection<StudentCourse> Courses { get; set; } = new List<StudentCourse>();
    }
}