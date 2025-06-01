using AutoMapper;
using BiblotecaClase.Model;
using BiblotecaClase.Model.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Evaluacion.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Tuition, TuitionCreateDto>().ReverseMap();
            CreateMap<Tuition, TuitionDto>().ReverseMap();
            CreateMap<Tuition, TuitionUpdateDto>().ReverseMap();



            CreateMap<Student, StudentCreateDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, StudentUpdateDto>().ReverseMap();


            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CourseCreateDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();

            CreateMap<StateTuition, StateTuitionDto>().ReverseMap();
            CreateMap<StateTuition, StateTuitionCreateDto>().ReverseMap();
            CreateMap<StateTuition, StateTuitionUpdateDto>().ReverseMap();


        }
    }
}
