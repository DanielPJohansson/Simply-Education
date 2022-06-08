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
        }
    }
}