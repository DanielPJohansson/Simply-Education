using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Data
{
    public class CoursesContext : IdentityDbContext
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
        // public DbSet<StudentCourse> StudentCourses => Set<StudentCourse>();
        public CoursesContext(DbContextOptions options) : base(options)
        {

        }
    }
}