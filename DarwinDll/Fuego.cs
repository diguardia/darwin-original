using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarwinDLL
{
    public class Fuego : SerVivo 
    {
        [XmlAttribute]
        public int cicloDeVida;
        [XmlAttribute]
        public int tiempoHastaReproducir;
        [XmlIgnore]
        public Rectangulo rect;
        private const int MaximaEdad = 10;

        private const int MaximosFuegos = 200;

        public Fuego() 
        { 
            tiempoHastaReproducir = Random.Entero(10, 50);
            angulo = Random.Real(0, 2 * Math.PI);
        } 

        public override bool SePuedeReproducirCon(SerVivo otroSerVivo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ReproducirCon(SerVivo serVivo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool EsperaParaReproducirse()
        {
            return cicloDeVida / 2 > (tiempoHastaReproducir) & MaximosFuegos >= especie.individuos.Count;
        }

        public override SerVivo Clonar()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override SerVivo ClonarConVariantes()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string TipoDeObjeto()
        {
            return ("Fuego");
        }

        public override void EscribirInfoRelevante(Ventana unaVentana)
        {
            
        }

        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla)
        {
            
        }

        public override void Dibujar(Pantalla pantalla)
        {
            pantalla.PegarImagen(this.posicion, "fuego" + Random.Entero (1,2), angulo);
        }

        public override void Tick()
        {
            MoverA(posicion);
            cicloDeVida = cicloDeVida + 1;
            if (EsperaParaReproducirse())
            {
                Reproducir();
                cicloDeVida = 0;
                tiempoHastaReproducir = tiempoHastaReproducir + 1;
            }

            if ((Edad() > MaximaEdad))
            {
                Morir();
            }

            angulo = (angulo + .1) % 2 * Math.PI;
            if (Random.EnteroEntreCeroY(4) == 0) MatarSeresVivos();   
            base.Tick();

        }

        public void MatarSeresVivos()
        {
            foreach (Especie especie in universo.Especies)
            {
                if (especie != this.especie)
                {
                    foreach (SerVivo oSerVivo in especie.iteradorIndividuos())
                    {
                        if (Contiene(oSerVivo))
                        {
                            if (typeof(Planta).IsInstanceOfType (oSerVivo) )
                            {
                                Reproducir();
                            }
                            oSerVivo.Morir();
                            Sonido.PlayGritoFuego ();
                        }
                    }
                }
            }
        }

        private bool Contiene(SerVivo oSerVivo)
        {
            if (rect == null)
            {
                rect = new Rectangulo(posicion, 10, 10);
            }
            return rect.Contiene(oSerVivo.posicion);
        }


        private void Reproducir()
        {
            if (MaximosFuegos >= especie.individuos.Count)
            {
                Terreno terreno = universo.terreno;
                agregarFuego(terreno);
            }
        }

        private void agregarFuego(Terreno terreno)
        {
            Planta planta = (Planta)terreno.ObtenerUn (typeof(Planta), posicion , 20);

            if ( planta != null)
            {
                especie.Encolar(especie.CrearIndividuo(), planta.posicion);
            }
        } 


    }
}
