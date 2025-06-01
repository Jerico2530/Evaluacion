using BiblotecaClase.Model;

namespace Evaluacion.Repository.IRepository
{
    public interface IStateTuitionRepository : IRepository<StateTuition>
    {
        Task<StateTuition> ActualizarStateTuition(StateTuition entidad);
    }
}
