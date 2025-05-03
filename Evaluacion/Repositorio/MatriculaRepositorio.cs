using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repositorio.IRepositorio;

namespace Evaluacion.Repositorio
{
    public class MatriculaRepositorio : Repositorio<Matricula>, IMatriculaRepositorio
    {
        private readonly BackendContext _db;

        public MatriculaRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Matricula> ActualizarMatricula(Matricula entidad)
        {
            _db.Matriculas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
