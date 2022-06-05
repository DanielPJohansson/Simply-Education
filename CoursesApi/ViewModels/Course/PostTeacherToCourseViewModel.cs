using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.ViewModels
{
    public class PostTeacherToCourseViewModel
    {
        [Required(ErrorMessage = "Course Id is required.")]
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Teacher Id is required.")]
        public int TeacherId { get; set; }
    }
}