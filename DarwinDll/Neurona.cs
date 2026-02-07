using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarwinDLL
{
    public class Neurona
    {
        [XmlIgnore]
        public float[] pesos;
        [XmlAttribute]
        public float bias;
        [XmlAttribute]
        public float umbral;

        private String myVar;

        public String pesosSerial
        {
            get
            {
                string[] pesosAux = new string[pesos.Length];
                for (int i = 0; i < pesos.Length; i++)
                {
                    pesosAux[i] = pesos[i].ToString();
                }
                return String.Join(";", pesosAux);
            }
            set
            {
                char[] seps = { ';' };
                string[] pesosAux = value.Split(seps);
                pesos = new float[pesosAux.Length];
                for (int i = 0; i < pesosAux.Length; i++)
                {
                    pesos[i] = float.Parse(pesosAux[i]);
                }
            }
        }


        public Neurona()
        {
        }

        public Neurona(int cantidadEntradas)
        {
            pesos = new float[cantidadEntradas];
            for (int i = 0; i < pesos.Length; i++)
            {
                pesos[i] = Random.Entero(-pesos.Length, pesos.Length);
            }
            umbral = Random.Entero(-pesos.Length, pesos.Length);
            bias = Random.Entero(-pesos.Length, pesos.Length);
        }
        /*
        public bool exitar(int[] entradas)
        {
            float suma = 0;

            for (int i = 0; i < entradas.Length; i++)
            {
                    suma += pesos[i] * entradas[i];
            }

            return (suma + bias > umbral);
        }
        */
        
        public bool exitar(bool[] entradas)
        {
            float suma = 0;


            for (int i = 0; i < entradas.Length; i++)
            {
                if (entradas[i])
                {
                    suma += pesos[i];
                }
            }

            return (suma + bias > umbral);
        }

        internal Neurona CruzarCon(Neurona n)
        {
            switch (Random.EnteroEntreCeroY(2))
            {
                case 0: return n.CrearVariante();
                case 1: return this.CrearVariante();
                default: return new Neurona(this.pesos.Length);
            }
        }

        private Neurona CrearVariante()
        {
            Neurona n = new Neurona(pesos.Length);
            n.bias = this.bias + Random.Float(-1, 1);
            for (int i = 0; i < pesos.Length; i++)
            {
                n.pesos[i] = pesos[i] + Random.Float(-1, 1);
            }
            n.umbral = umbral + Random.Float(-1, 1);
            return n;
        }
    }
}
