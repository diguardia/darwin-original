using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Punto
    public class PuntoEntero
    { 
        public int x; 
        public int y;

        public PuntoEntero()
            : this(0, 0)
        {
        }
        public PuntoEntero( int coordX, int coordY ) 
        { 
            x = coordX; 
            y = coordY; 
        } 
        
        // TRANSMISSINGCOMMENT: Method Sumar
        public Punto Sumar( Punto otroPunto ) 
        { 
            return new Punto( x + otroPunto.x, y + otroPunto.y ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Norma
        public int Norma() 
        { 
            return (int)Math.Sqrt( Math.Pow(x, 2 ) + Math.Pow( y, 2 ) ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Dividir
        public Punto Dividir( int escalar ) 
        { 
            return new Punto( x / escalar, y / escalar ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Multiplicar
        public Punto Multiplicar( double escalar ) 
        { 
            return new Punto( x * escalar, y * escalar ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Normalizar
        public Punto Normalizar() 
        { 
            int dblNorma = Norma(); 
            
            return new Punto( x / dblNorma, y / dblNorma ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Clonar
        public Punto Clonar() 
        { 
            return new Punto( x, y ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method point
        public System.Drawing.Point point() 
        { 
            return new System.Drawing.Point( System.Convert.ToInt32( x ), System.Convert.ToInt32( y ) ); 
        } 
        
    } 
    
    
} 
