using BiblotecaClase.Datos;
using BiblotecaClase.Model;
using Evaluacion.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Evaluacion.Repository
{
    public class TuitionRepository : Repository<Tuition>, ITuitionRepository
    {
        private readonly BackendContext _db;

        public TuitionRepository(BackendContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Tuition> ActualizarTuition(Tuition entidad)
        {
            _db.Tuitions.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }

        public async Task<(int resultCode, string message)> CreateMatriculaAsync(int studentId, int courseId, int stateId)
        {
            var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.NVarChar, 250)
            {
                Direction = ParameterDirection.Output
            };

            // Eliminé el parámetro ReturnCode y la asignación en EXEC
            var resultCode = await _db.Database.ExecuteSqlRawAsync(
                "EXEC spCreateMatricula @StudentId, @CourseId, @StateId, @OutputMessage OUTPUT",
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@CourseId", courseId),
                new SqlParameter("@StateId", stateId),
                outputMessage
            );

            return (resultCode, outputMessage.Value?.ToString());
        }

        public async Task<(int resultCode, string message)> ActualizarEstadoMatriculaAsync(int tuitionId, int stateId)
        {
            var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.NVarChar, 250)
            {
                Direction = ParameterDirection.Output
            };

            // Eliminé @ReturnCode y la asignación en EXEC
            var resultCode = await _db.Database.ExecuteSqlRawAsync(
                "EXEC spActualizarEstadoMatricula @TuitionId, @StateId, @OutputMessage OUTPUT",
                new SqlParameter("@TuitionId", tuitionId),
                new SqlParameter("@StateId", stateId),
                outputMessage
            );

            return (resultCode, outputMessage.Value?.ToString());
        }

        public async Task<(int resultCode, string message)> EliminarMatriculaAsync(int tuitionId)
        {
            using var command = _db.Database.GetDbConnection().CreateCommand();
            command.CommandText = "spEliminarMatricula";
            command.CommandType = CommandType.StoredProcedure;

            var paramTuitionId = new SqlParameter("@TuitionId", tuitionId);
            command.Parameters.Add(paramTuitionId);

            var paramOutput = new SqlParameter("@OutputMessage", SqlDbType.NVarChar, 250)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(paramOutput);

            var paramReturn = new SqlParameter()
            {
                ParameterName = "@ReturnVal",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(paramReturn);

            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            int resultCode = (int)(paramReturn.Value ?? -1);
            string message = paramOutput.Value?.ToString();

            return (resultCode, message);
        }
    }
}
