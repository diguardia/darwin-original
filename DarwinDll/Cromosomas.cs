// TRANSINFO: Option Strict On

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Cromosomas
    public class Cromosomas  
    { 
        public Cromosoma[] Items; 
        
        public Cromosomas() 
        { 
            Items = new Cromosoma[ 0 ]; 
            Items[ 0 ] = new Cromosoma(); 
        } 
        
        private Cromosomas( Cromosoma[] pItems ) 
        { 
            Items = new Cromosoma[ pItems.Length ]; 
            int i = 0; 
            
            for ( i=0; i <= Items.Length - 2; i++ ) 
            { 
                Items[ i ] = pItems[ i ]; 
            } 
            
            Items[ Items.Length - 1 ] = new Cromosoma(); 
        } 
        
        private Cromosomas( Cromosoma[] pItems, bool blnIntercambiados ) 
        { 
            Items = new Cromosoma[ pItems.Length - 1 ]; 
            int i = 0; 
            
            for ( i=0; i <= Items.Length - 1; i++ ) 
            { 
                Items[ i ] = pItems[ i ].Intercambiados(); 
            } 
        } 
        
        public Cromosomas( Cromosomas cromosomas1, Cromosomas cromosomas2 ) 
        { 
            int i = 0; 
            int a = System.Convert.ToInt32( VBMath.Rnd() * 3.0 ); 
            int CantidadCromosomas = 0; 
            
            switch ( a ) 
            {
                case 0:
                    CantidadCromosomas = Math.Max( cromosomas1.Items.Length, cromosomas2.Items.Length ); 
                    break;
                case 1:
                    CantidadCromosomas = Math.Min( cromosomas1.Items.Length, cromosomas2.Items.Length ); 
                    break;
                default:
                    CantidadCromosomas = Math.Min( cromosomas1.Items.Length, cromosomas2.Items.Length ); 
                    if ( CantidadCromosomas > 1 ) 
                    { 
                        CantidadCromosomas = CantidadCromosomas - 1; 
                    } 
                    break;
            }
            
            
            Items = new Cromosoma[ CantidadCromosomas - 1 ]; 
            for ( i=0; i <= Items.Length - 1; i++ ) 
            { 
                if ( i > cromosomas1.Items.Length - 1 ) 
                { 
                    Items[ i ] = cromosomas2.Items[ i ]; 
                } 
                else if ( i > cromosomas2.Items.Length - 1 ) 
                { 
                    Items[ i ] = cromosomas1.Items[ i ]; 
                } 
                else 
                { 
                    Items[ i ] = new Cromosoma( cromosomas1.Items[ i ], cromosomas2.Items[ i ] ); 
                } 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Method Ampliado
        public Cromosomas Ampliado() 
        { 
            return new Cromosomas( Items ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Intercambiados
        public Cromosomas Intercambiados() 
        { 
            return new Cromosomas( Items, true ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method IncrementoX
        public double IncrementoX( double t, bool tieneHoja ) 
        { 
            int i = 0; 
            double r = 0; 
            
            if ( tieneHoja ) 
            { 
                for ( i=0; i <= Items.Length - 1; i++ ) 
                { 
                    r = r + Math.Sin( t * Items[ i ].Genes[ 0 ] ) * Items[ i ].Genes[ 1 ]; 
                } 
            } 
            else 
            { 
                for ( i=0; i <= Items.Length - 1; i++ ) 
                { 
                    r = r + Math.Sin( t * Items[ i ].Genes[ 2 ] ) * Items[ i ].Genes[ 3 ]; 
                } 
            } 
            
            return r; 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method IncrementoY
        public double IncrementoY( double t, bool tieneHoja ) 
        { 
            int i = 0; 
            double r = 0; 
            
            if ( tieneHoja ) 
            { 
                for ( i=0; i <= Items.Length - 1; i++ ) 
                { 
                    r = r + Math.Sin( t * Items[ i ].Genes[ 4 ] ) * Items[ i ].Genes[ 5 ]; 
                } 
            } 
            else 
            { 
                for ( i=0; i <= Items.Length - 1; i++ ) 
                { 
                    r = r + Math.Sin( t * Items[ i ].Genes[ 6 ] ) * Items[ i ].Genes[ 7 ]; 
                } 
            } 
            
            return r; 
        } 
        
        
    } 
    
    
} 
