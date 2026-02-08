using System.Drawing; 

using System;
using System.Collections.Generic ;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Animal
    public abstract class Animal : SerVivo
    {
        public int Comidas; 
        public SerVivo comidaCargada; 
        public int ComidasSinSexo;
        private double anguloEficiente;
        
        private int radioCirculoEsperaReproducirse; 
        protected int m_t; 
        
        private const int MaximaEdad = 150; 
        
        // TRANSMISSINGCOMMENT: Method Mover
        public abstract void Mover();
        

        
        
        public Animal() 
        { 
            m_t = 0; 
        } 

        public override void Dibujar( Pantalla unaPantalla ) 
        { 
            System.Drawing.Color colorBichitoConMasComidas = new System.Drawing.Color(); 
            
            if ( especie.masEficiente == this ) 
            {
                unaPantalla.PegarImagen(posicion, "eficiente", 2 * Math.PI - anguloEficiente);
                anguloEficiente = anguloEficiente + .1;
                if (anguloEficiente <= 2 * Math.PI)
                    anguloEficiente = anguloEficiente - 2 * Math.PI;
            } 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method EscribirInfoRelevante
        public override void EscribirInfoRelevante( Ventana unaVentana ) 
        { 
            unaVentana.Escribir( Id + ( ( string )( EstCargandoComida()? " *" : "" ) ), Color.Black ); 
            unaVentana.Escribir( "Edad: " + Edad(), Color.Black );
            unaVentana.Escribir("Comidas: " + Comidas + " Nec: " + this.especie.ComidasNecesarias(), Color.Black); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method EsperaParaReproducirse
        public override bool EsperaParaReproducirse() 
        {
            return ComidasSinSexo >= especie.ComidasNecesarias(); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method SePuedeReproducirCon
        public override bool SePuedeReproducirCon( SerVivo otroSerVivo ) 
        { 
            return otroSerVivo.GetType().IsInstanceOfType( this ) & !( otroSerVivo == this ) & otroSerVivo.EsperaParaReproducirse(); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Morir
        public override void Morir() 
        { 
            if ( EstCargandoComida() ) 
            { 
                comidaCargada.Liberar(); 
            } 
            base.Morir(); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method ElegirAnimalConMasComidas
        /*
        protected void ElegirAnimalConMasComidas() 
        { 
/            SerVivo serVivo = null; 
            Animal oAnimalConMasComidas = this; 
            
            foreach ( SerVivo transTemp0 in universo.SeresVivos ) 
            { 
                serVivo = transTemp0; 
                if ( serVivo is Animal ) 
                { 
                    if ( oAnimalConMasComidas.Comidas < (Animal)serVivo.AgregarA|| .Comidas ) 
                    { 
                        oAnimalConMasComidas = serVivo; 
                    } 
                } 
            }
            
            AnimalConMasComidas = oAnimalConMasComidas; 
        } */
        
        
        // TRANSMISSINGCOMMENT: Method Tick
        public override void Tick() 
        {
            if ( EsperaParaReproducirse() ) 
            { 
                SerVivo canditado = BuscarPareja();
                if (canditado != null)
                    ReproducirCon(canditado);
            }           
            else 
            { 
                m_t = m_t + 1;

                Mover();

                if (comidaCargada == null) 
                { 
                    CargarComidaSiHay(); 
                } 
                else 
                {
                    if (comidaCargada.cargadoPor == null)
                    {
                        comidaCargada.cargadoPor = this;
                    }

                    if (comidaCargada.cargadoPor != this)
                    {
                        // Situación inesperada. La comida la está cargando otro
                        comidaCargada.cargadoPor.Liberar();
                        comidaCargada.cargadoPor = this;
                    }
                    MoverComida();
                }

                if (EstCargandoComida()) 
                {
                    Comer();
                } 
            } 
            
            if ( ( m_t > 1500 | Edad() > MaximaEdad ) & cargadoPor == null ) 
            { 
                Morir(); 
            } 
            
            base.Tick(); 
        }

        private void MoverComida()
        {
            Punto desplazamiento = new Punto(Math.Cos(angulo + Math.PI), Math.Sin(angulo + Math.PI));
            if (comidaCargada.especie == null)
            {
                comidaCargada = null;
            }
            else
            {
                comidaCargada.MoverA(posicion.Sumar(desplazamiento.Multiplicar(5)));
                comidaCargada.angulo = angulo;
            }
        } 
        
        
        // TRANSMISSINGCOMMENT: Method CargarComidaSiHay
        private void CargarComidaSiHay() 
        {
            SerVivo unaComida = BuscarComida(); 
            
            if ( unaComida != null) 
            { 
                CargarComida( unaComida ); 
            } 
        }

        public SerVivo BuscarComida()
        {
            int x = 0, y = 0;
            int x0 = Math.Max(universo.terreno.Left, (int)posicion.x - 3);
            int x1 = Math.Min(universo.terreno.Right, (int)posicion.x + 3);
            int y0 = Math.Max(universo.terreno.Top, (int)posicion.y - 3);
            int y1 = Math.Min(universo.terreno.Bottom, (int)posicion.y + 3);

            for (x = x0; x <= x1; x++)
            {
                for (y = y0; y <= y1; y++)
                {
                    Objeto obj = universo.terreno.ObjetoEnSuperficie(new PuntoEntero ( x, y));
                    if (obj != null && (obj.GetType().IsSubclassOf(typeof (SerVivo)))) {
                        SerVivo serVivo = (SerVivo)obj;

                        if (serVivo.especie == TipoComida() && serVivo.cargadoPor == null )
                            return (SerVivo)obj;
                    }
                }
            }

            return null;
        }

        // TRANSMISSINGCOMMENT: Method Comer
        public virtual void Comer() 
        { 
            m_t = 0; 
            Comidas = Comidas + 1; 
            ComidasSinSexo = ComidasSinSexo + 1; 
        //    ElegirAnimalConMasComidas(); 
            comidaCargada.cargadoPor = null;
            comidaCargada.Morir();
            comidaCargada = null; 
            Animal masEficiente = (Animal)especie.masEficiente;
            if (masEficiente == null || masEficiente.Comidas <= Comidas)
                especie.masEficiente = this;
            especie.comidas++;
            Sonido.Play("tragar");
        } 
        
        
        // TRANSMISSINGCOMMENT: Method CargarComida
        private void CargarComida( SerVivo unaComida ) 
        { 
            if ( comidaCargada == null ) 
            { 
                m_t = 0; 
            } 
            comidaCargada = unaComida; 
            unaComida.CargarPor( this ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Eficiencia
        public double Eficiencia() 
        { 
            return Math.Round( Comidas / Edad() * 100.0, 2 ); 
        } 
        
        
        // TRANSMISSINGCOMMENT: Method SeReprodujo
        public override void SeReprodujo() 
        { 
            ComidasSinSexo = 0;
            if (Edad() > 1) Sonido.Play("reproduccion");
        } 
        
        
        // TRANSMISSINGCOMMENT: Method EstCargandoComida
        public bool EstCargandoComida() 
        { 
            return !( comidaCargada == null ); 
        }

        public new void PrepararseParaSerializar()
        {
            comidaCargada = null;
            base.PrepararseParaSerializar();
        }

        
    } 

} 

