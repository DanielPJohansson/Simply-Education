using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Data;
using CoursesApi.Interfaces;
using CoursesApi.Models;
using CoursesApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly CoursesContext _context;
        public CategoriesRepository(CoursesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            return await _context.Categories.Select(cat => new CategoryViewModel
            {
                CategoryId = cat.Id,
                Name = cat.Name
            }).ToListAsync();
        }
        public async Task<CategoryWithCoursesViewModel?> GetCoursesInCategoryAsync(int id)
        {
            var courses = await _context.Categories.Where(cat => cat.Id == id).Include(cat => cat.Courses).Select(cat => new CategoryWithCoursesViewModel
            {
                CategoryId = cat.Id,
                CategoryName = cat.Name,
                Courses = cat.Courses.Select(c => new CourseViewModel
                {
                    CourseId = c.Id,
                    CourseCode = c.CourseCode,
                    Name = c.Name,
                    Duration = c.Duration,
                    Description = c.Description,
                    Details = c.Details
                }).ToList()
            }).SingleOrDefaultAsync();
            return courses;
        }

        public async Task<CategoryViewModel?> GetCategoryAsync(int id)
        {
            return await _context.Categories.Where(cat => cat.Id == id)
            .Select(cat => new CategoryViewModel
            {
                CategoryId = cat.Id,
                Name = cat.Name
            }).SingleOrDefaultAsync();
        }

        public async Task AddCategoryAsync(PostCategoryViewModel model)
        {
            if (await _context.Categories.AnyAsync(cat => cat.Name.ToLower() == model.Name!.ToLower()))
            {
                throw new Exception(message: $"Kategorin {model.Name} finns redan i databasen.");
            }

            var category = new Category
            {
                Name = model.Name!
            };

            await _context.Categories.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(int id, PostCategoryViewModel model)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Id == id);

            if (category is null)
            {
                throw new Exception($"Det finns ingen kategori med id: {id}");
            }

            category.Name = model.Name!;
            _context.Categories.Update(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Id == id);

            if (category is null)
            {
                throw new Exception($"Det finns ingen kategori med id: {id}");
            }

            _context.Categories.Remove(category);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}