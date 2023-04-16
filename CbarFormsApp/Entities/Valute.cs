using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbarFormsApp.Entities
{
    public class Valute
    {
        [Key]
        public Guid Id { get; set; }
        public string? Nominal { get; set; }
        public string? Name { get; set; }
        public double? Value { get; set; }

        [ForeignKey("ValTypeId")]
        public Guid? ValTypeId { get; set; }
    }
}
