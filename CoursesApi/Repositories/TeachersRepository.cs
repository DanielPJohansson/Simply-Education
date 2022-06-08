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
        private readonly UserManager<Person> _userManager;
        public TeachersRepository(DataContext context, UserManager<Person> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<TeacherViewModel>> GetTeachersAsync()
        {
            return await _context.Teachers
            .Include(t => t.Person)
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.Person!.FirstName,
                LastName = t.Person.LastName,
                Street = t.Person.Street,
                ZipCode = t.Person.ZipCode,
                City = t.Person.City,
                Email = t.Person.Email,
                PhoneNumber = t.Person.PhoneNumber,
                Competences = t.Competences.Select(c => c.Name).ToList()
            }).ToListAsync();
        }

        public async Task<IEnumerable<TeacherViewModel>?> GetTeachersAsync(string categoryName)
        {
            var teachers = await _context.Teachers
            .Include(t => t.Person)
            .Include(t => t.Competences)
            .Where(t => t.Competences.Any(c => c.Name.ToLower().Contains(categoryName.ToLower())))
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.Person!.FirstName,
                LastName = t.Person.LastName,
                Street = t.Person.Street,
                ZipCode = t.Person.ZipCode,
                City = t.Person.City,
                Email = t.Person.Email,
                PhoneNumber = t.Person.PhoneNumber
            })
            .ToListAsync();

            return teachers;
        }

        public async Task<TeacherViewModel?> GetTeacherAsync(int id)
        {
            return await _context.Teachers.Where(t => t.Id == id)
            .Include(t => t.Person)
            .Include(t => t.Competences)
            .Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.Person!.FirstName,
                LastName = t.Person.LastName,
                Street = t.Person.Street,
                ZipCode = t.Person.ZipCode,
                City = t.Person.City,
                Email = t.Person.Email,
                PhoneNumber = t.Person.PhoneNumber,
                Competences = t.Competences.Select(c => c.Name).ToList()
            }).SingleOrDefaultAsync();
        }



        public async Task AddTeacherAsync(PostTeacherViewModel model)
        {
            if (await _context.Teachers.Include(t => t.Person).AnyAsync(t => t.Person.Email == model.Email))
            {
                throw new Exception($"A teacher with email address {model.Email} already exists.");
            }
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

            var person = new Person
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
                Person = person,
                Competences = competences
            };

            var result = await _userManager.CreateAsync(person, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(person, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, person.Email!),
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
            .Include(s => s.Person)
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

            var person = teacherToUpdate.Person;

            person!.Street = model.Street;
            person.ZipCode = model.ZipCode;
            person.City = model.City;
            person.PhoneNumber = model.PhoneNumber;

            teacherToUpdate.Competences = competences;

            _context.People.Update(person);
            _context.Teachers.Update(teacherToUpdate);
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacherToDelete = await _context.Teachers.Include(s => s.Person).SingleOrDefaultAsync(t => t.Id == id);
            if (teacherToDelete is null)
            {
                throw new Exception($"Could not find teacher with id: {id}");
            }

            _context.Teachers.Remove(teacherToDelete);
            _context.People.Remove(teacherToDelete.Person!);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}