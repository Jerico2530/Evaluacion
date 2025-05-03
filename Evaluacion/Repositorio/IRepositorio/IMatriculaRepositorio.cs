using BiblotecaClase.Model;

namespace Evaluacion.Repositorio.IRepositorio
{
    public interface IMatriculaRepositorio : IRepositorio<Matricula>
    {
        Task<Matricula> ActualizarMatricula(Matricula entidad);
    
    }
}
