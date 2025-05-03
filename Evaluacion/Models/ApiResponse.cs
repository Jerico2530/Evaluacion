using System.Net;

namespace Evaluacion.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<string> ErroresMessages { get; set; } = new();
        public object Resultado { get; set; }
    }
}
