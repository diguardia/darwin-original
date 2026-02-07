using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [XmlInclude(typeof(HemisferioNeuronal))]
    [XmlInclude(typeof(HemisferioVistaReaccion ))]

    public abstract class Hemisferio
    {
        public abstract Accion SeleccionarAccionSegunVista(Vista vista, bool bloqueado, Codificador codificador);
        public abstract Hemisferio CruzarCon(Hemisferio unHemisferio, bool bloqueado);
        public Hemisferio CrearVariante(bool bloqueado)
        {
            return this.CruzarCon(this, bloqueado);
        }
    }
}
