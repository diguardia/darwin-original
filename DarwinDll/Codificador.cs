using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class Codificador
    {
        public SerializableDictionary<string, int> traduccion;
        public const int BitsCodificacion = 4;

        public Codificador()
        {
            traduccion = new SerializableDictionary<string, int>();
        }

        public static bool[] intToBoolArray(int n, int cantidadBits)
        {
            bool[] res = new bool[cantidadBits];

            String representacionBinaria = Convert.ToString (n, 2);

            if (representacionBinaria.Length > cantidadBits)
            {
                throw new Exception("Codificador: No se puede codificar el número " + n + " con " + cantidadBits + " bits");
            }
            for (int i = 0; i < representacionBinaria.Length; i++)
            {
                int indice = i + cantidadBits - representacionBinaria.Length;
                if (representacionBinaria[i].ToString() == "1")
                {
                    res[indice] = true;
                }
            }
            return res;
        }


        public bool[] CodificarObjeto(string objeto)
        {
            return intToBoolArray(CodificarObjetoInt(objeto), BitsCodificacion);
        }
        
        internal int CodificarObjetoInt(string objeto)
        {
            int res;

            if (!traduccion.TryGetValue (objeto, out res))
            {
                res = traduccion.Count;
                traduccion.Add(objeto, res);
            }

            return res;
        }
    }
}
