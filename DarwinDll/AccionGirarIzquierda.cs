using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class AccionGirarIzquierda
    public class AccionGirarIzquierda : Accion
    {
         public AccionGirarIzquierda()
            : this(0,0)
        {
        }

        public AccionGirarIzquierda(int vel, int dur)
        {
            Velocidad = vel;
            Duracion = dur;
        }

        // TRANSMISSINGCOMMENT: Method Ejecutar
        public override void Ejecutar(AnimalQueMira unAnimalQueMira)
        {
            unAnimalQueMira.Girar(-1);
        }


        // TRANSMISSINGCOMMENT: Method Nombre
        public override string Nombre()
        {
            return "Girar Izq";
        }

    }


}
