namespace CoursesApi.Interfaces
{
    public interface ICoursesRepository
    {
        public Task<CourseViewModel?> GetCourseAsync(int id);
        public Task<IEnumerable<CourseViewModel>> GetCoursesAsync();
        public Task<IEnumerable<CategoryViewModel>> GetCategoriesForActiveCoursesAsync();
        public Task<CourseWithStudentsAndTeachersViewModel?> GetCourseWithStudentsAndTeachersAsync(int courseId);
        public Task AddCourseAsync(PostCourseViewModel model);
        public Task AddTeacherToCourseAsync(PostTeacherToCourseViewModel model);
        public Task AddStudentToCourseAsync(PostStudentCourseViewModel model);
        public Task UpdateCourseAsync(int id, UpdateCourseViewModel model);
        public Task ArchiveCourseAsync(int id);
        public Task RemoveTeacherFromCourseAsync(int courseId, int teacherId);
        public Task RemoveStudentFromCourseAsync(int courseId, int studentId);
        public Task<bool> SaveChangesAsync();
    }
}