using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblotecaClase.Model
{
    [Table("StateTuition")]
    public class StateTuition
    {
        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool State { get; set; }


        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }
}
