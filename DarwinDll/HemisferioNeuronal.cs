using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarwinDLL
{
    public class HemisferioNeuronal : Hemisferio 
    {
        public Neurona[] neuronasN1;
        public Neurona[] neuronasN2;

        public HemisferioNeuronal()
        {
            neuronasN1 = new Neurona[6 * Codificador.BitsCodificacion];
            neuronasN2 = new Neurona[6];

            for (int i = 0; i < neuronasN1.Length; i++)
            {
                neuronasN1[i] = new Neurona(Vista.CantidadRayos * Codificador.BitsCodificacion * Vista.profundidadTemporal);
            }

            for (int i = 0; i < neuronasN2.Length; i++)
            {
                neuronasN2[i] = new Neurona(Codificador.BitsCodificacion);
            }
        }

        public override Accion SeleccionarAccionSegunVista(Vista vista, bool bloqueado, Codificador codificador)
        {
            bool[] vistaBits = vista.ToBits(codificador);
                   // return new AccionSeguir(1, 1);

            return SeleccionarAccionSegunVista(bloqueado, vistaBits);
        }

        public Accion SeleccionarAccionSegunVista(bool bloqueado, bool[] vistaBits)
        {
            return CrearUnaAccion(exitarRed(0, vistaBits)
                                 , exitarRed(1, vistaBits)
                                 , exitarRed(2, vistaBits)
                                 , exitarRed(3, vistaBits)
                                 , exitarRed(4, vistaBits)
                                 , exitarRed(5, vistaBits)
                                 , bloqueado);
        }

        bool[] entradaN2 = new bool[Codificador.BitsCodificacion];
        private bool exitarRed(int red, bool[] vista)
        {
            int offset = red * Codificador.BitsCodificacion;

            for (int i = 0; i < Codificador.BitsCodificacion; i++)
            {
                entradaN2[i] = neuronasN1[offset + i].exitar(vista);
            }

            return neuronasN2[red].exitar(entradaN2);
        }

        public override Hemisferio CruzarCon(Hemisferio unHemisferio, bool bloqueado)
        {
            HemisferioNeuronal unHemisferioNeuronal = (HemisferioNeuronal)unHemisferio;
            HemisferioNeuronal res = new HemisferioNeuronal ();

            for (int i = 0; i < neuronasN1.Length; i++)
            {
                res.neuronasN1[i] = neuronasN1[i].CruzarCon(unHemisferioNeuronal.neuronasN1[i]);
            }

            for (int i = 0; i < neuronasN2.Length; i++)
            {
                res.neuronasN2[i] = neuronasN2[i].CruzarCon(unHemisferioNeuronal.neuronasN2[i]);
            }
            
            return res;
        }

        private Accion CrearUnaAccion(bool seguir, bool derecha, bool velocidad1, bool velocidad2, bool duracion1, bool duracion2, bool bloqueado)
        {
            int vel = boolsToInt (velocidad1,velocidad2);
            int dur = boolsToInt (duracion1,duracion2) * 4 + 1;

            if (seguir && !bloqueado)
            {
                return new AccionSeguir(vel, dur);
            }
            else if (derecha)
            {
                return new AccionGirarDerecha(vel, dur);
            }
            else
            {
                return new AccionGirarIzquierda(vel, dur);
            }
        }

        private int boolsToInt(bool b1, bool b2)
        {
            if      (!b1 && !b2) return 0;
            else if (!b1 &&  b2) return 1;
            else if ( b1 && !b2) return 2;
            else                 return 3;
        }
    }
}
