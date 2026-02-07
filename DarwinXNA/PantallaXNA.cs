using System;
using System.Collections.Generic;
using System.Text;
using DarwinDLL;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Collections;

namespace DarwinXNA
{
    class PantallaXNA : Pantalla

    {
        protected SpriteBatch ForegroundBatch;
        private ContentManager content;
        private Hashtable sprites;
        private GraphicsDeviceManager graphics;
        private XNAFont textWriter;

        #region Pantalla Members
        
        public PantallaXNA (GraphicsDeviceManager _graphics, ContentManager _content ) {
            ForegroundBatch = new SpriteBatch(_graphics.GraphicsDevice);
            content = _content;
            sprites = new Hashtable();
            graphics = _graphics;
            CargarImagen("Punto");
            textWriter = XNAFont.LoadFont(graphics.GraphicsDevice,
                "Fonts\\franklin.dds.xml", "Fonts\\franklin.dds", ForegroundBatch  );
        }

        public void IniciarBatch()
        {
            ForegroundBatch.Begin(SpriteBlendMode.AlphaBlend  ,SpriteSortMode.Immediate  ,SaveStateMode.None  );
        }

        public void FinalizarBatch()
        {
            ForegroundBatch.End();
        }

        #endregion

        #region Pantalla Members

        public void DibujarCirculo(Punto centro, object radio, System.Drawing.Color color)
        {
//            throw new Exception("The method or operation is not implemented.");
        }

        public void EscribirTexto(int x, int y, string texto, System.Drawing.Color color)
        {
            textWriter.OutputText(texto, x, y, toXNAColor ( color));                        
        }

        public void DibujarCuadrado(Rectangulo rect, System.Drawing.Color color)
        {
            ForegroundBatch.Draw(  (Texture2D)sprites ["PUNTO"], toXNARectangle(rect), toXNAColor(color));
        }

        public void DibujarCuadrado(object x0, object y0, object x1, object y1, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DibujarCuadradoLleno(Rectangulo rect, System.Drawing.Color color)
        {
            ForegroundBatch.Draw((Texture2D)sprites["PUNTO"], toXNARectangle(rect), toXNAColor(color));
        }

        public void DibujarLinea(object x0, object y0, object x1, object y1, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CargarImagen(string clave)
        {
            Texture2D SpriteTexture = content.Load<Texture2D>("imagenes\\" + clave);
            sprites.Add(clave.ToUpper(), SpriteTexture);
        }


        private Texture2D Sprite(String clave)
        {
            Texture2D res = (Texture2D)sprites[clave.ToUpper()];
            if (res == null)
            {
                CargarImagen(clave);
                res = (Texture2D)sprites[clave.ToUpper()];
            }

            return res;
        }


        public void DibujarPunto(int x0, int y0, System.Drawing.Color color)
        {
            ForegroundBatch.Draw((Texture2D)sprites["PUNTO"], new Vector2 (x0,y0), toXNAColor(color));
        }

        public void Lock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Unlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        private Microsoft.Xna.Framework.Graphics.Color toXNAColor (System.Drawing.Color c) 
        {
            return new Microsoft.Xna.Framework.Graphics.Color(c.R, c.G, c.B);    
        }

        private Microsoft.Xna.Framework.Rectangle toXNARectangle(Rectangulo rect)
        {
            return new Microsoft.Xna.Framework.Rectangle(rect.Left,rect.Top,rect.Width,rect.Height );
        }


        #region Pantalla Members

        public void PegarImagen(Punto centro, string clave)
        {
            Texture2D tex = Sprite(clave);
            Vector2 vec = new Vector2((float)centro.x - tex.Width / 2, (float)centro.y - tex.Height / 2);

            ForegroundBatch.Draw(tex, vec, Color.White);
        }

        public void PegarImagen(Punto centro, string clave, double angulo)
        {
            Vector2 vec = new Vector2((float)centro.x, (float)centro.y);
            Texture2D tex = Sprite(clave);

            ForegroundBatch.Draw(tex, vec, null, Color.White, (float)angulo, new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }

        public void PegarImagen(Rectangulo rect, string clave)
        {
            Texture2D tex = Sprite(clave);

            ForegroundBatch.Draw(tex, toXNARectangle (rect), Color.White);
        }


        #endregion
    }
}
