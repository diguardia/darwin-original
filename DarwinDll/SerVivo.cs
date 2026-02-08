using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [XmlInclude(typeof(Fuego))]
    public abstract class SerVivo : Objeto
    {
        public Punto posicionInterna;
        [XmlAttribute]
        public string Id;
        [XmlAttribute]
        public bool estaMuerto;
        [XmlAttribute]
        public int TicksDesdeElNacimiento;

        [XmlIgnore]
        public Animal cargadoPor;
        [XmlIgnore]
        public Especie especie;

        public double angulo;

        public bool EstaMuerto => estaMuerto;

        public abstract bool SePuedeReproducirCon(SerVivo otroSerVivo);
        public abstract void ReproducirCon(SerVivo serVivo);
        public abstract bool EsperaParaReproducirse();
        public abstract SerVivo Clonar();
        public abstract SerVivo ClonarConVariantes();
        public abstract Especie TipoComida();

        public virtual void Tick()
        {
            TicksDesdeElNacimiento = TicksDesdeElNacimiento + 1;
        }

        public int Edad()
        {
            return Convert.ToInt32(TicksDesdeElNacimiento / 200);
        }

        public Universo universo => especie.universo;

        public void MoverA(Punto NuevaPosicion)
        {
            Terreno terreno = universo.terreno;
            terreno.Mover(this.posicion, NuevaPosicion, this);
            posicionInterna = NuevaPosicion;
        }

        public virtual void Morir()
        {
            if (estaMuerto) return;
            especie.Eliminar(this);
            Liberar();
            estaMuerto = true;
        }

        public virtual void SeReprodujo() { }

        public void CargarPor(SerVivo unSerVivo)
        {
            cargadoPor = (Animal)unSerVivo;
        }

        public void Liberar()
        {
            cargadoPor = null;
        }

        public override Punto posicion => posicionInterna;

        public void PrepararseParaSerializar()
        {
            if (cargadoPor != null)
            {
                cargadoPor.comidaCargada = null;
                cargadoPor = null;
            }
        }

        public override void Dibujar(Pantalla pantalla) { }

        public override string TipoDeObjeto()
        {
            return especie != null ? especie.nombre : GetType().Name;
        }

        public override void EscribirInfoRelevante(Ventana unaVentana)
        {
            unaVentana.Escribir(Id ?? GetType().Name, System.Drawing.Color.Black);
        }

        public override void EscribirInfoAdicional(Ventana unaVentana, Pantalla unaPantalla)
        {
        }

        protected SerVivo BuscarPareja()
        {
            foreach (SerVivo candidato in especie.individuos)
            {
                if (this.SePuedeReproducirCon(candidato))
                {
                    return candidato;
                }
            }

            return null;
        }
    }
}
