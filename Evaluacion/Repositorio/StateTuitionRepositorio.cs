using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repository.IRepository;

namespace Evaluacion.Repository
{
    public class StateTuitionRepository : Repository<StateTuition>, IStateTuitionRepository
    {
        private readonly BackendContext _db;

        public StateTuitionRepository(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<StateTuition> ActualizarStateTuition(StateTuition entidad)
        {
            _db.StateTuitions.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}

