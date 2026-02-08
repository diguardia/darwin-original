using System.Drawing; 

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{
    public class Planta : SerVivo 
    { 
        [XmlAttribute]
        public int cicloDeVida;
        [XmlAttribute]
        public int tiempoHastaReproducir; 
        private const int MaximaEdad = 6; 
        
        public Planta() 
        { 
            tiempoHastaReproducir = Random.Entero(100, 1000);
            angulo = Random.Real(0, 2 * Math.PI);
        } 
        
        private void Reproducir() 
        { 
            Terreno terreno = universo.terreno;
            agregarPlanta(terreno, 1, 1);
            agregarPlanta(terreno, 1,-1);
            agregarPlanta(terreno, 1,-1);
            agregarPlanta(terreno,-1, 1);
        }

        private void agregarPlanta(Terreno terreno, int signoX, int signoY)
        {
            int nuevox = (int)posicion.x + Random.Entero(10, 30) * signoX;
            int nuevoy = (int)posicion.y + Random.Entero(10, 30) * signoY;

            Punto p = new Punto(nuevox, nuevoy);

            if (!terreno.HayObjetos(p, 8) && terreno.Contiene(p))
                especie.Encolar(especie.CrearIndividuo(), p);
        } 
        
        
        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            unaPantalla.PegarImagen( posicion, "FLOR" + Math.Min( Edad(), 6 ) , angulo);
        } 
        
        
        public override void EscribirInfoRelevante( Ventana unaVentana ) 
        { 
            unaVentana.Escribir( Id, Color.Black ); 
            unaVentana.Escribir( "Edad: " + Edad(), Color.Black ); 
        } 
        
        
        public override void ReproducirCon( SerVivo serVivo ) 
        { 
            
        } 
        
        
        public override void Tick() 
        { 
            MoverA( posicion ); 
            cicloDeVida = cicloDeVida + 1; 
            if ( EsperaParaReproducirse() ) 
            { 
                cicloDeVida = 0; 
                Reproducir(); 
                tiempoHastaReproducir = tiempoHastaReproducir + 1; 
            } 
            
            if ( ( Edad() > MaximaEdad ) & cargadoPor == null ) 
            { 
                Morir(); 
            } 
            
            base.Tick(); 
        } 
        
        
        public override bool SePuedeReproducirCon( SerVivo otroSerVivo ) 
        { 
            return false; 
        } 
        
        public override bool EsperaParaReproducirse() 
        {
            int cantidadDePlantas = especie.individuos.Count;
            
            return cicloDeVida/2 > (tiempoHastaReproducir) & 300 >= cantidadDePlantas; 
        } 
  
        public override SerVivo Clonar()
        {
            return especie.CrearIndividuo();
        }

        public override SerVivo ClonarConVariantes()
        {
            return especie.CrearIndividuo();
        }

        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla) 
        { 
            
        }

        public override Especie TipoComida()
        {
            return null;
        }

        public override string TipoDeObjeto()
        {
            return especie.nombre;
        }
    } 
    
    
} 
