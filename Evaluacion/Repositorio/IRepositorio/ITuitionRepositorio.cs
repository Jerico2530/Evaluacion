using BiblotecaClase.Model;

namespace Evaluacion.Repository.IRepository
{
    public interface ITuitionRepository : IRepository<Tuition>
    {
        Task<Tuition> ActualizarTuition(Tuition entidad);
        Task<(int resultCode, string message)> CreateMatriculaAsync(int studentId, int courseId, int stateId);
        Task<(int resultCode, string message)> ActualizarEstadoMatriculaAsync(int tuitionId, int stateId);
        Task<(int resultCode, string message)> EliminarMatriculaAsync(int tuitionId);


    }
}
