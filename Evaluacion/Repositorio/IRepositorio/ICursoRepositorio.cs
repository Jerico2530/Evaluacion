using BiblotecaClase.Model;

namespace Evaluacion.Repositorio.IRepositorio
{
    public interface ICursoRepositorio : IRepositorio<Curso>
    {
        Task<Curso> ActualizarCurso(Curso entidad);
    }
}
