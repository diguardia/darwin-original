// TRANSINFO: Option Strict On
using System.Drawing; 

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Recolector
    public class Recolector : AnimalQueMira 
    { 
        private static int s_Id; 
        private Pantalla imagen; 
        public static int ComidasEspecie = 0;
        private int numeroImagen = 0;
        internal Recolector( CerebroAnimalQueMira unCerebro ) : this() 
        { 
            
            cerebro = unCerebro; 
        } 
        
        public Recolector() : base() 
        { 
            
            s_Id = s_Id + 1; 
            Id = "Recolector" + s_Id.ToString(); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method clon
        public override SerVivo Clonar() 
        { 
            return new Recolector( cerebro ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Dibujar
        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            numeroImagen = 1 - numeroImagen;
            unaPantalla.PegarImagen( posicion, "recolector" + (numeroImagen + 1), base.angulo  );
            if (comidaCargada != null)
                comidaCargada.Dibujar(unaPantalla);
            base.Dibujar( unaPantalla ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method TipoComida
        public override System.Type TipoComida() 
        { 
            return typeof( Planta ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method CruzarCon
        protected override AnimalQueMira CruzarCon( AnimalQueMira unAnimalQueMira ) 
        { 
            return new Recolector( cerebro.CruzarCon( unAnimalQueMira.cerebro ) ); 
        } 

        public override void Comer()
        {
            base.Comer();
            ComidasEspecie++;
        }
        
    } //  188
    
    
} 
