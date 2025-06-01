using BiblotecaClase.Model;

namespace Evaluacion.Repository.IRepository
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> ActualizarCourse(Course entidad);
    }
}
