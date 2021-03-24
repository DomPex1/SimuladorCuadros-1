using SimuladorMarcos.BLL;
using SimuladorMarcos.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorMarcos
{
    public partial class Form1 : Form
    {
        //Tiempo General
        public int Hora { get; set; }
        public int Minutos { get; set; }
        public int Segundos { get; set; }
        //Control de paquetes
        public List<Paquetes> PaquetesEntrantes { get; set; } = new List<Paquetes>();
        public Random RPaquetes { get; set; } = new Random();
        //Control de Ensamblaje
        public List<Cuadros> EsperaEnsamblaje { get; set; } = new List<Cuadros>();//FIFO
        public Random TiempoEns { get; set; } = new Random();

        //Control de carpinteros
        int contadorc1 = 0;
        int contadorc2 = 0;
        int contadorc3 = 0;
        int contadorc4 = 0;
        int contadorc5 = 0;

        int Horac1, minutoc1, segundoc1;
        int Horac2, minutoc2, segundoc2;
        int Horac3, minutoc3, segundoc3;
        int Horac4, minutoc4, segundoc4;
        int Horac5, minutoc5, segundoc5;
        public static List<Carpinteros> Carpinteros { get; set; } = new List<Carpinteros>();
        public int MovX { get; set; } = 111;
        int i = 0;
        int idist = 0;

        //Control de Almacen
        public List<Cuadros> EsperaAlmacen { get; set; } = new List<Cuadros>(); //FIFO
        public Form1()
        {
            InitializeComponent();
        }

        private void TiempoGeneral_Tick(object sender, EventArgs e)
        {
            Segundos++;
            string SHoras = Hora.ToString(), SMinutos = Minutos.ToString(), SSegundos = Segundos.ToString();

            if(Segundos < 10) { SSegundos = "0" + Segundos; }
            if (Minutos < 10) { SMinutos = "0" + Minutos; }
            if (Hora < 10) { SHoras = "0" + Hora; }

            TiempoTb.Text = SHoras + " : " + SMinutos + " : " + SSegundos;

            if(Segundos == 60)
            {
                Minutos++;
                Segundos = 0;
            }
            if(Minutos == 5)
            {
                
                Hora++;
                Minutos = 0;
            }
            if(Hora%5 == 0)
            {
                PaquetePb.Visible = true;
                LlegadaPaquete.Start();
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            TiempoGeneral.Start();
        }

        

        private void LlegadaPaquete_Tick(object sender, EventArgs e)
        {
            MovX++;
            PaquetePb.SetBounds(MovX-72,12,72,47);

            if (MovX == 600) 
            {
                GruposEntrantes();
                PaquetePb.Visible = false;
                MovX = 140;
                LlegadaPaquete.Stop();   
            }
                
        }
        public void GruposEntrantes()
        {
            i++;
            int n = RPaquetes.Next(1, 11);
            int vueltas = 0;
            int aux = 0;

            if (n == 1 || n == 2 || n == 3 || n == 4 || n == 5 || n == 6) { vueltas = 4; } else { vueltas = 6; }

            Paquetes p = new Paquetes();
            for (aux = 0; aux < vueltas; aux++)
            {
                Cuadros c = new Cuadros();
                c.ID = aux;
                p.Cuadros.Add(c);
                p.ID = i+1;
            }
            PaquetesEntrantes.Add(p);
            MarcosTb.Value = p.Cuadros.Count;
            TotalPaquetesNUD.Value = PaquetesEntrantes.Count;
            TotalMarcosNUD.Value += p.Cuadros.Count;
            foreach (var item in p.Cuadros)
            {
                EsperaEnsamblaje.Add(item);
            }
            ProcesoEnsamblaje();
        }
        //Metodos del proceso de ensamblaje
        public void ProcesoEnsamblaje()
        {
            int n = 0; 

            EntraronEnsNUD.Value = EsperaEnsamblaje.Count;
            foreach (var item in EsperaEnsamblaje)
            {
                n = TiempoEns.Next(1, 11);
                if(n == 1 || n == 2) { item.TiempoEnsamblaje = 2; }
                if (n == 3 || n == 4) { item.TiempoEnsamblaje = 3; }
                if (n == 5 || n == 6) { item.TiempoEnsamblaje = 4; }
                if (n == 7 || n == 8) { item.TiempoEnsamblaje = 5; }
                if (n == 9 || n == 10) { item.TiempoEnsamblaje = 6; }
            }
            DistribuirEnsamblaje();
        }
        public void DistribuirEnsamblaje()
        {
            int indice = 0;
            foreach (var item in EsperaEnsamblaje)
            {
                idist++;
                if(idist == 6) { idist = 1; }
                Carpinteros c = new Carpinteros();
                c = CarpinteroBLL.Buscar(idist);
                item.ID = idist;
                c.CuadrosATrabajar.Add(item);
            }
            EsperaEnsamblaje.RemoveRange(0,EsperaEnsamblaje.Count);
            C1CNUD.Value = CarpinteroBLL.Buscar(1).CuadrosATrabajar.Count;
            C2CNUD.Value = CarpinteroBLL.Buscar(2).CuadrosATrabajar.Count;
            C3CNUD.Value = CarpinteroBLL.Buscar(3).CuadrosATrabajar.Count;
            C4CNUD.Value = CarpinteroBLL.Buscar(4).CuadrosATrabajar.Count;
            C5CNUD.Value = CarpinteroBLL.Buscar(5).CuadrosATrabajar.Count;
            Carpintero1.Start();
            Carpintero2.Start();
            //Carpintero3.Start();
            //Carpintero4.Start();
            //Carpintero5.Start();

        }
        public void SalidaDeCuadrosEnsamblados()
        {
            Carpinteros c1 = CarpinteroBLL.Buscar(1);
            foreach (var item in c1.CuadrosATrabajar)
            {

            }
        }
        public void MetodoProcesoAlmacen()
        {
           TerminadosNUD.Value = EsperaAlmacen.Count;

        }
        private void Carpintero1_Tick(object sender, EventArgs e)
        {
            int TIEMPO = 0;
            Cuadros cuadroaux1 = new Cuadros();
            segundoc1++;
            Carpinteros c1 = CarpinteroBLL.Buscar(1);
            if(c1.CuadrosATrabajar.Count != 0)
                 cuadroaux1 = c1.CuadrosATrabajar.First();
            if(cuadroaux1 != null)
                 TIEMPO = cuadroaux1.TiempoEnsamblaje;

            if (Horac1 == TIEMPO)
            {
                Horac1 = 0;
                minutoc1 = 0;
                segundoc1 = 0;
                EsperaAlmacen.Add(cuadroaux1);
                c1.CuadrosATrabajar.Remove(cuadroaux1);
                contadorc1++;
            }


            if (segundoc1 == 60)
            {
                minutoc1++;
                segundoc1 = 0;
            }
            if(minutoc1 == 5)
            {
                Horac1++;
                minutoc1 = 0;
            }
            C1CNUD.Value = CarpinteroBLL.Buscar(1).CuadrosATrabajar.Count;
        }

        //Fin de Metodos del proceso de ensamblaje     
        
        private void Carpintero2_Tick(object sender, EventArgs e)
        {
            int TIEMPO = 0;
            Cuadros cuadroaux2 = new Cuadros();
            segundoc2++;
            Carpinteros c2 = CarpinteroBLL.Buscar(2);
            if (c2.CuadrosATrabajar.Count != 0)
                cuadroaux2 = c2.CuadrosATrabajar.First();
            if(cuadroaux2 != null)
                TIEMPO = cuadroaux2.TiempoEnsamblaje;

            if (Horac2 == TIEMPO)
            {
                Horac2 = 0;
                minutoc2 = 0;
                segundoc2 = 0;
                EsperaAlmacen.Add(cuadroaux2);
                c2.CuadrosATrabajar.Remove(cuadroaux2);
                contadorc2++;
            }


            if (segundoc2 == 60)
            {
                minutoc2++;
                segundoc2 = 0;
            }
            if (minutoc2 == 5)
            {
                Horac2++;
                minutoc2 = 0;
            }
            C2CNUD.Value = CarpinteroBLL.Buscar(2).CuadrosATrabajar.Count;
        }

        private void Carpintero3_Tick(object sender, EventArgs e)
        {
            segundoc3++;
            Carpinteros c3 = CarpinteroBLL.Buscar(3);
            int TIEMPO = c3.CuadrosATrabajar.Find(c => c.ID == 0).TiempoEnsamblaje;
            if (Horac3 == TIEMPO)
            {
                Horac3 = 0;
                minutoc3 = 0;
                segundoc3 = 0;
                EsperaAlmacen.Add(c3.CuadrosATrabajar.Find(c => c.ID != 0));
                c3.CuadrosATrabajar.Remove(c3.CuadrosATrabajar.Find(c => c.ID != 0));
                contadorc3++;
            }


            if (segundoc3 == 60)
            {
                minutoc3++;
                segundoc3 = 0;
            }
            if (minutoc3 == 5)
            {
                Horac3++;
                minutoc3 = 0;
            }
            C3CNUD.Value = CarpinteroBLL.Buscar(3).CuadrosATrabajar.Count;
            TerminadosNUD.Value = EsperaAlmacen.Count;
        }

        private void Carpintero4_Tick(object sender, EventArgs e)
        {
            segundoc4++;
            Carpinteros c4 = CarpinteroBLL.Buscar(4);
            int TIEMPO = c4.CuadrosATrabajar.Find(c => c.ID == 0).TiempoEnsamblaje;
            if (Horac4 == TIEMPO)
            {
                Horac4 = 0;
                minutoc4 = 0;
                segundoc4 = 0;
                EsperaAlmacen.Add(c4.CuadrosATrabajar.Find(c => c.ID != 0));
                c4.CuadrosATrabajar.Remove(c4.CuadrosATrabajar.Find(c => c.ID != 0));
                contadorc4++;
            }


            if (segundoc4 == 60)
            {
                minutoc4++;
                segundoc4 = 0;
            }
            if (minutoc4 == 5)
            {
                Horac4++;
                minutoc4 = 0;
            }
            C4CNUD.Value = CarpinteroBLL.Buscar(4).CuadrosATrabajar.Count;
            TerminadosNUD.Value = EsperaAlmacen.Count;
        }

        private void Carpintero5_Tick(object sender, EventArgs e)
        {
            segundoc5++;
            Carpinteros c5 = CarpinteroBLL.Buscar(5);
            int TIEMPO= c5.CuadrosATrabajar.Find(c => c.ID == 0).TiempoEnsamblaje;
            if (Horac5 == TIEMPO)
            {
                Horac5 = 0;
                minutoc5 = 0;
                segundoc5 = 0;
                EsperaAlmacen.Add(c5.CuadrosATrabajar.Find(c => c.ID != 0));
                c5.CuadrosATrabajar.Remove(c5.CuadrosATrabajar.Find(c => c.ID != 0));
                contadorc5++;
            }


            if (segundoc5 == 60)
            {
                minutoc5++;
                segundoc5 = 0;
            }
            if (minutoc5 == 5)
            {
                Horac5++;
                minutoc5 = 0;
            }
            C5CNUD.Value = CarpinteroBLL.Buscar(5).CuadrosATrabajar.Count;
            TerminadosNUD.Value = EsperaAlmacen.Count;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                Entidades.Carpinteros c = new Carpinteros();
                c.ID = i + 1;
                Carpinteros.Add(c);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TiempoGeneral.Stop();
        }


    }
}
