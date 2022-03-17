using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTec.Models
{
    public class UbicacionesGenerales
    {
        public IEnumerable<Ubicacion> Ubicaciones { get; set; }
    }

    public class Ubicacion
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
    }
}
