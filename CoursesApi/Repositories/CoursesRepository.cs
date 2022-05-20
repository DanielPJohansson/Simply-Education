using CoursesApi.Data;
using CoursesApi.Interfaces;
using CoursesApi.Models;
using CoursesApi.ViewModels.Course;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly CoursesContext _context;
        public CoursesRepository(CoursesContext context)
        {
            _context = context;
        }

        public async Task<CourseViewModel?> GetCourseAsync(int id)
        {
            return await _context.Courses.Where(c => c.Id == id)
            .Include(c => c.Category)
            .Include(c => c.Teachers)
            .Select(c => new CourseViewModel
            {
                CourseId = c.Id,
                CourseCode = c.CourseCode,
                Name = c.Name,
                Duration = c.Duration,
                Category = c.Category.Name,
                Description = c.Description,
                Details = c.Details
            })
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync()
        {
            return await _context.Courses.Where(c => c.IsDeprecated == false)
            .Include(c => c.Category)
            .Include(c => c.Teachers)
            .Select(c => new CourseViewModel
            {
                CourseId = c.Id,
                CourseCode = c.CourseCode,
                Name = c.Name,
                Duration = c.Duration,
                Category = c.Category.Name,
                Description = c.Description,
                Details = c.Details
            }).ToListAsync();
        }

        public async Task AddCourseAsync(PostCourseViewModel model)
        {
            if (await _context.Courses.AnyAsync(c => c.CourseCode == model.CourseCode))
            {
                throw new Exception($"Det finns redan en kurs med kod: {model.CourseCode}");
            }

            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == model.Category!.ToLower());

            if (category is null)
            {
                throw new Exception($"Kategorin {model.Category!.ToLower()} finns inte i databasen.");
            }

            var courseToAdd = new Course
            {
                CourseCode = model.CourseCode,
                Name = model.Name,
                Duration = model.Duration,
                Description = model.Description,
                Details = model.Details,
                Category = category
            };

            await _context.Courses.AddAsync(courseToAdd);
        }

        public async Task UpdateCourseAsync(int id, PostCourseViewModel model)
        {
            var courseToUpdate = await _context.Courses.SingleOrDefaultAsync(c => c.Id == id);

            if (courseToUpdate is null)
            {
                throw new Exception($"Hittade ingen kurs med id: {id}");
            }

            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == model.Category!.ToLower());

            if (category is null)
            {
                throw new Exception($"Kategorin {model.Category!.ToLower()} finns inte i databasen.");
            }

            courseToUpdate.CourseCode = model.CourseCode;
            courseToUpdate.Name = model.Name;
            courseToUpdate.Duration = model.Duration;
            courseToUpdate.Description = model.Description;
            courseToUpdate.Details = model.Details;
            courseToUpdate.Category = category;

            _context.Courses.Update(courseToUpdate);
        }

        public async Task ArchiveCourseAsync(int id)
        {
            var courseToArchive = await _context.Courses.SingleOrDefaultAsync(c => c.Id == id);

            if (courseToArchive is null)
            {
                throw new Exception($"Hittade ingen kurs med id: {id}");
            }

            courseToArchive.IsDeprecated = true;
            _context.Courses.Update(courseToArchive);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}