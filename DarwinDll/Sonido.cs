using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class Sonido
    {
        public static Queue<string> colaSonidos = new Queue<string> ();

        public static void PlayGritoFuego()
        {
            Play ("grito2");// + Random.Entero (1,2));

        }
        public static void Play(string clave)
        {
   //         colaSonidos.Enqueue(clave);
        }
    }
}
