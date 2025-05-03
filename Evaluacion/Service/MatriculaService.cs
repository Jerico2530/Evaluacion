using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Microsoft.EntityFrameworkCore;

namespace Evaluacion.Service
{
    public class MatriculaService
    {
        private readonly BackendContext _context;

        public MatriculaService(BackendContext context)
        {
            _context = context;
        }

        public async Task CrearMatriculaAsync(int estudianteId, int cursoId, string estado)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC SP_CrearMatricula {estudianteId}, {cursoId}, {estado}"
            );
        }

        public async Task CambiarEstadoAsync(int matriculaId, string nuevoEstado)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC SP_ActualizarEstadoMatricula {matriculaId}, {nuevoEstado}"
            );
        }

        public async Task EliminarMatriculaAsync(int matriculaId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC SP_EliminarMatricula {matriculaId}"
            );
        }

        public async Task<Matricula> ObtenerMatriculaPorIdAsync(int id)
        {
            return await _context.Matriculas.FirstOrDefaultAsync(m => m.MatriculaId == id);
        }
    }
}
