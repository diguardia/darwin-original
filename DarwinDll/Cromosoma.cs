// TRANSINFO: Option Strict On

using System.Drawing; 

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Cromosoma
    public class Cromosoma  
    { 
        public double[] Genes = new double[ 8 ]; 
        
        public Cromosoma() 
        { 
            int i = 0; 
            
            for ( i=0; i <= 7; i++ ) 
            { 
                Genes[ i ] = ( VBMath.Rnd() - 0.5 ) / 40; 
            } 
        } 
        
        public Cromosoma( Cromosoma Cromosoma1, Cromosoma Cromosoma2 ) 
        { 
            int i = 0; 
            
            for ( i=0; i <= 7; i++ ) 
            { 
                Genes[ i ] = elegir( Cromosoma1.Genes[ i ], Cromosoma2.Genes[ i ] ); 
            } 
        } 
        
        public Cromosoma( double[] pGenes ) 
        { 
            Genes = pGenes; 
        } 
        
        // TRANSMISSINGCOMMENT: Method Intercambiados
        public Cromosoma Intercambiados() 
        { 
            return new Cromosoma( Vector.Permutar( Genes ) ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Color
        public Color color() 
        { 
            System.Drawing.Color colorReturn = new System.Drawing.Color();
            int i = 0; 
            double r = 0; 
            double g = 0; 
            double b = 0; 
            
            r += Math.Abs( Genes[ 0 ] ) + Math.Abs( Genes[ 5 ] ) + Math.Abs( Genes[ 6 ] ); 
            g += Math.Abs( Genes[ 2 ] ) + Math.Abs( Genes[ 1 ] ) + Math.Abs( Genes[ 7 ] ); 
            b += Math.Abs( Genes[ 4 ] ) + Math.Abs( Genes[ 3 ] ) + Math.Abs( Genes[ 6 ] ); 
            
            double normal = 255 / Math.Max( Math.Max( r, g ), b ); 
            return Color.FromArgb( System.Convert.ToInt32( r * normal ), System.Convert.ToInt32( g * normal ), System.Convert.ToInt32( b * normal ) ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method elegir
        private double elegir( double gen1, double gen2 ) 
        { 
            double r = 0; 
            
            switch ( Random.EnteroEntreCeroY( 3 ) ) 
            {
                case 0:
                    r = gen1 + ( gen1 * VBMath.Rnd() ) / 10; 
                    break;
                case 1:
                    r = gen2 - ( gen2 * VBMath.Rnd() ) / 10; 
                    break;
                default:
                    switch ( Random.EnteroEntreCeroY( 3 ) ) 
                    {
                        case 0:
                            r = gen1 + ( gen1 * VBMath.Rnd() ) / 10; 
                            break;
                        case 1:
                            r = gen2 - ( gen2 * VBMath.Rnd() ) / 10; 
                            break;
                        default:
                            r = -( gen1 + gen2 ) / 2; 
                            break;
                    }
                    
                    break;
            }
            
            
            if ( r == 0 )
             { 
                r = 0.000000001; } 
            
            return r; 
        } 
        
    } 
    
    
} 
