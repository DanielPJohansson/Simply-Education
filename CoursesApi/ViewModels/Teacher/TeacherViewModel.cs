namespace CoursesApi.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Competences { get; set; } = new List<string>();
    }
}