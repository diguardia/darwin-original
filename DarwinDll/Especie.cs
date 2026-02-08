using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DarwinDLL
{
    [XmlInclude(typeof(Planta))]
    [XmlInclude(typeof(Fuego))]
    [XmlInclude(typeof(AnimalQueMira))]
    public class Especie
    {

        #region Propiedades
        //-------------------------------------------------------------
        public bool[] hash;
        [XmlAttribute]
        public String nombre;
        [XmlAttribute]
        public int comidas;
        [XmlAttribute]
        public String TypeNameIndividuos;
        [XmlAttribute]
        public int minimaCantidadDeIndividuos = 20;
        #region TipoComida
        [XmlAttribute]
        public String NombreTipoComida;
        private Especie _tipoComida;
        [XmlIgnore]
        public Especie TipoComida
        {
            get
            {
                if (_tipoComida == null)
                {
                    foreach (Especie e in this.universo.Especies)
                    {
                        if (e.nombre == NombreTipoComida)
                        {
                            _tipoComida = e;
                        }
                    }
                }
                return _tipoComida;
            }
            set
            {
                NombreTipoComida = value.nombre;
                _tipoComida = value;
            }
        }
        #endregion
        public SerVivo masEficiente;
        public EventList<SerVivo> individuos = new EventList<SerVivo>();

        [XmlIgnore]
        public Universo universo;
        [XmlIgnore]
        public ColaSerVivoPosicion colaSeresVivos = new ColaSerVivoPosicion();
        //-------------------------------------------------------------
        #endregion

        private int maximoId;
        private int pausaParaDesencolar;
        private SerVivo[] iteradorSeresVivos;

        public Especie()
        {
            individuos.beforeAdd += new EventList<SerVivo>.beforeAddDelegator(beforeAdd);
        }

        public Type Clase
        {
            set { TypeNameIndividuos = value.AssemblyQualifiedName; }
        }

        public SerVivo CrearIndividuo()
        {
            //            SerVivo serVivo = (SerVivo)System.Activator.CreateInstance(Clase, null);

            SerVivo serVivo = (SerVivo)System.Activator.CreateInstance(Type.GetType(TypeNameIndividuos), null);
            serVivo.especie = this;
            maximoId++;
            serVivo.Id = nombre + maximoId;
            return serVivo;
        }

        public void Eliminar(SerVivo serVivo)
        {
            if (individuos.Contains(serVivo))
            {
                individuos.Remove(serVivo);
            }
            universo.terreno.Eliminar(serVivo.posicion);
        }

        public void beforeAdd(SerVivo serVivo)
        {
            serVivo.especie = this;
            if (universo != null)
            {
                universo.terreno.agregar(serVivo.posicion, serVivo);
            }
        }

        public void tick()
        {
            foreach (SerVivo serVivo in iteradorIndividuos())
            {
                serVivo.Tick();
            }
            if (colaSeresVivos.Count > 0)
            {
                pausaParaDesencolar++;
                if (pausaParaDesencolar == 4)
                {
                    DesencolarIndividuo();
                    pausaParaDesencolar = 0;
                }
            }
            iteradorSeresVivos = null;
        }

        private void DesencolarIndividuo()
        {
            SerVivoPosicion serVivoPosicion = colaSeresVivos.Dequeue();
            serVivoPosicion.serVivo.MoverA(serVivoPosicion.Posicion);
            individuos.Add(serVivoPosicion.serVivo);
        }

        public int ComidasNecesarias()
        {
            return (int)Math.Pow(2, individuos.Count / 90) + individuos.Count / 40;
        }

        internal void Dibujar(Pantalla pantalla)
        {
            throw new NotImplementedException("Usar Dibujar(pantalla, nivel");
        }

        public void Dibujar(Pantalla pantalla, int nivel)
        {
            foreach (SerVivo serVivo in iteradorIndividuos())
            {
                serVivo.Dibujar(pantalla);
            }
        }

        public SerVivo[] iteradorIndividuos()
        {
            if (iteradorSeresVivos == null)
            {
                iteradorSeresVivos = new SerVivo[individuos.Count];
                individuos.CopyTo(iteradorSeresVivos);
            }

            return iteradorSeresVivos;
        }

        public void Encolar(SerVivo unSerVivo)
        {
            Punto nuevaPosicion;
            nuevaPosicion = BuscarLugarVacío();

            Encolar(unSerVivo, nuevaPosicion);
        }

        private Punto BuscarLugarVacío()
        {
            Punto nuevaPosicion;
            nuevaPosicion = PuntoRandom();
            while (universo.terreno.HayObjetos(nuevaPosicion, 8))
            {
                nuevaPosicion = PuntoRandom();
            }
            return nuevaPosicion;
        }

        private Punto PuntoRandom()
        {
            return new Punto(Random.EnteroEntreCeroY(universo.terreno.Width - 1), Random.EnteroEntreCeroY(universo.terreno.Height - 1));
        }

        public void Encolar(SerVivo unSerVivo, Punto nuevaPosicion)
        {
            unSerVivo.especie = this;
            if (colaSeresVivos.Count > 30)
                return;

            colaSeresVivos.Enqueue(new SerVivoPosicion(unSerVivo, nuevaPosicion));
        }

        internal void PrepararseParaSerializar()
        {
            while (colaSeresVivos.Count > 0)
            {
                DesencolarIndividuo();
            }
            if (masEficiente != null)
            {
                masEficiente.PrepararseParaSerializar();
            }
            foreach (SerVivo serVivo in individuos)
            {
                serVivo.PrepararseParaSerializar();
            }
        }

        internal void Reconstruir()
        {
            if (masEficiente != null)
            {
                masEficiente.especie = this;

                foreach (SerVivo serVivo in individuos)
                {
                    if (serVivo.Id == masEficiente.Id)
                    {
                        masEficiente = serVivo;
                    }
                }
            }
        }

        internal void agregarAlTerreno()
        {
            foreach (SerVivo serVivo in iteradorIndividuos())
            {
                universo.terreno.agregar(serVivo.posicion, serVivo);
            }
        }
    }
}
