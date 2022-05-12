using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Length { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Description { get; set; }
        public string? Details { get; set; }
    }
}