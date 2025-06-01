using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model.Dto
{
    public class TuitionUpdateDto
    {
        [Key]
        public int TuitionId { get; set; }



		[ForeignKey("StateTuition")]
		public int StateId { get; set; }



	}
}
