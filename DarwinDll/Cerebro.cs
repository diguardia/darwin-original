using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [XmlInclude(typeof(CerebroNeuronal))]
    [XmlInclude(typeof(CerebroVistaReaccion))]
    public abstract class Cerebro
    {
        public Hemisferio HemisferioNormal;
        public Hemisferio HemisferioBloqueado;

        public abstract Cerebro Crear( Hemisferio unHemisferioNormal, Hemisferio unHemisferioBloqueado );
        
        public Accion SeleccionarAccionSegunVista( Vista vista, bool estaCargandoComida, bool bloqueado, Codificador codificador ) 
        { 
            return Hemisferio( estaCargandoComida, bloqueado ).SeleccionarAccionSegunVista (vista,  bloqueado, codificador ); 
        }

        public Cerebro CruzarCon(Cerebro unCerebro) 
        {
            Cerebro unCerebroAnimalQueMira = (Cerebro)unCerebro;

            Hemisferio nuevoHemisferioNormal = HemisferioNormal.CruzarCon(unCerebroAnimalQueMira.HemisferioNormal, false);
            Hemisferio nuevoHemisferioBloqueado = HemisferioBloqueado.CruzarCon(unCerebroAnimalQueMira.HemisferioBloqueado, true); 
            
            return unCerebro.Crear(nuevoHemisferioNormal, nuevoHemisferioBloqueado); 
        } 
        
        private Hemisferio Hemisferio(bool estCargandoComida, bool bloqueado)
        {
            if (bloqueado)
            {
                return HemisferioBloqueado;
            }
            else
            {
                return HemisferioNormal;
            }
        }

        internal Cerebro CrearVariante()
        {
            Hemisferio nuevoHemisferioNormal = HemisferioNormal.CrearVariante(false);
            Hemisferio nuevoHemisferioBloqueado = HemisferioBloqueado.CrearVariante(true);

            return Crear(nuevoHemisferioNormal, nuevoHemisferioBloqueado);
        }
    } 
    
    
} 
