namespace CoursesApi.Interfaces
{
    public interface ITeachersRepository
    {
        public Task<IEnumerable<TeacherViewModel>> GetTeachersAsync();
        public Task<IEnumerable<TeacherViewModel>?> GetTeachersAsync(string categoryName);
        public Task<TeacherViewModel?> GetTeacherAsync(int id);
        public Task AddTeacherAsync(PostTeacherViewModel model);
        public Task UpdateTeacherAsync(int id, UpdateTeacherViewModel model);
        public Task DeleteTeacherAsync(int id);
        public Task<bool> SaveChangesAsync();


    }
}