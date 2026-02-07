using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public bool est·Muerto;
        [XmlAttribute]
        public int TicksDesdeElNacimiento;
 
        [XmlIgnore]
        public Animal cargadoPor;
        [XmlIgnore]
        public Especie especie;

        public double angulo;

 
        public bool Est·Muerto
        {
            get { return est·Muerto; }
        }

        public abstract bool SePuedeReproducirCon(SerVivo otroSerVivo);
        public abstract void ReproducirCon(SerVivo serVivo);
        public abstract bool EsperaParaReproducirse();
        public abstract SerVivo Clonar();
        public abstract SerVivo ClonarConVariantes();

        public virtual void Tick()
        {
            TicksDesdeElNacimiento = TicksDesdeElNacimiento + 1;
        }


        public int Edad()
        {
            return System.Convert.ToInt32(TicksDesdeElNacimiento / 200);
        }

        public Universo universo
        {
            get
            {
                return especie.universo;
            }
        }

        public void MoverA(Punto NuevaPosicion)
        {
            Terreno terreno = universo.terreno;
            terreno.Mover(this.posicion, NuevaPosicion, this);

            posicionInterna = NuevaPosicion;
        }


        // TRANSMISSINGCOMMENT: Method BuscarPareja
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


        // TRANSMISSINGCOMMENT: Method Morir
        public virtual void Morir()
        {
            especie.Eliminar(this);
            Liberar();
            est·Muerto = true;
        }

        public virtual void SeReprodujo()
        {
        }

        public void CargarPor(SerVivo unSerVivo)
        {
            cargadoPor = ((DarwinDLL.Animal)(unSerVivo));
        }


        // TRANSMISSINGCOMMENT: Method Liberar
        public void Liberar()
        {
            cargadoPor = null;
        }


        // TRANSMISSINGCOMMENT: Property posicion
        public override Punto posicion
        {
            get
            {
                return posicionInterna;
            }
        }

        public void PrepararseParaSerializar()
        {
            if (cargadoPor != null)
            {
                cargadoPor.comidaCargada = null;
                cargadoPor = null;
            }
        }
    }
}
