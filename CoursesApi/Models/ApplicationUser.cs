using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoursesApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        // public ICollection<Course> TeachingCourses { get; set; } = new List<Course>();
        // public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        // public ICollection<Category> Competences { get; set; } = new List<Category>();
    }
}