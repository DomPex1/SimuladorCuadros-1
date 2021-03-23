using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMarcos.Entidades
{
    public class Carpinteros
    {
        public int ID { get; set; }
        public List<Cuadros> CuadrosATrabajar { get; set; } = new List<Cuadros>();

    }
}
