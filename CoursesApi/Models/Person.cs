using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }
        public string? Email { get; set; }
        public ICollection<CourseStudent> CoursesAsStudent { get; set; } = new List<CourseStudent>();
        public ICollection<Course> CoursesAsTeacher { get; set; } = new List<Course>();
        public ICollection<Category> Competences { get; set; } = new List<Category>();
    }
}