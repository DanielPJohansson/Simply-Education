using CoursesApi.Data;
using CoursesApi.Interfaces;
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
            var response = await _context.Courses.Where(c => c.Id == id)
            .Include(c => c.Category)
            .Include(c => c.Teachers)
            .Select(c => new CourseViewModel
            {
                CourseId = c.Id
            })
            .SingleOrDefaultAsync();
            return response;
        }

        public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync()
        {
            return new List<CourseViewModel>();
        }

        public async Task AddCourseAsync(PostCourseViewModel model)
        {

        }

        public async Task UpdateCourseAsync(int id, PostCourseViewModel model)
        {

        }

        public async Task DeleteCourseAsync(int id)
        {

        }

        public async Task<bool> SaveChangesAsync(int id)
        {
            return false;
        }
    }
}