using System.Security.Claims;
using System.Text;
using CoursesApi.Data;
using CoursesApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CoursesApi.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<Person> _userManager;
        public StudentsRepository(DataContext context, UserManager<Person> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<StudentViewModel>> GetStudentsAsync()
        {
            return await _context.Students.Include(s => s.Person).Select(s => new StudentViewModel
            {
                Id = s.Id,
                FirstName = s.Person!.FirstName,
                LastName = s.Person.LastName,
                Street = s.Person.Street,
                ZipCode = s.Person.ZipCode,
                City = s.Person.City,
                Email = s.Person.Email,
                PhoneNumber = s.Person.PhoneNumber
            }).ToListAsync();
        }

        public async Task<StudentViewModel?> GetStudentAsync(int id)
        {
            return await _context.Students.Where(s => s.Id == id)
            .Include(s => s.Person)
            .Select(s => new StudentViewModel
            {
                Id = s.Id,
                FirstName = s.Person!.FirstName,
                LastName = s.Person.LastName,
                Street = s.Person.Street,
                ZipCode = s.Person.ZipCode,
                City = s.Person.City,
                Email = s.Person.Email,
                PhoneNumber = s.Person.PhoneNumber
            }).SingleOrDefaultAsync();
        }

        public async Task AddStudentAsync(PostStudentViewModel model)
        {
            if (await _context.Students.Include(s => s.Person).AnyAsync(s => s.Person!.Email == model.Email))
            {
                throw new Exception($"A student with email address {model.Email} already exists.");
            }
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                throw new Exception($"Email address {model.Email} is already assigned to a user.");
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
                UserName = model.Email,
            };

            var studentToAdd = new Student
            {
                Person = person,
                EnrollmentDate = DateTime.Today
            };

            var result = await _userManager.CreateAsync(person, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(person, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, person.Email!),
                    new Claim("Student", "true")
                });

                await _context.Students.AddAsync(studentToAdd);
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

        public async Task UpdateStudentAsync(int id, UpdateStudentViewModel model)
        {
            var studentToUpdate = await _context.Students.Include(s => s.Person).SingleOrDefaultAsync(s => s.Id == id);
            if (studentToUpdate is null)
            {
                throw new Exception($"Could not find student with id: {id}");
            }

            var person = studentToUpdate.Person;

            person!.Street = model.Street;
            person.ZipCode = model.ZipCode;
            person.City = model.City;
            person.PhoneNumber = model.PhoneNumber;

            _context.People.Update(person);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var studentToDelete = await _context.Students.Include(s => s.Person).SingleOrDefaultAsync(s => s.Id == id);
            if (studentToDelete is null)
            {
                throw new Exception($"Could not find student with id: {id}");
            }

            _context.Students.Remove(studentToDelete);
            _context.People.Remove(studentToDelete.Person!);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}