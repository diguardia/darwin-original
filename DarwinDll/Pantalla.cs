using System.Drawing; 

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Interface Pantalla
    public interface Pantalla 
    { 
        // TRANSMISSINGCOMMENT: Method DibujarCirculo
        void DibujarCirculo( Punto centro, object radio, Color color );
        
        // TRANSMISSINGCOMMENT: Method EscribirTexto
        void EscribirTexto( int x, int y, string texto, Color color );
        
        // TRANSMISSINGCOMMENT: Method DibujarCuadrado
        void DibujarCuadrado( Rectangulo rect, Color color );
        
        // TRANSMISSINGCOMMENT: Method DibujarCuadrado
        void DibujarCuadrado( object x0, object y0, object x1, object y1, Color color );
        
        // TRANSMISSINGCOMMENT: Method DibujarCuadradoLleno
        void DibujarCuadradoLleno( Rectangulo rect, Color color );
        
        // TRANSMISSINGCOMMENT: Method DibujarLinea
        void DibujarLinea( object x0, object y0, object x1, object y1, Color color );
        
        // TRANSMISSINGCOMMENT: Method AgregarImagen
        void CargarImagen( string clave );
        
        // TRANSMISSINGCOMMENT: Method PegarImagen
        void PegarImagen( Punto centro, string clave );

        void PegarImagen( Punto centro, string clave, double angulo);

        void PegarImagen(Rectangulo rect, string clave);

        // TRANSMISSINGCOMMENT: Method DibujarPunto
        void DibujarPunto( int x0, int y0, Color color );
        
        // TRANSMISSINGCOMMENT: Method Lock
        void Lock();
        
        // TRANSMISSINGCOMMENT: Method Unlock
        void Unlock();
        
    } 
    
    
} 
