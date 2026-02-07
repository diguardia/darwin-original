// TRANSINFO: Option Strict On
using System.Drawing;

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [Serializable()]
    public class Universo : Objeto
    {
        [XmlIgnore]
        public Terreno terreno;
        public EventList<Pozo> Pozos;
        public EventList<Especie> Especies;
        public Codificador codificador;
        public int velocidad;
        [XmlAttribute]
        public int regenerarTerreno;
        [XmlAttribute]
        public int tiempo;

        public Universo()
        {
            Inicializar();
        }

        private void Inicializar()
        {
            velocidad = 1;
            tiempo = 0;
            regenerarTerreno = 0;
            terreno = new Terreno(new PuntoEntero(900, 768));
            Pozos = new EventList<Pozo>();
            Especies = new EventList<Especie>();
            codificador = new Codificador();
            Pozos.beforeAdd += new EventList<Pozo>.beforeAddDelegator(beforeAdd);
            Especies.beforeAdd += new EventList<Especie>.beforeAddDelegator(beforeAdd);
        }

        public void InicializarPorDefault()
        {
            Inicializar();
            Pozos.Add(new Pozo(new Punto(500, 300), 150, 150));
            Pozos.Add(new Pozo(new Punto(500, 500), 60, 60));
            Pozos.Add(new Pozo(new Punto(20, 20), 38, 38));
            Pozos.Add(new Pozo(new Punto(20, 748), 38, 38));
            
            Pozos.Add(new Pozo(new Punto(450, 10),500, 10));
            Pozos.Add(new Pozo(new Punto(450, 758),500, 10));

            Pozos.Add(new Pozo(new Punto(880, 20), 38, 38));
            Pozos.Add(new Pozo(new Punto(880, 748), 38, 38));

            Especie Vegetal = new Especie();
            Vegetal.nombre = "Vegetal";
            Vegetal.Clase = typeof(Planta);
            Especies.Add(Vegetal);

            Especie Recolector = new Especie();
            Recolector.nombre = "Recolector";
            Recolector.TipoComida = Vegetal;
            Recolector.Clase = typeof(AnimalVistaReaccion);
            Especies.Add(Recolector);

            Especie Predador = new Especie();
            Predador.nombre = "Predador";
            Predador.TipoComida = Recolector;
            Predador.Clase = typeof(AnimalVistaReaccion);
            Especies.Add(Predador);

            Especie PredadorNeuro = new Especie();
            PredadorNeuro.nombre = "Neurodator";
            PredadorNeuro.TipoComida = Recolector;
            PredadorNeuro.Clase = typeof(AnimalNeuronal);
            Especies.Add(PredadorNeuro);

            Especie fueguito = new Especie();
            fueguito.nombre = "Fuego";
            fueguito.Clase = typeof (Fuego);
            fueguito.minimaCantidadDeIndividuos = 5;
            Especies.Add(fueguito );

            int i = 0;

            for (i = 0; i <= 10; i++)
            {
                Vegetal.Encolar(Vegetal.CrearIndividuo());
            }

            for (i = 1; i <= 50; i++)
            {
                Recolector.Encolar(Recolector.CrearIndividuo());
                Predador.Encolar(Predador.CrearIndividuo());
                PredadorNeuro.Encolar(PredadorNeuro.CrearIndividuo());
            }
        }

        private void beforeAdd(Pozo pozo)
        {
            pozo.universo = this;
            AgregarPozoAlTerreno(pozo);
        }

        public void AgregarPozoAlTerreno(Pozo pozo)
        {
            terreno.AgregarSuelo(pozo);
        }

        private void beforeAdd(Especie especie)
        {
            especie.universo = this;
/*    /        if (especie.casa != null)
            {
                terreno.AgregarSuelo(especie.casa);
            }*/
        }

        public void Tick()
        {
            for (int i = 0; i < velocidad; i++)
            {
                TickInterno();
            }
        }

        private void TickInterno()
        {
            if (regenerarTerreno == 1000)
            {
                terreno.blanquearSuperficie();
                foreach (Especie especie in Especies)
                {
                    especie.agregarAlTerreno();
                }
                regenerarTerreno = 0;
            }
            regenerarTerreno++;

            foreach (Especie especie in Especies)
            {
                Log.iniciarTimer(especie.nombre);
                especie.tick();
                Log.detenerTimer(especie.nombre);
            }
            foreach (Pozo pozo in Pozos)
            {
                pozo.tick();
            }

            foreach (Especie especie in Especies)
            {
                if (especie.individuos.Count + especie.colaSeresVivos.Count < especie.minimaCantidadDeIndividuos)
                {
                    SerVivo oSerVivo = especie.CrearIndividuo();
                    especie.Encolar(oSerVivo);

                    if (especie.masEficiente != null)
                    {
                        oSerVivo.ReproducirCon(especie.masEficiente);
                    }
                }
            }

            tiempo++;
        }

        public override void Dibujar(Pantalla pantalla)
        {
            foreach (Especie especie in Especies)
            {
                especie.Dibujar(pantalla, 0);
            }

            foreach (Especie especie in Especies)
            {
                especie.Dibujar(pantalla, 1);
            }

            foreach (Pozo pozo in Pozos)
            {
                pozo.Dibujar(pantalla);
            }
        }


        public override string TipoDeObjeto()
        {
            return "Universo";
        }

        public override void EscribirInfoRelevante(Ventana unaVentana)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override Punto posicion
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public new string Serialize()
        {
            foreach (Especie especie in Especies)
            {
                especie.PrepararseParaSerializar();
            }
            return base.Serialize();
        }

        public static Universo Deserialize(string xml)
        {
            Universo u = (Universo)Universo.Deserialize(xml, typeof(Universo));

            foreach (Pozo p in u.Pozos)
            {
                u.AgregarPozoAlTerreno(p);
            }

            foreach (Especie e in u.Especies)
            {
                e.Reconstruir();
            }

            return u;
        }

        public void Guardar(string fileName)
        {
            System.IO.StreamWriter archivo = new System.IO.StreamWriter(fileName, false, System.Text.Encoding.Unicode);
            archivo.WriteLine(this.Serialize());
            archivo.Close();
        }

        public static Universo Cargar(string fileName)
        {
            Universo universo;

            System.IO.StreamReader archivo = new System.IO.StreamReader(fileName, true);
            String xml = archivo.ReadToEnd();
            universo = Universo.Deserialize(xml);
            archivo.Close();

            return universo;
        }


    }


}
