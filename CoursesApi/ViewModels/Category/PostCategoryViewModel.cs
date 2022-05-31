using System.ComponentModel.DataAnnotations;

namespace CoursesApi.ViewModels
{
    public class PostCategoryViewModel
    {
        [Required]
        public string? Name { get; set; }
    }
}