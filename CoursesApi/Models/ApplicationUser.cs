using Microsoft.AspNetCore.Identity;

namespace CoursesApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        // public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        // public string? Email { get; set; }
        // public string? PhoneNumber { get; set; }
    }
}