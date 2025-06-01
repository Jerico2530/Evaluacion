using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repository.IRepository;

namespace Evaluacion.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly BackendContext _db;

        public CourseRepository(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Course> ActualizarCourse(Course entidad)
        {
            _db.Courses.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
