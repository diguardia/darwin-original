using System;
using System.Drawing;

namespace DarwinDLL
{
    public class Casa : Rectangulo
    {
        public Casa(Punto centro, int width, int height) : base(centro, width, height)
        {
        }

        public override void Dibujar(Pantalla unaPantalla)
        {
            unaPantalla.DibujarCuadradoLleno(this, Color.Brown);
        }

        public override string TipoDeObjeto()
        {
            return "Casa";
        }
    }
}
