using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model.Dto
{
    public class TuitionDto
    {
        [Key]
        public int TuitionId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

		[ForeignKey("StateTuition")]
		public int StateId { get; set; }

		public bool State { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
