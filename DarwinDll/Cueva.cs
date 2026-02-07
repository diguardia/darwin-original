// TRANSINFO: Option Strict On
using System.Drawing; 

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Cueva
    public class Cueva : Casa 
    { 
        
        public Cueva( Punto centro, int width, int height ) : base( centro, width, height ) 
        { 
            
        } 
        
        // TRANSMISSINGCOMMENT: Method Dibujar
        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            unaPantalla.DibujarCuadradoLleno( this, Color.Peru ); 
            unaPantalla.DibujarCuadrado( this, Color.Pink ); 
        } 
        
    } 
    
    
} 
