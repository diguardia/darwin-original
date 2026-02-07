using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class AccionSeguir
    public class AccionSeguir : Accion 
    { 
        public AccionSeguir()
            : this(0,0)
        {
        }

        public AccionSeguir (int vel, int dur)
        {
            Velocidad = vel;
            Duracion = dur;
        }

        // TRANSMISSINGCOMMENT: Method Ejecutar
        public override void Ejecutar( AnimalQueMira unAnimalQueMira ) 
        { 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Nombre
        public override string Nombre() 
        { 
            return "Seguir"; 
        } 
        
    } 
    
    
} 
