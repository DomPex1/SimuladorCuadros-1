using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMarcos.Entidades
{
    public class Cuadros
    {
        public int ID { get; set; }
        public int Estado { get; set; }
        public int TiempoEnsamblaje { get; set; }
        public int TiempoEnAlmacen { get; set; }
        public int TiempoPintura { get; set; }
        public int TiempoEmpaque { get; set; }

    }
}
