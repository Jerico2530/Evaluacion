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
            CreateMap<Matricula, MatriculaCreateDto>().ReverseMap();
            CreateMap<Matricula, MatriculaDto>().ReverseMap();
            CreateMap<Matricula, MatriculaUpdateDto>().ReverseMap();



            CreateMap<Estudiante, EstudianteCreateDto>().ReverseMap();
            CreateMap<Estudiante, EstudianteDto>().ReverseMap();
            CreateMap<Estudiante, EstudianteUpdateDto>().ReverseMap();


            CreateMap<Curso, CursoDto>().ReverseMap();
            CreateMap<Curso, CursoCreateDto>().ReverseMap();
            CreateMap<Curso, CursoUpdateDto>().ReverseMap();


        }
    }
}
