using System.Drawing; 

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Comida2
    public class Comida2 : Rectangulo 
    { 
        
        public int cantidadNecesaria = 1; 
        private int cicloDeVida; 
        
        public Comida2( int x, int y, int width, int height ) : base( new Punto( x, y ), width, height ) 
        { 
            
        } 
        
        // TRANSMISSINGCOMMENT: Method Tick
        public void Tick( int CantSeresVivos ) 
        { 
            cantidadNecesaria = Math.Max( System.Convert.ToInt32( ( CantSeresVivos / 50 ) ), 1 ); 
            cicloDeVida = cicloDeVida + 1; 
            if ( cicloDeVida >= 1000 ) 
            { 
                Crecer(); 
                cicloDeVida = 0; 
            } 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Crecer
        private void Crecer() 
        { 
            Height = Height + 1; 
            Width = Width + 1; 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Achicarse
        private void Achicarse() 
        { 
            if ( Height > 1 & Width > 1 ) 
            { 
                Height = Height - 1; 
                Width = Width - 1; 
            } 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method SerComida
        public void SerComida() 
        { 
            Achicarse(); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Dibujar
        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            try 
            { 
                unaPantalla.DibujarCuadradoLleno( this, Color.Green ); 
            } 
            catch ( Exception ex ) 
            { 
            } 
            unaPantalla.DibujarCuadrado( this, Color.GreenYellow ); 
        } 
        
    } 
    
    
} 
