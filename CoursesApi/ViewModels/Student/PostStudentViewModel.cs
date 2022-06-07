namespace CoursesApi.ViewModels
{
    public class PostStudentViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "The zip code can only be a five digit number.")]
        public string? ZipCode { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Phone]
        [Required(ErrorMessage = "Phone number is required.")]
        public string? PhoneNumber { get; set; }
    }
}