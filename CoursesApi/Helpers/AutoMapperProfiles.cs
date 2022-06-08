using AutoMapper;
using CoursesApi.Models;

namespace CoursesApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PostStudentViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.Email));
            CreateMap<UpdateStudentViewModel, ApplicationUser>();

            CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.FirstName, options => options.MapFrom(src => src.User!.FirstName))
            .ForMember(dest => dest.LastName, options => options.MapFrom(src => src.User!.LastName))
            .ForMember(dest => dest.Street, options => options.MapFrom(src => src.User!.Street))
            .ForMember(dest => dest.ZipCode, options => options.MapFrom(src => src.User!.ZipCode))
            .ForMember(dest => dest.City, options => options.MapFrom(src => src.User!.City))
            .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(src => src.User!.PhoneNumber))
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.User!.Email));

            CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.CourseId, options => options.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, options => options.MapFrom(src => src.Category.Name));

            CreateMap<PostCourseViewModel, Course>()
            .ForMember(dest => dest.Category, options => options.Ignore());
            CreateMap<UpdateCourseViewModel, Course>()
            .ForMember(dest => dest.Category, options => options.Ignore());

            CreateMap<Teacher, TeacherViewModel>()
            .ForMember(dest => dest.FirstName, options => options.MapFrom(src => src.User!.FirstName))
            .ForMember(dest => dest.LastName, options => options.MapFrom(src => src.User!.LastName))
            .ForMember(dest => dest.Street, options => options.MapFrom(src => src.User!.Street))
            .ForMember(dest => dest.ZipCode, options => options.MapFrom(src => src.User!.ZipCode))
            .ForMember(dest => dest.City, options => options.MapFrom(src => src.User!.City))
            .ForMember(dest => dest.PhoneNumber, options => options.MapFrom(src => src.User!.PhoneNumber))
            .ForMember(dest => dest.Email, options => options.MapFrom(src => src.User!.Email))
            .ForMember(dest => dest.Competences, options => options.MapFrom(src => src.Competences.Select(c => c.Name).ToList()));

            CreateMap<PostTeacherViewModel, ApplicationUser>()
            .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.Email));
            CreateMap<UpdateTeacherViewModel, ApplicationUser>();

        }
    }
}