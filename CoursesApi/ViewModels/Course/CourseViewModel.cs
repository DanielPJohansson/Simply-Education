namespace CoursesApi.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string? CourseCode { get; set; }
        public string? Name { get; set; }
        public double DurationInHours { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

        //Potentiellt behövs formatering lagras här
        public string? Details { get; set; }
    }
}