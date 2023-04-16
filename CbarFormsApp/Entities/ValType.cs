using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CbarFormsApp.Entities
{
    public class ValType
    {
        [Key]
        public Guid Id { get; set; }
        public string? NameForTime { get; set; }
        public List<Valute>? Valutes { get; set; }
    }
}
