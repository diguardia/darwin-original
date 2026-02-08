using System.Drawing;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Pozo
    public class Pozo : Rectangulo
    {
        [XmlIgnore]
        public Universo universo;

        public Pozo()
            : this(new Punto(100, 100), 100, 100)
        {
        }

        public Pozo(Punto centro, int width, int height)
            : base(centro, width, height)
        {

        }

        // TRANSMISSINGCOMMENT: Method tick
        public void tick()
        {
            MatarSeresVivos();
        }


        public void MatarSeresVivos()
        {
            foreach (Especie especie in universo.Especies)
            {
                foreach (SerVivo oSerVivo in especie.iteradorIndividuos())
                {
                    if (this.Contiene(oSerVivo.posicion))
                    {
                        oSerVivo.Morir();
                        Sonido.Play("caida");
                    }
                }
            }
        }

        public override void Dibujar(Pantalla unaPantalla)
        {
            unaPantalla.PegarImagen(this, "pozo");
//            unaPantalla.DibujarCuadradoLleno(this, System.Drawing.Color.Black);
        }

        public override string TipoDeObjeto()
        {
            return "Pozo";
        }

    }


}
