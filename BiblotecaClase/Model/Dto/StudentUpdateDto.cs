using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model.Dto
{
    [Table("Student")]
    public class StudentUpdateDto
    {
   
            [Key]
           
            public int StudentId { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [Required]
            [StringLength(100)]
            public string LastName { get; set; }

        [Required]
        [Range(3, 17, ErrorMessage = "La Age debe estar entre 3 y 17 años.")]
        public int Age { get; set; }

		public bool State { get; set; }




	}
}
