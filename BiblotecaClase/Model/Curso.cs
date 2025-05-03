using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model
{
    [Table("Curso")]
    public class Curso
    {

        [Key]
        public int CursoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

    }
}
