using CoursesApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoursesApi.Data
{
    public class DataContext : IdentityDbContext<Person>
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<StudentInCourse> StudentCourses => Set<StudentInCourse>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Person> People => Set<Person>();
        // public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }
}