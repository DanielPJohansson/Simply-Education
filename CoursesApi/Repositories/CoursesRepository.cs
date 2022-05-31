using CoursesApi.Data;
using CoursesApi.Models;

namespace CoursesApi.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly CoursesContext _context;
        public CoursesRepository(CoursesContext context)
        {
            _context = context;
        }

        public async Task<CourseViewModel?> GetCourseAsync(int id)
        {
            return await _context.Courses.Where(c => c.Id == id)
            .Include(c => c.Category)
            .Include(c => c.Teachers)
            .Select(c => new CourseViewModel
            {
                CourseId = c.Id,
                CourseCode = c.CourseCode,
                Name = c.Name,
                DurationInHours = c.DurationInHours,
                Category = c.Category.Name,
                Description = c.Description,
                Details = c.Details
            })
            .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CourseViewModel>> GetCoursesAsync()
        {
            return await _context.Courses.Where(c => c.IsDeprecated == false)
            .Include(c => c.Category)
            .Include(c => c.Teachers)
            .Select(c => new CourseViewModel
            {
                CourseId = c.Id,
                CourseCode = c.CourseCode,
                Name = c.Name,
                DurationInHours = c.DurationInHours,
                Category = c.Category.Name,
                Description = c.Description,
                Details = c.Details
            }).ToListAsync();
        }

        public async Task<CourseWithStudentsAndTeachersViewModel?> GetCourseWithStudentsAndTeachersAsync(int courseId)
        {
            return await _context.Courses.Where(c => c.Id == courseId)
            .Include(c => c.Teachers)
            .Include(c => c.StudentCourses)
            .ThenInclude(sc => sc.Student)
            .Select(c => new CourseWithStudentsAndTeachersViewModel
            {
                CourseId = c.Id,
                CourseCode = c.CourseCode,
                Students = c.StudentCourses.Where(sc => sc.IsActive == true)
                .Select(sc => new StudentViewModel
                {
                    Id = sc.Student!.Id,
                    FirstName = sc.Student.FirstName,
                    LastName = sc.Student.LastName,
                    Email = sc.Student.Email
                }).ToList(),
                Teachers = c.Teachers.Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email
                }).ToList()
            }).SingleOrDefaultAsync();
        }


        public async Task AddCourseAsync(PostCourseViewModel model)
        {
            if (await _context.Courses.AnyAsync(c => c.CourseCode == model.CourseCode))
            {
                throw new Exception($"Course with course code {model.CourseCode} already exists.");
            }

            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == model.Category!.ToLower());

            if (category is null)
            {
                throw new Exception($"The category {model.Category!.ToLower()} already exists.");
            }

            var courseToAdd = new Course
            {
                CourseCode = model.CourseCode,
                Name = model.Name,
                DurationInHours = model.DurationInHours,
                Description = model.Description,
                Details = model.Details,
                Category = category
            };

            await _context.Courses.AddAsync(courseToAdd);
        }

        public async Task AddTeacherToCourseAsync(PostTeacherToCourseViewModel model)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == model.CourseId);
            if (course is null)
            {
                throw new BadHttpRequestException($"Could not find course with Id: {model.CourseId}", 404);
            }

            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == model.TeacherId);
            if (teacher is null)
            {
                throw new BadHttpRequestException($"Could not find teacher with Id: {model.TeacherId}", 404);
            }
            if (course.Teachers.Any(t => t.Id == model.TeacherId))
            {
                throw new BadHttpRequestException($"Teacher with id {model.TeacherId} is already assigned to course.", 400);
            }

            course.Teachers.Add(teacher);
            _context.Courses.Update(course);
        }

        public async Task AddStudentToCourseAsync(PostStudentCourseViewModel model)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == model.CourseId);
            if (course is null)
            {
                throw new BadHttpRequestException($"Could not find course with Id: {model.CourseId}", 404);
            }

            var student = await _context.Students.SingleOrDefaultAsync(t => t.Id == model.StudentId);
            if (student is null)
            {
                throw new BadHttpRequestException($"Could not find teacher with Id: {model.StudentId}", 404);
            }

            if (await _context.StudentCourses.AnyAsync(sc => sc.StudentId == model.StudentId && sc.CourseId == model.CourseId))
            {
                throw new BadHttpRequestException($"Student with id {model.StudentId} is already assigned to course with Id: {model.CourseId}.", 400);
            }

            var studentCourse = new CourseStudent
            {
                Student = student,
                Course = course,
                IsActive = true
            };

            await _context.StudentCourses.AddAsync(studentCourse);
        }

        public async Task UpdateCourseAsync(int id, PostCourseViewModel model)
        {
            var courseToUpdate = await _context.Courses.SingleOrDefaultAsync(c => c.Id == id);

            if (courseToUpdate is null)
            {
                throw new Exception($"Could not find course with Id: {id}");
            }

            var category = await _context.Categories.SingleOrDefaultAsync(cat => cat.Name.ToLower() == model.Category!.ToLower());

            if (category is null)
            {
                throw new Exception($"The category {model.Category!.ToLower()} already exists.");
            }

            courseToUpdate.CourseCode = model.CourseCode;
            courseToUpdate.Name = model.Name;
            courseToUpdate.DurationInHours = model.DurationInHours;
            courseToUpdate.Description = model.Description;
            courseToUpdate.Details = model.Details;
            courseToUpdate.Category = category;

            _context.Courses.Update(courseToUpdate);
        }

        public async Task ArchiveCourseAsync(int id)
        {
            var courseToArchive = await _context.Courses.SingleOrDefaultAsync(c => c.Id == id);

            if (courseToArchive is null)
            {
                throw new Exception($"Could not find course with Id: {id}");
            }

            courseToArchive.IsDeprecated = true;
            _context.Courses.Update(courseToArchive);
        }

        public async Task RemoveTeacherFromCourseAsync(int courseId, int teacherId)
        {
            var course = await _context.Courses.Include(c => c.Teachers).SingleOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
            {
                throw new BadHttpRequestException($"Could not find course with Id: {courseId}", 404);
            }

            var teacher = await _context.Teachers.SingleOrDefaultAsync(t => t.Id == teacherId);
            if (teacher is null)
            {
                throw new BadHttpRequestException($"Could not find teacher with Id: {teacherId}", 404);
            }

            if (course.Teachers.Any(t => t.Id == teacherId))
            {
                course.Teachers.Remove(teacher);
                _context.Courses.Update(course);
            }
            else
            {
                throw new BadHttpRequestException($"Teacher with Id {teacherId} is not assigned to course with Id {courseId}", 404);
            }
        }

        public async Task RemoveStudentFromCourseAsync(int courseId, int studentId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
            {
                throw new BadHttpRequestException($"Could not find course with Id: {courseId}", 404);
            }

            var student = await _context.Students.SingleOrDefaultAsync(t => t.Id == studentId);
            if (student is null)
            {
                throw new BadHttpRequestException($"Could not find teacher with Id: {studentId}", 404);
            }

            var studentCourseToRemove = await _context.StudentCourses.SingleOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId && sc.IsActive == true);

            if (studentCourseToRemove is null)
            {
                throw new BadHttpRequestException($"Student with id {studentId} is not active in course with Id: {courseId}.", 400);
            }

            studentCourseToRemove.IsActive = false;

            _context.StudentCourses.Update(studentCourseToRemove);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}