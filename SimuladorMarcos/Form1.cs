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

        int horasAl = 0, minutosAl = 0, SegundosAl = 0;
        public List<Cuadros> EsperaAlmacen { get; set; } = new List<Cuadros>(); //FIFO

        //Control Pintura
        int horasPin = 0, minutosPin = 0, SegundosPin = 0;
        public int ContadorMaquina { get; set; }
        public List<Cuadros> EsperaPintura { get; set; } = new List<Cuadros>();
        public Random Rpintura { get; set; } = new Random();
        public Random MaquinaDamage { get; set; } = new Random();
        public int MinutosMaquinaDañada { get; set; }
        public string Estado { get; set; }

        int HorasMaq = 0, MinutosMaq = 0, SegundosMac = 0;

        //Control Inspeccion
        int horasIns = 0, minutosIns = 0, SegundosIns = 0;
        public List<Cuadros> EsperaInspeccion { get; set; } = new List<Cuadros>();
        public List<Cuadros> Retrabajar { get; set; } = new List<Cuadros>();
        Random paso = new Random();

        //Control de empaque
        int horasEmp = 0, minutosEmp = 0, SegundosEmp = 0;

        public List<Cuadros> EsperaEmpaque { get; set; } = new List<Cuadros>();
        Random Empac = new Random();
        //Lista Terminados
        public List<Cuadros> Terminados { get; set; } = new List<Cuadros>();

        //Listas entraron
        public List<Cuadros> EntraronEnsamblaje { get; set; } = new List<Cuadros>();//
        public List<Cuadros> EntraronAlmacen { get; set; } = new List<Cuadros>();
        public List<Cuadros> EntraronPintura { get; set; } = new List<Cuadros>();
        public List<Cuadros> EntraronInspeccion { get; set; } = new List<Cuadros>();
        public List<Cuadros> EntraronEmpaque { get; set; } = new List<Cuadros>();

        public List<Cuadros> SalieronEnsamblaje { get; set; } = new List<Cuadros>();//
        public List<Cuadros> SalieronAlmacen { get; set; } = new List<Cuadros>();
        public List<Cuadros> SalieronPintura { get; set; } = new List<Cuadros>();
        public List<Cuadros> SalieronInspeccion { get; set; } = new List<Cuadros>();
        public List<Cuadros> SalieronEmpaque { get; set; } = new List<Cuadros>();

        public List<Cuadros> TotalPasaron { get; set; } = new List<Cuadros>();
        public List<Cuadros> TotalATrabajar { get; set; } = new List<Cuadros>();

        public Form1()
        {
            InitializeComponent();
        }



        private void TiempoGeneral_Tick(object sender, EventArgs e)
        {
            Segundos++;

            EstadoTb.Text = Estado;
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
            Estado = "En produccion";
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
                EntraronEnsamblaje.Add(item);
            }
            ProcesoEnsamblaje();
        }



        //Metodos del proceso de ensamblaje
        public void ProcesoEnsamblaje()
        {
            int n = 0; 

            EntraronEnsNUD.Value = EntraronEnsamblaje.Count;
            
            foreach (var item in EsperaEnsamblaje)
            {
                n = TiempoEns.Next(2, 7);
                item.TiempoEnsamblaje = n;
            }
            DistribuirEnsamblaje();
        }
        public void DistribuirEnsamblaje()
        {
            
            foreach (var item in EsperaEnsamblaje)
            {
                idist++;
                if(idist == 6) { idist = 1; }
                Carpinteros c = new Carpinteros();
                c = CarpinteroBLL.Buscar(idist);
                item.ID = idist;
                c.CuadrosATrabajar.Add(item);
            }
           
            EsperaEnsamblaje = new List<Cuadros>();
            C1CNUD.Value = CarpinteroBLL.Buscar(1).CuadrosATrabajar.Count;
            C2CNUD.Value = CarpinteroBLL.Buscar(2).CuadrosATrabajar.Count;
            C3CNUD.Value = CarpinteroBLL.Buscar(3).CuadrosATrabajar.Count;
            C4CNUD.Value = CarpinteroBLL.Buscar(4).CuadrosATrabajar.Count;
            C5CNUD.Value = CarpinteroBLL.Buscar(5).CuadrosATrabajar.Count;
            Carpintero1.Start();
            Carpintero2.Start();
            Carpintero3.Start();
            Carpintero4.Start();
            Carpintero5.Start();

        }

        private void Carpintero1_Tick(object sender, EventArgs e)
        {
            Cuadros cuadroaux1 = new Cuadros();
            segundoc1++;
            Carpinteros c1 = CarpinteroBLL.Buscar(1);

            if (segundoc1 == 60)
            {
                minutoc1++;
                segundoc1 = 0;
            }
            if(minutoc1 == 5)
            {
                if (c1.CuadrosATrabajar.Count != 0)
                {
                    Cuadros item = c1.CuadrosATrabajar.First();

                    item.TiempoEnsamblaje--;
                    if (item.TiempoEnsamblaje == 0)
                    {
                        item.TiempoEnAlmacen = 24;
                        SalieronEnsamblaje.Add(item);
                        EsperaAlmacen.Add(item);
                        EntraronAlmacen.Add(item);
                        c1.CuadrosATrabajar.Remove(item);
                        Almacen.Start();
                    }
                }
                TerminadosNUD.Value = SalieronEnsamblaje.Count;
                Horac1++;
                minutoc1 = 0;
            }
            C1CNUD.Value = CarpinteroBLL.Buscar(1).CuadrosATrabajar.Count;
        }
        
        private void Carpintero2_Tick(object sender, EventArgs e)
        {
            Cuadros cuadroaux2 = new Cuadros();
            segundoc2++;
            Carpinteros c2 = CarpinteroBLL.Buscar(2);


            if (segundoc2 == 60)
            {
                minutoc2++;
                segundoc2 = 0;
            }
            if (minutoc2 == 5)
            {
                if(c2.CuadrosATrabajar.Count != 0)
                {
                    Cuadros item = c2.CuadrosATrabajar.First();

                    item.TiempoEnsamblaje--;
                    if (item.TiempoEnsamblaje == 0)
                    {
                        item.TiempoEnAlmacen= 24;
                        SalieronEnsamblaje.Add(item);
                        EsperaAlmacen.Add(item);
                        EntraronAlmacen.Add(item);
                        c2.CuadrosATrabajar.Remove(item);
                        Almacen.Start();
                    }
                }
                TerminadosNUD.Value = SalieronEnsamblaje.Count;
                Horac2++;
                minutoc2 = 0;
            }
            C2CNUD.Value = CarpinteroBLL.Buscar(2).CuadrosATrabajar.Count;
        }

        private void Carpintero3_Tick(object sender, EventArgs e)
        {
            Cuadros cuadroaux3 = new Cuadros();
            segundoc3++;
            Carpinteros c3 = CarpinteroBLL.Buscar(3);

            if (segundoc3 == 60)
            {
                minutoc3++;
                segundoc3 = 0;
            }
            if (minutoc3 == 5)
            {
                if (c3.CuadrosATrabajar.Count != 0)
                {
                    Cuadros item = c3.CuadrosATrabajar.First();

                    item.TiempoEnsamblaje--;
                    if (item.TiempoEnsamblaje == 0)
                    {
                        item.TiempoEnAlmacen = 24;
                        SalieronEnsamblaje.Add(item);
                        EsperaAlmacen.Add(item);
                        EntraronAlmacen.Add(item);
                        c3.CuadrosATrabajar.Remove(item);
                        Almacen.Start();
                    }
                }
                TerminadosNUD.Value = SalieronEnsamblaje.Count;
                Horac3++;
                minutoc3 = 0;
            }
            C3CNUD.Value = CarpinteroBLL.Buscar(3).CuadrosATrabajar.Count;
        }

        private void Carpintero4_Tick(object sender, EventArgs e)
        {
            Cuadros cuadroaux4 = new Cuadros();
            segundoc4++;
            Carpinteros c4 = CarpinteroBLL.Buscar(4);

            if (segundoc4 == 60)
            {
                minutoc4++;
                segundoc4 = 0;
            }
            if (minutoc4 == 5)
            {
                if (c4.CuadrosATrabajar.Count != 0)
                {
                    Cuadros item = c4.CuadrosATrabajar.First();

                    item.TiempoEnsamblaje--;
                    if (item.TiempoEnsamblaje == 0)
                    {
                        item.TiempoEnAlmacen = 24;
                        SalieronEnsamblaje.Add(item);
                        EsperaAlmacen.Add(item);
                        EntraronAlmacen.Add(item);
                        c4.CuadrosATrabajar.Remove(item);
                        Almacen.Start();
                    }
                }
                TerminadosNUD.Value = SalieronEnsamblaje.Count;
                Horac4++;
                minutoc4 = 0;
            }
            C4CNUD.Value = CarpinteroBLL.Buscar(4).CuadrosATrabajar.Count;
        }

        private void Carpintero5_Tick(object sender, EventArgs e)
        {
            int TIEMPO = 0;
            Cuadros cuadroaux5 = new Cuadros();
            segundoc5++;
            Carpinteros c5 = CarpinteroBLL.Buscar(5);


            if (segundoc5 == 60)
            {
                minutoc5++;
                segundoc5 = 0;
            }
            if (minutoc5 == 5)
            {
                if (c5.CuadrosATrabajar.Count != 0)
                {
                    Cuadros item = c5.CuadrosATrabajar.First();

                    item.TiempoEnsamblaje--;
                    if (item.TiempoEnsamblaje == 0)
                    {
                        item.TiempoEnAlmacen = 24;
                        EsperaAlmacen.Add(item);
                        SalieronEnsamblaje.Add(item);
                        EntraronAlmacen.Add(item);
                        c5.CuadrosATrabajar.Remove(item);
                        Almacen.Start();
                    }
                }
                TerminadosNUD.Value = SalieronEnsamblaje.Count;
                Horac5++;
                minutoc5 = 0;
            }
            C5CNUD.Value = CarpinteroBLL.Buscar(5).CuadrosATrabajar.Count;
        }

        //Fin de Metodos del proceso de ensamblaje 

        //Metodos del proceso de almacen
        public void MetodoProcesoAlmacen()
        {
            EntraronAlmacenNUD.Value = EntraronAlmacen.Count;

            SegundosAl++;

            Cuadros c = new Cuadros();
                

            if(SegundosAl == 60)
            {
                SegundosAl = 0;
                minutosAl++;
            }
            if(minutosAl == 5)
            {
                foreach (var item in EsperaAlmacen)
                {
                    item.TiempoEnAlmacen--;
                    if(item.TiempoEnAlmacen == 0)
                    {
                        int t = Rpintura.Next(10, 21);
                        item.TiempoPintura = t;
                        SalieronAlmacen.Add(item);
                        EntraronPintura.Add(item);
                        EsperaPintura.Add(item);
                        Pintura.Start();
                    }
                }
                EsperaAlmacen.RemoveAll(r => r.TiempoEnAlmacen == 0);

                horasAl++;
                minutosAl = 0;
            }
            TerminadosAlNUD.Value = SalieronAlmacen.Count;

        }

        private void Almacen_Tick(object sender, EventArgs e)
        {
            MetodoProcesoAlmacen();
        }

        //Metodos del proceso de pintura
        private void timer1_Tick(object sender, EventArgs e)
        {
            EntraronPinturaNUD.Value = EntraronPintura.Count;

            SegundosPin++;

            Cuadros c = new Cuadros();


            if (SegundosPin == 60)
            {
                SegundosPin = 0;
                minutosPin++;

                Cuadros item = EsperaPintura.First();
                    item.TiempoPintura--;
                    if (item.TiempoPintura == 0)
                    {
                        SalieronPintura.Add(item);
                        ContadorMaquina++;
                        EsperaInspeccion.Add(item);
                        EntraronInspeccion.Add(item);
                        EsperaPintura.Remove(item);
                        Inspeccion.Start();
                    }
               
            }
            if (minutosPin == 5)
            {
                horasPin++;
                minutosPin = 0;
            }
            
            TerminadosPinNUD.Value = SalieronPintura.Count;

            if(ContadorMaquina == 20)
            {
                ContadorMaquina = 0;
                MaquinaPintura.Start();
                int d = MaquinaDamage.Next(1,4);
                if(d == 1) { MinutosMaquinaDañada = 30; }
                if(d == 2) { MinutosMaquinaDañada = 45; }
                if(d == 3) { MinutosMaquinaDañada = 60; }
            }
                


        }
        private void MaquinaPintura_Tick(object sender, EventArgs e)
        {
            SegundosMac++;
            string S = SegundosMac.ToString();
            string H = HorasMaq.ToString();
            string M = MinutosMaq.ToString();

            if(SegundosMac < 10) { S = "0" + SegundosMac; }
            if (HorasMaq < 10) { H = "0" + HorasMaq; }
            if (MinutosMaq < 10) { M = "0" + MinutosMaq; }

            TMDTb.Text = H + " : " + M + " : " + S;

            if (MinutosMaquinaDañada != 0)
            {
                Estado = "Dañada";
                Pintura.Stop();
            }
                
            else
            {
                Estado = "En produccion";
                HorasMaq = 0;
                SegundosMac = 0;
                MinutosMaq = 0;
                TMDTb.Text = string.Empty;
                Pintura.Start();
                MaquinaPintura.Stop();
            }

            if(SegundosMac == 60)
            {
                SegundosMac = 0;
                MinutosMaquinaDañada--;
                MinutosMaq++;
            }
            if(MinutosMaq == 5)
            {
                MinutosMaq = 0;
                HorasMaq++;
            }
        }
        //Metodos del proceso de inspeccion
        private void Inspeccion_Tick(object sender, EventArgs e)
        {
            SegundosIns++;
            EntraronInspeccionNUD.Value = EntraronInspeccion.Count;
            Cuadros c = new Cuadros();
            if(EsperaInspeccion.Count != 0)
            {
                foreach (var item in EsperaInspeccion)
                {
                    int p = paso.Next(1, 10);
                    int em = Empac.Next(10, 16);
                    if (p == 1) 
                    { 
                        Retrabajar.Add(item);
                        TotalATrabajar.Add(item);
                    } 
                    else 
                    {
                        TotalPasaron.Add(item);
                        EntraronEmpaque.Add(item);
                        SalieronInspeccion.Add(item);
                        item.TiempoEmpaque = em;
                        EsperaEmpaque.Add(item); 
                    }
                    Empaquetar.Start();
                }
                EsperaInspeccion.RemoveAll(r => r.TiempoPintura == 0);
            }

            if (SegundosIns == 60)
            {
                SegundosIns = 0;
                minutosIns++;
            }
            if (minutosIns == 5)
            {
                horasIns++;
                minutosIns = 0;
            }
            PasaronNUD.Value = TotalPasaron.Count;
            RetrabajarNUD.Value = TotalATrabajar.Count;
        }

        //Metodo de empaquetar
        private void Empaquetar_Tick(object sender, EventArgs e)
        {
            EntraronEmpNUD.Value = EntraronEmpaque.Count;
            SegundosEmp++;

            Cuadros c = new Cuadros();


            if (SegundosEmp == 60)
            {
                SegundosEmp = 0;
                minutosEmp++;

                foreach (var item in EsperaEmpaque)
                {
                    item.TiempoEmpaque--;
                    if (item.TiempoEmpaque == 0)
                    {
                        SalieronEmpaque.Add(item);
                        Terminados.Add(item);
                    }
                }
                EsperaEmpaque.RemoveAll(r => r.TiempoEmpaque == 0);
            }
            if (minutosEmp == 5)
            {
                horasEmp++;
                minutosEmp = 0;
            }
            AbandonaronNUD.Value = SalieronEmpaque.Count;
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
            LlegadaPaquete.Stop();
            Carpintero1.Stop();
            Carpintero2.Stop();
            Carpintero3.Stop();
            Carpintero4.Stop();
            Carpintero5.Stop();
            Pintura.Stop();
            Almacen.Stop();
            Inspeccion.Stop();
            Empaquetar.Stop();
            MaquinaPintura.Stop();

        }


    }
}
