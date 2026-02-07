using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class CerebroNeuronal : Cerebro 
    {
        public CerebroNeuronal()
        {
            HemisferioBloqueado = new HemisferioNeuronal();
            HemisferioNormal = new HemisferioNeuronal();
        }

        public override Cerebro Crear( Hemisferio unHemisferioNormal, Hemisferio unHemisferioBloqueado )
        { 
            CerebroNeuronal unCerebro = new CerebroNeuronal();

            unCerebro.HemisferioNormal= unHemisferioNormal;
            unCerebro.HemisferioBloqueado = unHemisferioBloqueado;

            return unCerebro;
        } 

    }
}
