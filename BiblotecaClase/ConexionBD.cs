using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BiblotecaClase
{
    public class ConexionBD
    {
        private readonly string conexion;

        public ConexionBD(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
        }

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(conexion);
        }

        public string ObtenerCadenaConexion()
        {
            return conexion;
        }
    }
}

