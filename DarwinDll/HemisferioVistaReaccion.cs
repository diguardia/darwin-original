using System.Drawing;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{

    public class HemisferioVistaReaccion : Hemisferio
    {
        public DiccionarioAcciones diccionarioAcciones = new DiccionarioAcciones();

        private HemisferioVistaReaccion(DiccionarioAcciones diccionario)
        {
            diccionarioAcciones = diccionario;
        }

        public HemisferioVistaReaccion()
        {

        }

        public override Accion SeleccionarAccionSegunVista(Vista vista, bool bloqueado, Codificador codificador)
        {
            return SeleccionarAccionSegunClaveVista (vista.Clave (), bloqueado );
        }

        private Accion SeleccionarAccionSegunClaveVista(String claveVista, bool bloqueado)
        {
            if (!(diccionarioAcciones.ContainsKey(claveVista)))
            {
                if (bloqueado)
                {
                    diccionarioAcciones.Add(claveVista, Accion.CrearUnaAccionDeGiro() );
                }
                else
                {
                    diccionarioAcciones.Add(claveVista, Accion.CrearUnaAccion());
                }
            }

            return (Accion)diccionarioAcciones[claveVista];
        }

        public override Hemisferio CruzarCon(Hemisferio unHemisferio, bool bloqueado)
        {
            DiccionarioAcciones diccionarioResultado = new DiccionarioAcciones();

            CruzarGenes((HemisferioVistaReaccion)unHemisferio, diccionarioResultado,bloqueado);

            return new HemisferioVistaReaccion(diccionarioResultado);
        }

        private void CruzarGenes(HemisferioVistaReaccion unHemisferio, DiccionarioAcciones diccionarioResultado, bool bloqueado)
        {
            foreach (string claveVista in diccionarioAcciones.Keys)
            {
                int r = Random.EnteroEntreCeroY(11);
                if (r < 5)
                    diccionarioResultado.Add(claveVista, SeleccionarAccionSegunClaveVista(claveVista,bloqueado));
                else if (r <= 10)
                    diccionarioResultado.Add(claveVista, unHemisferio.SeleccionarAccionSegunClaveVista(claveVista, bloqueado));
            }

            foreach (string claveVista in unHemisferio.diccionarioAcciones.Keys)
            {
                if (!(unHemisferio.diccionarioAcciones.ContainsKey(claveVista)))
                {
                    int r = Random.EnteroEntreCeroY(4);
                    if (r <= 1)
                        diccionarioResultado.Add(claveVista, SeleccionarAccionSegunClaveVista(claveVista, bloqueado));
                    else if (r <= 3)
                        diccionarioResultado.Add(claveVista, unHemisferio.SeleccionarAccionSegunClaveVista(claveVista, bloqueado));
                }
            }
        }

        public void EscribirInfo(Ventana unaVentana)
        {
            foreach (KeyValuePair<string, Accion> entrada  in diccionarioAcciones)
            {
                string ClaveVista = System.Convert.ToString(entrada.Key);
                Accion accion = ((DarwinDLL.Accion)(entrada.Value));

                unaVentana.Escribir(ClaveVista + ": " + accion.Nombre(), Color.Black);
            }
        }

    }


}
