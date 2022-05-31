namespace CoursesApi.ViewModels
{
    public class PostTeacherViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Zip code is required.")]
        public string? ZipCode { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phone number is required.")]
        public string? PhoneNumber { get; set; }
        public List<string> Competences { get; set; } = new List<string>();
    }
}