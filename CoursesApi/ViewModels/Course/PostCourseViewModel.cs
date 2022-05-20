using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Models;

namespace CoursesApi.ViewModels.Course
{
    public class PostCourseViewModel
    {
        [Required]
        public int CourseCode { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        [Required]
        public string? Details { get; set; }
    }
}