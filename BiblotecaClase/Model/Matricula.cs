using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model
{
    [Table("Matricula")]
    public class Matricula
    {
        [Key]
        public int MatriculaId { get; set; }

        [ForeignKey("Estudiante")]
        public int EstudianteId { get; set; }
        public Estudiante Estudiante { get; set; }

        [ForeignKey("Curso")]
        public int CursoId { get; set; }
        public Curso Curso { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
