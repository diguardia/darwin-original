using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class CerebroVistaReaccion : Cerebro 
    {
        public CerebroVistaReaccion()
        {
            HemisferioBloqueado = new HemisferioVistaReaccion();
            HemisferioNormal = new HemisferioVistaReaccion();
        }

        public override Cerebro Crear(Hemisferio unHemisferioNormal, Hemisferio unHemisferioBloqueado)
        {
            CerebroVistaReaccion unCerebro = new CerebroVistaReaccion();

            unCerebro.HemisferioNormal = unHemisferioNormal;
            unCerebro.HemisferioBloqueado = unHemisferioBloqueado;

            return unCerebro;
        } 

    }
}
