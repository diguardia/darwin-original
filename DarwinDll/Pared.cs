using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class ParedArriba
    public class Pared : Objeto
    {
        String nombre;
        public Pared(string unNombre)
        {
            nombre = unNombre;
        }

        public override void EscribirInfoRelevante(Ventana unaVentana)
        {
            unaVentana.Escribir("Pared " + nombre, System.Drawing.Color.WhiteSmoke);
        }

        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla)
        {
            // Sin detalles adicionales
        }

        public override Punto posicion
        {
            get { return new Punto(0, 0); }
        }

        public override void Dibujar(Pantalla pantalla)
        {
            // Las paredes representan los bordes; no se dibujan aquí.
        }

        public override string TipoDeObjeto()
        {
            return "Pared " + nombre;
        }
    }

}
