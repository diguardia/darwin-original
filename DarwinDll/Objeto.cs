using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace DarwinDLL
{
    [Serializable()]
    public abstract class Objeto
    {
        public abstract String TipoDeObjeto();

        public abstract void EscribirInfoRelevante( Ventana unaVentana );
        public abstract void EscribirInfoAdicional( Ventana unaVentana, Pantalla unaPantalla );
        public abstract Punto posicion { get; }
        public abstract void Dibujar(Pantalla pantalla);

        public string Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StringWriter stream = new StringWriter())
            {
                try
                {
                    serializer.Serialize(stream, this);
                    stream.Flush();
                    return stream.ToString();
                }
                catch (Exception ex)
                {
                    Log.escribir(ex.ToString());                    
                    throw ex;
                }
            }
        }

        protected static Objeto Deserialize(string xml, Type T)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException("xml");
            }

            XmlSerializer serializer = new XmlSerializer(T);
            using (StringReader stream = new StringReader(xml))
            {
                try
                {
                    return (Objeto)serializer.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    // The serialization error messages are cryptic at best.
                    // Give a hint at what happened
                    throw new InvalidOperationException("Failed to " +
                                     "create object from xml string", ex);
                }
            }
        }
    } 
}
