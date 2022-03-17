using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTec.Models
{
    public class NombreComun
    {
        public string Name { get; set; }
        public IEnumerable<Ciudad> Country { get; set; }
    }

    public class Ciudad
    {
        public string Country_id { get; set; }
        public float Probability { get; set; }
    }
}
