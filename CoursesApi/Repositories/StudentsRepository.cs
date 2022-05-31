using CoursesApi.Data;
using CoursesApi.Models;

namespace CoursesApi.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly CoursesContext _context;
        public StudentsRepository(CoursesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentViewModel>> GetStudentsAsync()
        {
            return await _context.Students.Include(s => s.Address).Select(s => new StudentViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Address = $"{s.Address!.Street}, {s.Address.ZipCode} {s.Address.City}",
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            }).ToListAsync();
        }

        public async Task<StudentViewModel?> GetStudentAsync(int id)
        {
            return await _context.Students.Include(s => s.Address).Where(s => s.Id == id).Select(s => new StudentViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Address = $"{s.Address!.Street}, {s.Address.ZipCode} {s.Address.City}",
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            }).SingleOrDefaultAsync();
        }

        public async Task AddStudentAsync(PostStudentViewModel model)
        {
            if (await _context.Students.AnyAsync(s => s.Email == model.Email))
            {
                throw new Exception($"A student with email address {model.Email} already exists.");
            }

            var studentToAdd = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = new Address
                {
                    Street = model.City,
                    ZipCode = model.ZipCode,
                    City = model.City
                },
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            await _context.Students.AddAsync(studentToAdd);
        }

        public async Task UpdateStudentAsync(int id, PostStudentViewModel model)
        {
            var studentToUpdate = await _context.Students.Include(s => s.Address).SingleOrDefaultAsync(s => s.Id == id);
            if (studentToUpdate is null)
            {
                throw new Exception($"Could not find student with id: {id}");
            }

            studentToUpdate.FirstName = model.FirstName;
            studentToUpdate.LastName = model.LastName;
            studentToUpdate.Address = new Address
            {
                Street = model.City,
                ZipCode = model.ZipCode,
                City = model.City
            };
            studentToUpdate.Email = model.Email;
            studentToUpdate.PhoneNumber = model.PhoneNumber;

            _context.Students.Update(studentToUpdate);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var studentToDelete = await _context.Students.SingleOrDefaultAsync(s => s.Id == id);
            if (studentToDelete is null)
            {
                throw new Exception($"Could not find student with id: {id}");
            }

            _context.Students.Remove(studentToDelete);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}