using CoursesApi.Data;
using CoursesApi.Models;

namespace CoursesApi.Repositories
{
    public class TeachersRepository : ITeachersRepository
    {
        private readonly CoursesContext _context;
        public TeachersRepository(CoursesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherViewModel>> GetTeachersAsync()
        {
            return await _context.Teachers.Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Address = $"{t.Address!.Street}, {t.Address.ZipCode} {t.Address.City}",
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Competences = t.Competences.Select(c => c.Name).ToList()
            }).ToListAsync();
        }

        public async Task<TeacherViewModel?> GetTeacherAsync(int id)
        {
            return await _context.Teachers.Where(t => t.Id == id).Include(t => t.Competences).Select(t => new TeacherViewModel
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Address = $"{t.Address!.Street}, {t.Address.ZipCode} {t.Address.City}",
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Competences = t.Competences.Select(c => c.Name).ToList()
            }).SingleOrDefaultAsync();
        }

        public async Task AddTeacherAsync(PostTeacherViewModel model)
        {
            if (await _context.Teachers.AnyAsync(t => t.Email == model.Email))
            {
                throw new BadHttpRequestException($"A teacher with email address {model.Email} already exists.", 400);
            }

            var competences = new List<Category>();
            foreach (var competence in model.Competences)
            {

                var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == competence.ToLower());

                if (category is null)
                {
                    throw new BadHttpRequestException($"The competence {competence.ToLower()} does not exist in the database.", 404);
                }

                competences.Add(category);
            }

            var teacherToAdd = new Teacher
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
                PhoneNumber = model.PhoneNumber,
                Competences = competences
            };

            await _context.Teachers.AddAsync(teacherToAdd);
        }

        public async Task UpdateTeacherAsync(int id, PostTeacherViewModel model)
        {
            var teacherToUpdate = _context.Teachers.Include(t => t.Competences).SingleOrDefault(t => t.Id == id);
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

            teacherToUpdate.FirstName = model.FirstName;
            teacherToUpdate.LastName = model.LastName;
            teacherToUpdate.Address = new Address
            {
                Street = model.City,
                ZipCode = model.ZipCode,
                City = model.City
            };
            teacherToUpdate.Email = model.Email;
            teacherToUpdate.PhoneNumber = model.PhoneNumber;
            teacherToUpdate.Competences = competences;

            _context.Teachers.Update(teacherToUpdate);
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var teacherToDelete = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == id);
            if (teacherToDelete is null)
            {
                throw new Exception($"Could not find teacher with id: {id}");
            }

            _context.Teachers.Remove(teacherToDelete);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}