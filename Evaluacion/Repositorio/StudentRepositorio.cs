using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repository.IRepository;

namespace Evaluacion.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly BackendContext _db;

        public StudentRepository(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Student> ActualizarStudent(Student entidad)
        {
            _db.Students.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
