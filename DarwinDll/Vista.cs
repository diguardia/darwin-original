using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;

namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Vista
    public class Vista  
    {
        private List<Punto> AlcanceVision = new List<Punto>();
        private AnimalQueMira animal;
        private int clave;
        private const int tamañoVision = 3;
        public const int CantidadRayos = 10;
        private String[] objetos;
        private int indice;
        public Vista visionAnterior;
        public static int profundidadTemporal = 1;
        
        public Vista( AnimalQueMira unAnimal ) 
        {
            animal = unAnimal;
        }

        public void Ver(Codificador codificador)
        {
            Log.iniciarTimer("Ver");
            AlcanceVision = new List<Punto>();
            clave = 0;
            indice = 0;
            objetos = new String[CantidadRayos];
            ObjetosEnColisión(0, codificador);
            ObjetosEnColisión(1, codificador );
            Log.detenerTimer("Ver");
        }

        private void ObjetosEnColisión(int nivel,Codificador codificador)
        {
            agregarAClave(nivel, -0.6, codificador);
//            agregarAClave(nivel, -0.3, codificador);
            agregarAClave(nivel,-0.1, codificador);
            agregarAClave(nivel, 0, codificador);
            agregarAClave(nivel, 0.1, codificador);
//            agregarAClave(nivel, 0.3, codificador);
            agregarAClave(nivel, 0.6, codificador);
        }

        private void agregarAClave(int nivel, double angulo, Codificador codificador)
        {
            String objeto = objetoEnColisión(animal.posicion, animal.angulo, angulo, nivel).TipoDeObjeto();
            objetos[indice] = objeto;
            clave = (clave * codificador.CodificarObjetoInt(objeto)) % 997 + 1;
            indice++;
        }

        public string Clave() 
        {
            return clave.ToString (); 
        }

        private Objeto objetoEnColisión( Punto posicion, double angulo, double apertura, int nivel)
        {
            Punto vista = posicion.Clonar();
            double distancia = tamañoVision * 2;

            for (int i = 1; i < 100; i++) ;
            {
                Punto incremento = new Punto(Math.Cos(angulo + apertura) * 8, Math.Sin(angulo + apertura) * 8).Multiplicar(distancia);
                apertura = apertura * 1.03;
                distancia += .2;
                vista = vista.Sumar(incremento);

                if (vista.x > animal.universo.terreno.Right)
                {
                    return new Pared("Derecha");
                }
                if (vista.x < animal.universo.terreno.Left)
                {
                    return new Pared("Izquierda");
                }
                if (vista.y > animal.universo.terreno.Bottom)
                {
                    return new Pared("Abajo");
                }
                if (vista.y < animal.universo.terreno.Top)
                {
                    return new Pared("Arriba");
                }

                Objeto obj = animal.universo.terreno.UnObjeto(vista, tamañoVision, nivel);
               // return new Pared("Derecha");
                if (!(obj == null))
                {
                    return obj;
                }

                AlcanceVision.Add(vista);
            }

            return new Nada();
        }


        internal void Dibujar(Pantalla unaPantalla)
        {
            foreach ( Punto opunto in AlcanceVision)
            {
                unaPantalla.DibujarCuadradoLleno(new Rectangulo(opunto, tamañoVision, tamañoVision), Color.Tomato);
            }
        }

        public bool[] ToBits(Codificador codificador)
        {
            bool[] bits = new bool[CantidadRayos * Codificador.BitsCodificacion * profundidadTemporal];
            ToBits(this, codificador, bits, 0);
            if (profundidadTemporal != 1)
            {
                if (visionAnterior == null || visionAnterior.objetos == null)
                {
                    visionAnterior = this;
                }
                ToBits(visionAnterior, codificador, bits, CantidadRayos * Codificador.BitsCodificacion);
            }
            return bits;
        }

        private void ToBits(Vista vista, Codificador codificador, bool[] bits, int offset)
        {
            for (int i = 0; i < vista.objetos.Length; i++)
            {
                bool[] bitsObjeto = codificador.CodificarObjeto(vista.objetos[i]);
                int offseti = i * Codificador.BitsCodificacion + offset;
                for (int j = 0; j < Codificador.BitsCodificacion; j++)
                {
                    bits[offseti + j] = bitsObjeto[j];
                }
            }
        }
    } 
    
    
} 
