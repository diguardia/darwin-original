using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Nada
    public class Nada  : Objeto 
    {

        public override void EscribirInfoRelevante(Ventana unaVentana)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override Punto posicion
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void Dibujar(Pantalla pantalla)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string TipoDeObjeto()
        {
            return "Nada";
        }
    } 
    
    
} 
