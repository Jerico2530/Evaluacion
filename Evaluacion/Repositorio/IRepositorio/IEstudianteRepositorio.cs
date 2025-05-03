using BiblotecaClase.Model;

namespace Evaluacion.Repositorio.IRepositorio
{
    public interface IEstudianteRepositorio : IRepositorio<Estudiante>
    {
        Task<Estudiante> ActualizarEstudiante(Estudiante entidad);
    }
}
