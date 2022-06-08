using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoursesApi.Data;
using CoursesApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CoursesApi.Repositories
{
    public class TeachersRepository : ITeachersRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public TeachersRepository(DataContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<TeacherViewModel>> GetTeachersAsync()
        {
            return await _context.Teachers
            .Include(t => t.User)
            .ProjectTo<TeacherViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<IEnumerable<TeacherViewModel>?> GetTeachersAsync(string categoryName)
        {
            return await _context.Teachers
            .Include(t => t.User)
            .Include(t => t.Competences)
            .Where(t => t.Competences.Any(c => c.Name.ToLower().Contains(categoryName.ToLower())))
            .ProjectTo<TeacherViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<TeacherViewModel?> GetTeacherAsync(int id)
        {
            return await _context.Teachers.Where(t => t.Id == id)
            .Include(t => t.User)
            .Include(t => t.Competences)
            .ProjectTo<TeacherViewModel>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
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

            var userToAdd = _mapper.Map<ApplicationUser>(model);

            var teacherToAdd = new Teacher
            {
                User = userToAdd,
                Competences = competences
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(userToAdd, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, userToAdd.Email!),
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

            var userToUpdate = _mapper.Map<UpdateTeacherViewModel, ApplicationUser>(model, teacherToUpdate.User!);

            teacherToUpdate.Competences = competences;

            _context.People.Update(userToUpdate);
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