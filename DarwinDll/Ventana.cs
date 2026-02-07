using System.Drawing; 

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Ventana
    public class Ventana : Rectangulo 
    { 
        private Pantalla pantalla; 
        private int renglon; 
        public const int ALTORENGLON = 14;
        public String Titulo = "Sin título";
        
        public Ventana( Pantalla unaPantalla, Punto centro, int width, int height ) : base( centro, width, height ) 
        {             
            pantalla = unaPantalla; 
        } 
        
        // TRANSMISSINGCOMMENT: Method Escribir
        public void Escribir( string texto, System.Drawing.Color color ) 
        { 
            pantalla.EscribirTexto( this.Left + 4, renglon * ALTORENGLON + this.Top, texto, color ); 
            renglon = renglon + 1; 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Reset
        public void Reset() 
        { 
            renglon = 0;
            pantalla.DibujarCuadradoLleno(new Rectangulo(this.posicion.Sumar(new Punto (10,10)), Width + 2, Height + 2), Color.FromArgb (40,40,40));
            pantalla.DibujarCuadradoLleno(new Rectangulo(this.posicion, Width + 2, Height + 2), Color.Silver);
            pantalla.DibujarCuadradoLleno(this, Color.DarkGreen );
            pantalla.DibujarCuadradoLleno(new Rectangulo(Left , Top, Right, Top + 12), Color.DarkBlue );
            Escribir(Titulo, Color.White);
        } 
        
    } 
    
    
} 
