using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class SerVivoPosicion
    {
        public SerVivo serVivo;
        public Punto Posicion;

        public SerVivoPosicion(SerVivo _serVivo, Punto _posicion)
        {
            serVivo = _serVivo;
            Posicion = _posicion;

        }
    }
}
