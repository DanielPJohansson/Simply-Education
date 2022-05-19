using CoursesApi.ViewModels.Course;

namespace CoursesApi.Interfaces
{
    public interface ICoursesRepository
    {
        public Task<CourseViewModel?> GetCourseAsync(int id);

        public Task<IEnumerable<CourseViewModel>> GetCoursesAsync();

        public Task AddCourseAsync(PostCourseViewModel model);

        public Task UpdateCourseAsync(int id, PostCourseViewModel model);

        public Task DeleteCourseAsync(int id);

        public Task<bool> SaveChangesAsync(int id);

    }
}