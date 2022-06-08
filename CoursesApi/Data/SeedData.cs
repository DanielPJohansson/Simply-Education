using System.Text.Json;
using CoursesApi.Models;

namespace CoursesApi.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new DataContext(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>());
            await context.Database.MigrateAsync();
            await SeedStudents(context);
            await SeedCategories(context);
            await SeedTeachers(context);
            await SeedCourses(context);

        }

        public static async Task SeedStudents(DataContext context)
        {
            if (await context.Students.AnyAsync())
            {
                return;
            }

            var studentData = await File.ReadAllTextAsync("Data/students.json");
            var students = JsonSerializer.Deserialize<List<PostStudentViewModel>>(studentData);

            foreach (var student in students!)
            {
                var person = new ApplicationUser
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Street = student.Street,
                    ZipCode = student.ZipCode,
                    City = student.City,
                    Email = student.Email,
                    PhoneNumber = student.PhoneNumber
                };
                var newStudent = new Student
                {
                    User = person
                };

                context.Students.Add(newStudent);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedTeachers(DataContext context)
        {
            if (await context.Teachers.AnyAsync())
            {
                return;
            }

            var teacherData = await File.ReadAllTextAsync("Data/teachers.json");
            var teachers = JsonSerializer.Deserialize<List<PostTeacherViewModel>>(teacherData);

            foreach (var teacher in teachers!)
            {
                var competences = new List<Category>();
                foreach (var competence in teacher.Competences)
                {
                    var category = await context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == competence!.ToLower());
                    if (category is null)
                    {
                        return;
                    }
                    competences.Add(category);
                }
                var person = new ApplicationUser
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Street = teacher.Street,
                    ZipCode = teacher.ZipCode,
                    City = teacher.City,
                    Email = teacher.Email,
                    PhoneNumber = teacher.PhoneNumber
                };
                var newTeacher = new Teacher
                {
                    User = person,
                    Competences = competences
                };

                context.Teachers.Add(newTeacher);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedCategories(DataContext context)
        {
            if (await context.Categories.AnyAsync())
            {
                return;
            }

            var categoryData = await File.ReadAllTextAsync("Data/categories.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);

            await context.AddRangeAsync(categories!);
            await context.SaveChangesAsync();
        }
        public static async Task SeedCourses(DataContext context)
        {
            if (await context.Courses.AnyAsync())
            {
                return;
            }

            var courseData = await File.ReadAllTextAsync("Data/courses.json");
            var courses = JsonSerializer.Deserialize<List<PostCourseViewModel>>(courseData);

            foreach (var course in courses!)
            {
                var category = await context.Categories.SingleOrDefaultAsync(c => c.Name.ToLower() == course.Category.ToLower());

                var newCourse = new Course
                {
                    CourseCode = course.CourseCode,
                    Name = course.Name,
                    DurationInHours = course.DurationInHours,
                    Category = category!,
                    Description = course.Description,
                    Details = course.Details,
                    ImageUrl = course.ImageUrl
                };

                await context.Courses.AddAsync(newCourse);
            }
            await context.SaveChangesAsync();
        }
    }
}