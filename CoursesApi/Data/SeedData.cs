using System.Text.Json;
using CoursesApi.Models;

namespace CoursesApi.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new CoursesContext(serviceProvider.GetRequiredService<DbContextOptions<CoursesContext>>());
            await context.Database.MigrateAsync();
            await SeedStudents(context);
            await SeedCategories(context);
            await SeedTeachers(context);
            await SeedCourses(context);

        }

        public static async Task SeedStudents(CoursesContext context)
        {
            if (await context.Students.AnyAsync())
            {
                return;
            }

            var studentData = await File.ReadAllTextAsync("Data/students.json");
            var students = JsonSerializer.Deserialize<List<Student>>(studentData);

            await context.AddRangeAsync(students!);
            await context.SaveChangesAsync();
        }

        public static async Task SeedTeachers(CoursesContext context)
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
                var newTeacher = new Teacher
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Address = teacher.Address,
                    Email = teacher.Email,
                    Competences = competences
                };

                context.Teachers.Add(newTeacher);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedCategories(CoursesContext context)
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
        public static async Task SeedCourses(CoursesContext context)
        {
            // if (await context.Courses.AnyAsync())
            // {
            //     return;
            // }

            // var courseData = await File.ReadAllTextAsync("Data/courses.json");
            // var courses = JsonSerializer.Deserialize<List<Course>>(courseData);

            // await context.AddRangeAsync(courses!);
            // await context.SaveChangesAsync();
        }

    }
}