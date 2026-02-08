using System.Drawing;

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;

namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Terreno
    public class Terreno : Rectangulo
    {
        private Objeto[,] suelo;
        private Objeto[,] superficie;
        public PuntoEntero tamaño; // Hacer público para acceso desde Program

        private Objeto matriz(int x, int y)
        {
            Objeto objetoEnSuperficie = superficie[x, y];
            if (objetoEnSuperficie == null)
                return suelo[x, y];
            else
                return objetoEnSuperficie;
        }

        private Objeto matriz(int x, int y, int nivel)
        {
            try
            {
                if (nivel == 0)
                    return suelo[x, y];
                else
                    return superficie[x, y];
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public Terreno(PuntoEntero Tamaño)
            : base(0, 0, Tamaño.x - 1, Tamaño.y - 1)
        {
            tamaño = Tamaño;
            suelo = new Objeto[Tamaño.x, Tamaño.y];
            blanquearSuperficie();
        }

        public void agregar(Punto posicion, Objeto objeto)
        {
            if (posicion != null)
            {
                if (Contiene(posicion))
                {
                    superficie[(int)posicion.x, (int)posicion.y] = objeto;
                }
            }
        }


        public void AgregarSuelo(Rectangulo unRectngulo)
        {
            int indiceX = 0;
            int indiceY = 0;
            int x0 = unRectngulo.Left;
            int x1 = unRectngulo.Right;
            int y0 = unRectngulo.Top;
            int y1 = unRectngulo.Bottom;

            for (indiceX = x0; indiceX <= x1; indiceX++)
            {
                for (indiceY = y0; indiceY <= y1; indiceY++)
                {
                    suelo[indiceX, indiceY] = unRectngulo;
                }
            }
        }


        // TRANSMISSINGCOMMENT: Method Eliminar
        public void Eliminar(Punto coordenada)
        {
            if (!(coordenada == null) && this.Contiene(coordenada))
            {
                superficie[(int)coordenada.x, (int)coordenada.y] = null;
            }
        }


        // TRANSMISSINGCOMMENT: Method Mover
        public void Mover(Punto origen, Punto destino, Objeto obj)
        {
            Eliminar(origen);
            agregar(destino, obj);
        }

        public Punto Buscar(SerVivo ser)
        {
            for (int x = Left; x <= Right; x++)
            {
                for (int y = Top; y <= Bottom; y++)
                {
                    if (superficie[x, y] == ser)
                    {
                        return new Punto(x, y);
                    }
                }
            }
            return null;
        }

        // TRANSMISSINGCOMMENT: Method objetos
        public bool HayObjetos(Punto centro, int radio)
        {
            int x = 0, y = 0;
            int x0 = Math.Max(Left, (int)centro.x - radio);
            int x1 = Math.Min(Right, (int)centro.x + radio);
            int y0 = Math.Max(Top, (int)centro.y - radio);
            int y1 = Math.Min(Bottom, (int)centro.y + radio);

            for (x = x0; x <= x1; x++)
            {
                for (y = y0; y <= y1; y++)
                {
                    if (matriz(x, y) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        // TRANSMISSINGCOMMENT: Method objetos
        public Objeto ObtenerUn(Type tipo, Punto centro, int radio)
        {
            int x = 0, y = 0;
            int x0 = Math.Max(Left, (int)centro.x - radio);
            int x1 = Math.Min(Right, (int)centro.x + radio);
            int y0 = Math.Max(Top, (int)centro.y - radio);
            int y1 = Math.Min(Bottom, (int)centro.y + radio);

            for (x = x0; x <= x1; x++)
            {
                for (y = y0; y <= y1; y++)
                {
                    Objeto obj = matriz(x, y);
                    if (obj != null && (obj.GetType().IsSubclassOf(tipo) | obj.GetType() == tipo))
                    {
                        if (((SerVivo)obj).cargadoPor == null)
                            return (SerVivo)obj;
                    }
                }
            }

            return null;
        }

        public Objeto UnObjeto(Punto centro, int radio)
        {
            int x0 = Math.Max(Left, (int)centro.x - radio);
            int x1 = Math.Min(Right, (int)centro.x + radio);
            int y0 = Math.Max(Top, (int)centro.y - radio);
            int y1 = Math.Min(Bottom, (int)centro.y + radio);

            for (int x = x0; x <= x1; x++)
            {
                for (int y = y0; y <= y1; y++)
                {
                    if (matriz(x, y) != null)
                    {
                        return matriz(x, y);
                    }
                }
            }

            return null;
        }

        public Objeto UnObjeto(Punto centro, int radio, int nivel)
        {
            int x0 = Math.Max(Left, (int)centro.x - radio);
            int x1 = Math.Min(Right, (int)centro.x + radio);
            int y0 = Math.Max(Top, (int)centro.y - radio);
            int y1 = Math.Min(Bottom, (int)centro.y + radio);

            for (int x = x0; x <= x1; x++)
            {
                for (int y = y0; y <= y1; y++)
                {
                    Objeto obj = matriz(x, y, nivel);
                    if (obj != null)
                    {
                        return obj;
                    }
                }
            }

            return null;
        }

        public object IncrementarColor(int c)
        {
            c = c + Random.Entero(-2, 2);

            if (c > 255)
            {
                c = 255;
            }

            if (c < 0)
            {
                c = 0;
            }

            return c;
        }

        public Objeto ObjetoEnSuperficie(PuntoEntero p)
        {
            try
            {
                return superficie[p.x, p.y];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        internal void blanquearSuperficie()
        {
            superficie = new Objeto[tamaño.x, tamaño.y];
        }
    }


}
