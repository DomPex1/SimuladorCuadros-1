using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMarcos.Entidades
{
    public class Paquetes
    {
        public int ID { get; set; }
        public List<Cuadros> Cuadros { get; set; } = new List<Cuadros>();
    }
}
