using BiblotecaClase.Model;

namespace Evaluacion.Repository.IRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> ActualizarStudent(Student entidad);
    }
}
