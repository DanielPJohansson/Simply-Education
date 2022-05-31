namespace CoursesApi.Interfaces
{
    public interface IStudentsRepository
    {
        public Task<IEnumerable<StudentViewModel>> GetStudentsAsync();
        public Task<StudentViewModel?> GetStudentAsync(int id);
        public Task AddStudentAsync(PostStudentViewModel model);
        public Task UpdateStudentAsync(int id, PostStudentViewModel model);
        public Task DeleteStudentAsync(int id);
        public Task<bool> SaveChangesAsync();
    }
}