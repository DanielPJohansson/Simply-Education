using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Category> Competences { get; set; } = new List<Category>();
    }
}