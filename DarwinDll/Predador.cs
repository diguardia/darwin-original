using System.Drawing; 

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Predador
    public class Predador : AnimalQueMira 
    { 
        
        private static int s_Id;
        public static int ComidasEspecie = 0;
        
        internal Predador( CerebroAnimalQueMira unCerebro ) : this() 
        { 
            
            cerebro = unCerebro; 
        } 
        
        public Predador() : base() 
        { 
            
            s_Id = s_Id + 1; 
            Id = "Predador" + s_Id.ToString(); 
        } 
        
        // TRANSMISSINGCOMMENT: Method clon
        public override SerVivo Clonar() 
        { 
            return new Predador( cerebro ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method TipoComida
        public override System.Type TipoComida() 
        { 
            return typeof( Recolector ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Dibujar
        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            unaPantalla.PegarImagen( posicion, "PREDADOR" , angulo ); 
            base.Dibujar( unaPantalla ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method CruzarCon

        public override void Comer()
        {
            base.Comer();
            ComidasEspecie++;
        }
        
    } 
    
    
} 
