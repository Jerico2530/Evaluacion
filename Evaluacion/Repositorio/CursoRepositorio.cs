using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repositorio.IRepositorio;

namespace Evaluacion.Repositorio
{
    public class CursoRepositorio : Repositorio<Curso>, ICursoRepositorio
    {
        private readonly BackendContext _db;

        public CursoRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Curso> ActualizarCurso(Curso entidad)
        {
            _db.Cursos.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
