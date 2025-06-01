using System.Net;

namespace Evaluacion.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessfull { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new();
        public object Result { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
