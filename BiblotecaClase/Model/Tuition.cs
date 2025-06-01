using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model
{
    [Table("Tuition")]
    public class Tuition
    {
        [Key]
        public int TuitionId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

		[ForeignKey("StateTuition")]
		public int StateId { get; set; }
        public StateTuition StateTuition { get; set; }

		public bool State { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

    }
}
