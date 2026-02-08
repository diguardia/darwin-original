// TRANSINFO: Option Strict On
using System.Drawing; 

using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Bichito
    public class Bichito : Animal 
    { 
        
        private static int s_IdBichito; 
        private int m_signoX = 1; 
        private int m_signoY = 1; 
        private Pantalla imagen; 
        public Cromosomas Cromosomas; 
        
        public Bichito() : this( new Cromosomas() ) 
        { 
            
        } 
        
        internal Bichito( Cromosomas pCromosomas ) : base() 
        { 
            
            s_IdBichito = s_IdBichito + 1; 
            Id = "Bichito" + s_IdBichito.ToString(); 
            Cromosomas = pCromosomas; 
        } 
        
        // TRANSMISSINGCOMMENT: Method CrearMutante
        public Bichito CrearMutante() 
        { 
            return new Bichito( Cromosomas.Ampliado() ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method CruzarCon
        public Bichito CruzarCon( Bichito unBichito ) 
        { 
            return new Bichito( new Cromosomas( Cromosomas, unBichito.Cromosomas ) ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Dibujar
        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            Cromosoma oCromosoma = null; 
            
            if ( imagen == null ) 
            { 
                int items = Cromosomas.Items.Length; 
                imagen = unaPantalla.CrearImagen( items + 1, items + 1 ); 
                
                foreach ( Cromosoma transTemp0 in Cromosomas.Items ) 
                { 
                    oCromosoma = transTemp0; /* TRANSWARNING: check temp variable in foreach */ 
                    int i = 0; 
                    
                    imagen.DibujarCuadrado( 0, 0, items, items, oCromosoma.color() ); 
                    i = i + 1; 
                }
            } 
            
            unaPantalla.PegarImagen( posicion, imagen ); 
            base.Dibujar( unaPantalla ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method ReproducirCon
        public override void ReproducirCon( SerVivo unSerVivo ) 
        { 
            Bichito unBichito = ( ( Bichito )( unSerVivo ) ); 
            int i = 0; 
            
            for ( i=1; i <= 7; i++ ) 
            { 
                universo.Agregar( CruzarCon( unBichito ) ); 
            } 
            
            //  Agrega los clones
            this.clon().AgregarA( universo ); 
            this.clon().AgregarA( universo ); 
            unSerVivo.clon().AgregarA( universo ); 
            unSerVivo.clon().AgregarA( universo ); 
            
            //  Agregar mutante
            for ( i=1; i <= 2; i++ ) 
            { 
                universo.Agregar( CrearMutante() ); 
                universo.Agregar( unBichito.CrearMutante() ); 
            } 
            
            //  Agregar Darwin con genes intercambiados
            universo.Agregar( new Bichito( Cromosomas.Intercambiados() ) ); 
            universo.Agregar( new Bichito( unBichito.Cromosomas.Intercambiados() ) ); 
            for ( i=1; i <= 2; i++ ) 
            { 
                Bichito oMutante = new Bichito( CruzarCon( unBichito ).Cromosomas.Intercambiados() ); 
                universo.Agregar( oMutante ); 
            } 
            
            SeReprodujo(); 
            unBichito.SeReprodujo(); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method ComidasNecesarias
        public override int ComidasNecesarias( Universo unUniverso ) 
        { 
            return Math.Max( System.Convert.ToInt32( unUniverso.SeresVivos.CantidadDe( typeof( Bichito ) ) / 30 ), 1 ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Mover
        public override void Mover() 
        { 
            if ( posicion.x >= universo.terreno.Right ) 
            { 
                MoverA( new Punto( universo.terreno.Right - 1, posicion.y ) ); 
                m_signoX = -m_signoX; 
            } 
            
            if ( posicion.x <= universo.terreno.Left ) 
            { 
                MoverA( new Punto( universo.terreno.Left + 1, posicion.y ) ); 
                m_signoX = -m_signoX; 
            } 
            
            if ( posicion.y >= universo.terreno.Bottom ) 
            { 
                MoverA( new Punto( posicion.x, universo.terreno.Bottom - 1 ) ); 
                m_signoY = -m_signoY; 
            } 
            
            if ( posicion.y <= universo.terreno.Top ) 
            { 
                MoverA( new Punto( posicion.x, universo.terreno.Top + 1 ) ); 
                m_signoY = -m_signoY; 
            } 
            
            Punto desplazamiento = new Punto( Cromosomas.IncrementoX( m_t, EstCargandoComida() ) * m_signoX, Cromosomas.IncrementoY( m_t, EstCargandoComida() ) * m_signoY ); 
            MoverA( posicion.Sumar( desplazamiento.Normalizar() ) ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method clon
        public override SerVivo clon() 
        { 
            return new Bichito( Cromosomas ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method EscribirInfoAdicional
        public override void EscribirInfoAdicional( Ventana unaVentana, Pantalla unaPantalla ) 
        { 
            
        } 
        
        
        // TRANSMISSINGCOMMENT: Method TipoComida
        public override System.Type TipoComida() 
        { 
            return typeof( Planta ); 
        } 
        
    } 
    
    
} 
