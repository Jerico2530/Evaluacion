using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model.Dto
{
    public class CursoUpdateDto
    {
        [Key]
        public int CursoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}
