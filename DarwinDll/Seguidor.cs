using System;

namespace DarwinDLL
{
    public class Seguidor
    {
        public Punto posicion;
        public Objeto seleccionado;
        private double angulo;

        public Seguidor()
        {
            posicion = new Punto(0, 0);
        }

        public void Dibujar(Pantalla unaPantalla, Ventana ventanaSeleccion, Ventana ventanaSeleccionAdicional)
        {
            unaPantalla.PegarImagen(posicion, "seguidor", angulo);
            angulo = angulo + .1;
            if (angulo <= 2 * Math.PI)
                angulo = angulo - 2 * Math.PI;

            if (seleccionado != null)
            {
                seleccionado.EscribirInfoRelevante(ventanaSeleccion);
            }
        }

        public void Tick(Terreno terreno)
        {
            if (seleccionado == null)
            {
                seleccionado = terreno.UnObjeto(posicion, 10);
            }

            if (seleccionado != null)
            {
                posicion = seleccionado.posicion.Clonar();
                if (EsUnSerVivo())
                {
                    LiberarSiEstaMuerto();
                }
            }
        }

        private void LiberarSiEstaMuerto()
        {
            SerVivo serVivo = (SerVivo)seleccionado;
            if (serVivo.EstaMuerto)
            {
                seleccionado = null;
            }
        }

        private bool EsUnSerVivo()
        {
            return seleccionado is SerVivo;
        }

        public void LiberSeguimiento()
        {
            seleccionado = null;
        }
    }
}
