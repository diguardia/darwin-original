// TRANSINFO: Option Strict On

using System.Drawing; 

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Rectangulo
    public class Rectangulo : Objeto
    { 
        
        private Rectangle m_rectangle; 
        private Punto m_posicion;

        public override void Dibujar(Pantalla unaPantalla)
        {
        }

        public Rectangulo(PuntoEntero centro, int width, int height)
        {
            m_rectangle = new Rectangle(System.Convert.ToInt32(centro.x - width / 2), System.Convert.ToInt32(centro.y - height / 2), width, height);
            m_posicion = centro.Clonar();
        }

        public Rectangulo(Punto centro, int width, int height) 
        { 
            m_rectangle = new Rectangle( System.Convert.ToInt32( centro.x - width / 2 ), System.Convert.ToInt32( centro.y - height / 2 ), width, height ); 
            m_posicion = centro.Clonar(); 
        }

        public Rectangulo(int x0, int y0, int x1, int y1)
        {
            m_rectangle = new Rectangle(x0,y0, x1-x0,y1-y0);
            m_posicion = new Punto((x0 + x1) / 2, (y0 + y1) / 2);
        } 

        // TRANSMISSINGCOMMENT: Property CentroX
        public double CentroX 
        { 
            get 
            { 
                return m_posicion.x; 
            } 
            set 
            { 
                m_posicion.x = value; 
                m_rectangle.X = System.Convert.ToInt32( value - Width / 2 ); 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property CentroY
        public double CentroY 
        { 
            get 
            { 
                return m_posicion.y; 
            } 
            set 
            { 
                m_posicion.y = value; 
                m_rectangle.Y = System.Convert.ToInt32( value - Height / 2 ); 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Rectangle
        public Rectangle Rectangle 
        { 
            get 
            { 
                return m_rectangle; 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Left
        public int Left 
        { 
            get 
            { 
                return m_rectangle.Left; 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Right
        public int Right 
        { 
            get 
            { 
                return m_rectangle.Right; 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Bottom
        public int Bottom 
        { 
            get 
            { 
                 return m_rectangle.Bottom; 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Top
        public int Top 
        { 
            get 
            { 
                return m_rectangle.Top; 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Width
        public int Width 
        { 
            get 
            { 
                return m_rectangle.Width; 
            } 
            set 
            { 
                m_rectangle.Width = value; 
                m_rectangle.X = System.Convert.ToInt32( m_posicion.x - Width / 2 ); 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Property Height
        public int Height 
        { 
            get 
            { 
                return m_rectangle.Height; 
            } 
            set 
            { 
                m_rectangle.Height = value; 
                m_rectangle.Y = System.Convert.ToInt32( m_posicion.y - Height / 2 ); 
            } 
        } 
        
        // TRANSMISSINGCOMMENT: Method Contiene
        public bool Contiene( Punto coordenada ) 
        { 
            return m_rectangle.Contains( (int)coordenada.x, (int) coordenada.y ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method EscribirInfoAdicional
        public override void EscribirInfoAdicional( Ventana unaVentana, Pantalla unaPantalla ) 
        { 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method EscribirInfoRelevante
        public override void EscribirInfoRelevante( Ventana unaVentana ) 
        { 
            unaVentana.Escribir( this.GetType().Name, Color.Black ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Property posicion
        public override Punto posicion 
        { 
            get 
            { 
                return m_posicion; 
            } 
        }

        public override string TipoDeObjeto()
        {
            return this.GetType().Name;
        }
    } 
    
    
} 
