using System.Drawing;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Seguidor
    public class Seguidor
    {
        public Punto posicion;
        public Objeto seleccionado;
        private double angulo;

        public Seguidor()
        {
            posicion = new Punto(0, 0);
        }

        // TRANSMISSINGCOMMENT: Method Dibujar
        public void Dibujar(Pantalla unaPantalla, Ventana ventanaSeleccion, Ventana ventanaSeleccionAdicional)
        {
            unaPantalla.PegarImagen(posicion, "seguidor", angulo);
            angulo = angulo + .1;
            if (angulo <= 2 * Math.PI)
                angulo = angulo - 2 * Math.PI;


            if (seleccionado != null)
            {
                seleccionado.EscribirInfoRelevante(ventanaSeleccion);
//                seleccionado.EscribirInfoAdicional(ventanaSeleccionAdicional, unaPantalla);
            }
        }


        // TRANSMISSINGCOMMENT: Method Tick
        public void Tick(Terreno terreno)
        {
            if (seleccionado == null)
            {
                seleccionado = ((DarwinDLL.Objeto)(terreno.UnObjeto(posicion, 10)));
            }

            if (seleccionado != null)
            {
                posicion = seleccionado.posicion.Clonar();
                if (EsUnSerVivo())
                {
                    LiberarSiEstáMuerto();
                }
            }
        }

        private void LiberarSiEstáMuerto()
        {
            SerVivo serVivo = (SerVivo)seleccionado;
            if (serVivo.EstáMuerto)
            {
                seleccionado = null;
            }
        }

        private bool EsUnSerVivo()
        {
            return seleccionado.GetType().IsSubclassOf(typeof(SerVivo));
        }


        // TRANSMISSINGCOMMENT: Method LiberSeguimiento
        public void LiberSeguimiento()
        {
            seleccionado = null;
        }

    }


}
