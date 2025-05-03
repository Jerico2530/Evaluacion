using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model.Dto
{
    public class MatriculaUpdateDto
    {
        [Key]
        public int MatriculaId { get; set; }

        [ForeignKey("Estudiante")]
        public int EstudianteId { get; set; }

        [ForeignKey("Curso")]
        public int CursoId { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

    }
}
