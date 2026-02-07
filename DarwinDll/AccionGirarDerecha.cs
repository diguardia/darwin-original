using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class AccionGirarDerecha
    public class AccionGirarDerecha : Accion 
    {
        public AccionGirarDerecha()
            : this(0,0)
        {
        }
        public AccionGirarDerecha(int vel, int dur)
        {
            Velocidad = vel;
            Duracion = dur;
        }

        public override void Ejecutar( AnimalQueMira unAnimalQueMira ) 
        { 
            unAnimalQueMira.Girar( 1 ); 
        } 
        
        public override string Nombre() 
        { 
            return "Girar Der"; 
        } 
        
    } 
    
    
} 
