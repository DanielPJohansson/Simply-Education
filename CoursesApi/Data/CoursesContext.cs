using CoursesApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CoursesApi.Data
{
    public class CoursesContext : IdentityDbContext
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<CourseStudent> StudentCourses => Set<CourseStudent>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Address> Addresses => Set<Address>();
        // public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
        public CoursesContext(DbContextOptions options) : base(options)
        {

        }
    }
}