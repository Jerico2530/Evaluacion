using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model
{
    [Table("Course")]
    public class Course
    {

        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
		public bool State { get; set; }


		public DateTime RegistrationDate { get; set; } = DateTime.Now;

    }
}
