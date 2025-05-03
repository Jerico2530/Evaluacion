using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repositorio.IRepositorio;

namespace Evaluacion.Repositorio
{
    public class EstudianteRepositorio : Repositorio<Estudiante>, IEstudianteRepositorio
    {
        private readonly BackendContext _db;

        public EstudianteRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Estudiante> ActualizarEstudiante(Estudiante entidad)
        {
            _db.Estudiantes.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
