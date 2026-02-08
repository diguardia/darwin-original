using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [XmlInclude(typeof(AccionGirarDerecha))]
    [XmlInclude(typeof(AccionGirarIzquierda))]
    [XmlInclude(typeof(AccionSeguir))]
    public abstract class Accion  
    {
        enum TipoAccion
        {
            Seguir,
            GirarDerecha,
            GirarIzquierda
        }
        public abstract void Ejecutar( AnimalQueMira unAnimalQueMira );
        
        public abstract string Nombre();

        [XmlAttribute] public int Velocidad;
        [XmlAttribute]
        public int Duracion;

        public static Accion CrearUnaAccion() 
        {
            int vel = Random.EnteroEntreCeroY(3);
            int dur = Random.EnteroEntreCeroY(40);
            switch (Random.EnteroEntreCeroY(2)) 
            {
                case 0:
                    return new AccionSeguir(vel, dur); 
                case 1:
                    return new AccionGirarDerecha(vel, dur); 
                case 2:
                    return new AccionGirarIzquierda(vel, dur); 
            }
            
            return null;
        }


        internal static Accion CrearUnaAccionDeGiro()
        {
            int vel = Random.EnteroEntreCeroY(3);
            int dur = Random.EnteroEntreCeroY(40);

            switch (Random.EnteroEntreCeroY(1))
            {
                case 0:
                    return new AccionGirarDerecha(vel,dur);
                case 1:
                    return new AccionGirarIzquierda(vel, dur);
            }

            return null;
        }
    }    
} 
