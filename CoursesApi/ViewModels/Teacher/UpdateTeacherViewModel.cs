using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesApi.ViewModels
{
    public class UpdateTeacherViewModel
    {
        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "The zip code can only be a five digit number.")]
        public string? ZipCode { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phone number is required.")]
        public string? PhoneNumber { get; set; }
        public List<string> Competences { get; set; } = new List<string>();
    }
}