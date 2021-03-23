using SimuladorMarcos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimuladorMarcos.BLL
{
    public class CarpinteroBLL
    {
        public static Carpinteros Buscar(int id)
        {
            Carpinteros a = new Carpinteros();
            try
            {
                foreach (var item in Form1.Carpinteros)
                {
                    if (item.ID == id)
                    {
                        a = item;
                    }
                }
                return a;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<Carpinteros> GetCarpinteros()
        {
            List<Carpinteros> l = new List<Carpinteros>();

            l = Form1.Carpinteros;

            return l;
        }
    }
}