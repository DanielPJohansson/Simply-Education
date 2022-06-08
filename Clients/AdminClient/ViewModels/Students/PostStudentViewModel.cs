using System.ComponentModel.DataAnnotations;

namespace AdminClient.ViewModels
{
    public class PostStudentViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Street is required.")]
        public string? Street { get; set; }
        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^([0-9]{5})$", ErrorMessage = "The zip code can only be a five digit number.")]
        [Display(Name = "Zip code")]
        public string? ZipCode { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }
        [EmailAddress(ErrorMessage = "Not a valid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email address")]
        public string? Email { get; set; }
        [Phone(ErrorMessage = "Not a valid phone number format.")]
        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must consist of at least six characters")]
        [Display(Name = "Account password")]
        public string? Password { get; set; }
    }
}