using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model.Dto
{
    public class EstudianteCreateDto
    {

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [Range(3, 17, ErrorMessage = "La edad debe estar entre 3 y 17 años.")]
        public int Edad { get; set; }

        [Required]
        [StringLength(1)]
        [RegularExpression("^[MF]$", ErrorMessage = "El sexo debe ser 'M' o 'F'.")]
        public string Sexo { get; set; }


    }
}
