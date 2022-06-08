using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoursesApi.Data;
using CoursesApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CoursesApi.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public StudentsRepository(DataContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<StudentViewModel>> GetStudentsAsync()
        {
            return await _context.Students.Include(s => s.User).ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<StudentViewModel?> GetStudentAsync(int id)
        {
            return await _context.Students.Where(s => s.Id == id)
            .Include(s => s.User)
            .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task AddStudentAsync(PostStudentViewModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                throw new Exception($"Email address {model.Email} is already assigned to a user.");
            }

            var userToAdd = _mapper.Map<ApplicationUser>(model);

            var studentToAdd = new Student
            {
                User = userToAdd,
                EnrollmentDate = DateTime.Today
            };

            var result = await _userManager.CreateAsync(userToAdd, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimsAsync(userToAdd, new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, userToAdd.Email!),
                    new Claim("Student", "true")
                });
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

            await _context.Students.AddAsync(studentToAdd);
        }

        public async Task CreateStudentForUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new Exception($"Could not find user with email address: {email.ToLower()}");
            }

            var studentToAdd = new Student
            {
                User = user,
                EnrollmentDate = DateTime.Today
            };

            await _context.Students.AddAsync(studentToAdd);
        }

        public async Task UpdateStudentAsync(int id, UpdateStudentViewModel model)
        {
            var studentToUpdate = await _context.Students.Include(s => s.User).SingleOrDefaultAsync(s => s.Id == id);
            if (studentToUpdate is null)
            {
                throw new Exception($"Could not find student with id: {id}");
            }

            var userToUpdate = _mapper.Map<UpdateStudentViewModel, ApplicationUser>(model, studentToUpdate.User!);

            _context.People.Update(userToUpdate);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var studentToDelete = await _context.Students.Include(s => s.User).SingleOrDefaultAsync(s => s.Id == id);
            if (studentToDelete is null)
            {
                throw new Exception($"Could not find student with id: {id}");
            }

            _context.Students.Remove(studentToDelete);
            _context.People.Remove(studentToDelete.User!);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}