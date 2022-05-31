namespace CoursesApi.Interfaces
{
    public interface ICategoriesRepository
    {
        public Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();
        public Task<CategoryViewModel?> GetCategoryAsync(int id);
        public Task<CategoryWithCoursesViewModel?> GetCoursesInCategoryAsync(int id);
        public Task<CategoryWithTeachersViewModel?> GetTeachersInCategoryAsync(int id);
        public Task AddCategoryAsync(PostCategoryViewModel model);
        public Task UpdateCategoryAsync(int id, PostCategoryViewModel model);
        public Task DeleteCategoryAsync(int id);
        public Task<bool> SaveChangesAsync();
    }
}