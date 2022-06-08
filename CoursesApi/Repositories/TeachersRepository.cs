using System.Security.Claims;
using System.Text;
using CoursesApi.Data;
using CoursesApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CoursesApi.Repositories
{
    public class TeachersRepository : ITeachersRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TeachersRepository(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<TeacherViewModel>> GetTeachersAsync()
        {
            return await _context.Teachers
            .Include(t => t.User)
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.User!.FirstName,
                LastName = t.User.LastName,
                Street = t.User.Street,
                ZipCode = t.User.ZipCode,
                City = t.User.City,
                Email = t.User.Email,
                PhoneNumber = t.User.PhoneNumber,
                Competences = t.Competences.Select(c => c.Name).ToList()
            }).ToListAsync();
        }

        public async Task<IEnumerable<TeacherViewModel>?> GetTeachersAsync(string categoryName)
        {
            var teachers = await _context.Teachers
            .Include(t => t.User)
            .Include(t => t.Competences)
            .Where(t => t.Competences.Any(c => c.Name.ToLower().Contains(categoryName.ToLower())))
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.User!.FirstName,
                LastName = t.User.LastName,
                Street = t.User.Street,
                ZipCode = t.User.ZipCode,
                City = t.User.City,
                Email = t.User.Email,
                PhoneNumber = t.User.PhoneNumber
            })
            .ToListAsync();

            return teachers;
        }

        public async Task<TeacherViewModel?> GetTeacherAsync(int id)
        {
            return await _context.Teachers.Where(t => t.Id == id)
            .Include(t => t.User)
            .Include(t => t.Competences)
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.User!.FirstName,
                LastName = t.User.LastName,
                Street = t.User.Street,
                ZipCode = t.User.ZipCode,
                City = t.User.City,
                Email = t.User.Email,
                PhoneNumber = t.User.PhoneNumber,
                Competences = t.Competences.Select(c => c.Name).ToList()
            }).SingleOrDefaultAsync();
        }



        public async Task AddTeacherAsync(PostTeacherViewModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                throw new Exception($"Email address {model.Email} is already assigned to a user.");
            }

            var competences = new List<Category>();
            foreach (var competence in model.Competences)
            {

                var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == competence.ToLower());

                if (category is null)
                {
                    throw new Exception($"The competence {competence.ToLower()} does not exist in the database.");
                }

                competences.Add(category);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Street = model.Street,
                ZipCode = model.ZipCode,
                City = model.City,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                UserName = model.Email
            };

            var teacherToAdd = new Teacher
            {
                User = user,
                Competences = competences
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(user, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("Teacher", "true")
                });

                await _context.Teachers.AddAsync(teacherToAdd);
            }
            else
            {
                var errorSummery = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    errorSummery.Append(error.Description);
                    errorSummery.Append(' ');
                }
                throw new Exception(errorSummery.ToString());
            }
        }


        public async Task UpdateTeacherAsync(int id, UpdateTeacherViewModel model)
        {
            var teacherToUpdate = _context.Teachers
            .Include(s => s.User)
            .Include(t => t.Competences)
            .SingleOrDefault(t => t.Id == id);

            if (teacherToUpdate is null)
            {
                throw new Exception($"Could not find teacher with id: {id}");
            }

            var competences = new List<Category>();

            foreach (var competence in model.Competences)
            {
                var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == competence.ToLower());

                if (category is null)
                {
                    throw new Exception($"The competence {competence.ToLower()} does not exist in the database.");
                }

                competences.Add(category);
            }

            var user = teacherToUpdate.User;

            user!.Street = model.Street;
            user.ZipCode = model.ZipCode;
            user.City = model.City;
            user.PhoneNumber = model.PhoneNumber;

            teacherToUpdate.Competences = competences;

            _context.People.Update(user);
            _context.Teachers.Update(teacherToUpdate);
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacherToDelete = await _context.Teachers.Include(s => s.User).SingleOrDefaultAsync(t => t.Id == id);
            if (teacherToDelete is null)
            {
                throw new Exception($"Could not find teacher with id: {id}");
            }

            _context.Teachers.Remove(teacherToDelete);
            _context.People.Remove(teacherToDelete.User!);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}