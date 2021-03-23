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
        public List<Carpinteros> Carpinteros { get; set; } = new List<Carpinteros>();
        public int MovX { get; set; } = 111;
        int i = 0;

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
        }
        public void DistribuirEnsamblaje()
        {
            foreach (var item in EsperaEnsamblaje)
            {
                
            }
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
