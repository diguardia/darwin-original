using System.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [XmlInclude(typeof(AnimalVistaReaccion))]
    [XmlInclude(typeof(AnimalNeuronal))]
    public abstract class AnimalQueMira : Animal 
    {
        private Vista Vision;
        public double velocidad = 0;
        public double velocidad_deseada = 0;
        public bool bloqueado = false;
        public int ticksDesdeUltimaAccion = 10000;
        public Accion accion;
        public Punto desplazamientoAnterior = new Punto();
        public Cerebro cerebro;
	
        public AnimalQueMira CrearConEsteCerebro(Cerebro unCerebro)
        {
            AnimalQueMira animal = (AnimalQueMira)especie.CrearIndividuo();
            animal.cerebro = unCerebro;

            return animal;
        }

        protected AnimalQueMira CruzarCon(AnimalQueMira unAnimalQueMira)
        {
            return CrearConEsteCerebro(cerebro.CruzarCon(unAnimalQueMira.cerebro));
        }

        public AnimalQueMira()
        {
            angulo = Random.Real(0, Math.PI * 2);
            Vision = new Vista(this);
        }



        // TRANSMISSINGCOMMENT: Method EscribirInfoAdicional
        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla)
        {
//            unaVentana.Escribir(Vision.Clave(), Color.Black);

            Vision.Dibujar(unaPantalla);
        }


        // TRANSMISSINGCOMMENT: Method Mover
        public override void Mover()
        {
            if (accion==null || ticksDesdeUltimaAccion > accion.Duracion)
            {
                Vista visionAnt = Vision;
                Vision = new Vista(this);
                Vision.visionAnterior = visionAnt;
                visionAnt.visionAnterior = null;
                Vision.Ver(universo.codificador);
                if (bloqueado)
                    ticksDesdeUltimaAccion = -32;
                else
                    ticksDesdeUltimaAccion = 0;
                accion = cerebro.SeleccionarAccionSegunVista(Vision, EstCargandoComida(), bloqueado, universo.codificador);
                velocidad_deseada = accion.Velocidad * 1.5 + .5;
            }
            ticksDesdeUltimaAccion = ticksDesdeUltimaAccion + 1;
            accion.Ejecutar(this);

            Punto desplazamiento = new Punto(Math.Cos(angulo), Math.Sin(angulo));
            velocidad = velocidad * .98 + velocidad_deseada * .02;
            desplazamiento = desplazamiento.Normalizar().Multiplicar(velocidad);
            desplazamiento = desplazamiento.Multiplicar (.1).Sumar(desplazamientoAnterior.Multiplicar(.9));
            Punto nuevaPosicion = posicion.Sumar(desplazamiento);
            desplazamientoAnterior = desplazamiento;

            if (EsUnaPosiciónValida(nuevaPosicion))
            {
                MoverA(nuevaPosicion);
                bloqueado = false;
            }
            else
            {
                bloqueado = true;
            }
        }

        private bool EsUnaPosiciónValida( Punto posición)
        {
            return universo.terreno.Contiene(posición);
        }

        // TRANSMISSINGCOMMENT: Method ReproducirCon
        public override void ReproducirCon(SerVivo unSerVivo)
        {
            if (this.posicion == null)
            {
                return;
            }

            if (unSerVivo.GetType() != this.GetType())
            {
                throw new Exception("Reproducción entre seres de distintas especies");
            }

            int i = 0;

            for (i = 1; i <= 20; i++)
            {
                especie.Encolar(CruzarCon((AnimalQueMira)unSerVivo), this.posicion);
            }

            //  Agrega los clones
            especie.Encolar (this.Clonar(),this.posicion);
            especie.Encolar(this.Clonar(), this.posicion);
            especie.Encolar(unSerVivo.Clonar(), this.posicion);
            especie.Encolar(unSerVivo.Clonar(), this.posicion);

            SeReprodujo();
            unSerVivo.SeReprodujo();
        }

        public void Girar(double unAngulo)
        {
            angulo = angulo + unAngulo / 10;
        }

        public override Especie TipoComida()
        {
            return especie.TipoComida;
        }

        public override SerVivo Clonar()
        {
            return CrearConEsteCerebro(cerebro);
        }

        public override SerVivo ClonarConVariantes()
        {
            return CrearConEsteCerebro(cerebro.CrearVariante());
        }

        public override void Dibujar(Pantalla unaPantalla)
        {
            unaPantalla.PegarImagen(posicion, especie.nombre, angulo);
            if (comidaCargada != null)
                comidaCargada.Dibujar(unaPantalla);
            base.Dibujar(unaPantalla);
        }

        public override string TipoDeObjeto()
        {
            return especie.nombre ;
        }
    }


}
