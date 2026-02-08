using System;
using DarwinDLL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.IO;
using System.Linq;

namespace DarwinXNA
{
    class PantallaXNA : Pantalla

    {
        protected SpriteBatch ForegroundBatch;
        private Hashtable sprites;
        private GraphicsDeviceManager graphics;
        private SpriteFont textWriter;
        private Camera camera;

        #region Pantalla Members
        
        public PantallaXNA(GraphicsDeviceManager _graphics, ContentManager content, Camera cam) 
        {
            ForegroundBatch = new SpriteBatch(_graphics.GraphicsDevice);
            sprites = new Hashtable();
            graphics = _graphics;
            camera = cam;
            
            try
            {
                CargarImagen("Punto");
                
                // Cargar fuente usando el Content Pipeline de MonoGame
                textWriter = content.Load<SpriteFont>("Franklin");
                Log.Info("Fuente SpriteFont cargada correctamente");
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Error al cargar recursos en PantallaXNA");
                throw;
            }
        }

        public void IniciarBatch()
        {
            ForegroundBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
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
            // El texto de UI no se transforma con la cámara (está en coordenadas de pantalla)
            ForegroundBatch.DrawString(textWriter, texto, new Vector2(x, y), toXNAColor(color));
        }

        public void DibujarCuadrado(Rectangulo rect, System.Drawing.Color color)
        {
            // Transformar coordenadas del mundo a coordenadas de pantalla
            if (!camera.IsVisible(rect)) return; // Culling: no dibujar si está fuera de vista
            
            var screenPos = camera.WorldToScreen(rect.Left, rect.Top);
            var screenRect = new Microsoft.Xna.Framework.Rectangle(
                (int)screenPos.X, (int)screenPos.Y, rect.Width, rect.Height);
                
            ForegroundBatch.Draw((Texture2D)sprites["PUNTO"], screenRect, toXNAColor(color));
        }

        public void DibujarCuadrado(object x0, object y0, object x1, object y1, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DibujarCuadradoLleno(Rectangulo rect, System.Drawing.Color color)
        {
            // Transformar coordenadas del mundo a coordenadas de pantalla
            if (!camera.IsVisible(rect)) return; // Culling
            
            var screenPos = camera.WorldToScreen(rect.Left, rect.Top);
            var screenRect = new Microsoft.Xna.Framework.Rectangle(
                (int)screenPos.X, (int)screenPos.Y, rect.Width, rect.Height);
                
            ForegroundBatch.Draw((Texture2D)sprites["PUNTO"], screenRect, toXNAColor(color));
        }

        public void DibujarLinea(object x0, object y0, object x1, object y1, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CargarImagen(string clave)
        {
            try
            {
                Texture2D SpriteTexture = LoadTexture(clave);
                sprites.Add(clave.ToUpper(), SpriteTexture);
                Log.Debug($"Sprite '{clave}' agregado al cache");
            }
            catch (Exception ex)
            {
                Log.Exception(ex, $"Error al cargar sprite con clave '{clave}'");
                throw;
            }
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

        private Texture2D LoadTexture(string clave)
        {
            var baseDir = AppContext.BaseDirectory;
            var folder = Path.Combine(baseDir, "imagenes");
            var extensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".dds", ".tga" };

            Log.Debug($"Intentando cargar imagen: '{clave}'");
            
            string? path = extensions
                .Select(ext => Path.Combine(folder, clave + ext))
                .FirstOrDefault(File.Exists);

            if (path == null)
            {
                var error = $"No se encontró la imagen para la clave '{clave}' en {folder}";
                Log.Error(error);
                throw new FileNotFoundException(error);
            }

            try
            {
                var fileName = Path.GetFileName(path);
                var extension = Path.GetExtension(path).ToLower();
                Log.Info($"Cargando imagen: {fileName} (formato: {extension})");
                
                // Advertir si es formato problemático
                if (extension == ".dds" || extension == ".tga")
                {
                    Log.Warning($"Formato '{extension}' puede no ser compatible con MonoGame. Convierte '{fileName}' a PNG.");
                }
                
                using var stream = File.OpenRead(path);
                var texture = Texture2D.FromStream(graphics.GraphicsDevice, stream);
                
                Log.Debug($"Imagen '{fileName}' cargada exitosamente ({texture.Width}x{texture.Height})");
                return texture;
            }
            catch (Exception ex)
            {
                var fileName = Path.GetFileName(path);
                var extension = Path.GetExtension(path).ToLower();
                
                string errorMsg = $"ERROR al cargar imagen '{fileName}' (clave: '{clave}')\n" +
                                 $"Ruta completa: {path}\n" +
                                 $"Formato: {extension}\n" +
                                 $"Tamaño archivo: {new FileInfo(path).Length} bytes";
                
                if (extension == ".dds")
                {
                    errorMsg += "\n\n⚠️ FORMATO DDS NO SOPORTADO ⚠️\n" +
                               $"MonoGame no puede cargar archivos .dds\n" +
                               $"SOLUCIÓN: Convierte '{fileName}' a PNG:\n" +
                               "  1. Ejecuta: .\\ConvertDDStoPNG.ps1\n" +
                               "  2. O usa: https://www.aconvert.com/image/dds-to-png/";
                }
                else if (extension == ".tga")
                {
                    errorMsg += "\n\n⚠️ FORMATO TGA PUEDE NO SER SOPORTADO ⚠️\n" +
                               $"Intenta convertir '{fileName}' a PNG";
                }
                else
                {
                    errorMsg += $"\n\nEl archivo puede estar corrupto o ser un formato inválido.\n" +
                               "Formatos soportados: PNG, JPG, BMP, GIF";
                }
                
                Log.Error(errorMsg);
                throw new InvalidOperationException(errorMsg, ex);
            }
        }


        public void DibujarPunto(int x0, int y0, System.Drawing.Color color)
        {
            // Transformar coordenadas del mundo a coordenadas de pantalla
            if (!camera.IsVisible(x0, y0)) return; // Culling
            
            var screenPos = camera.WorldToScreen(x0, y0);
            ForegroundBatch.Draw((Texture2D)sprites["PUNTO"], screenPos, toXNAColor(color));
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

        private Microsoft.Xna.Framework.Color toXNAColor (System.Drawing.Color c) 
        {
            return new Microsoft.Xna.Framework.Color(c.R, c.G, c.B);    
        }

        private Microsoft.Xna.Framework.Rectangle toXNARectangle(Rectangulo rect)
        {
            return new Microsoft.Xna.Framework.Rectangle(rect.Left,rect.Top,rect.Width,rect.Height );
        }


        #region Pantalla Members

        public void PegarImagen(Punto centro, string clave)
        {
            Texture2D tex = Sprite(clave);
            
            // Transformar coordenadas del mundo a coordenadas de pantalla
            double worldX = centro.x - tex.Width / 2;
            double worldY = centro.y - tex.Height / 2;
            
            if (!camera.IsVisible(worldX, worldY) && 
                !camera.IsVisible(worldX + tex.Width, worldY + tex.Height)) 
                return; // Culling
            
            Vector2 screenPos = camera.WorldToScreen(worldX, worldY);
            ForegroundBatch.Draw(tex, screenPos, Color.White);
        }

        public void PegarImagen(Punto centro, string clave, double angulo)
        {
            Texture2D tex = Sprite(clave);
            
            // Transformar coordenadas del mundo a coordenadas de pantalla
            if (!camera.IsVisible(centro.x, centro.y)) return; // Culling aproximado
            
            Vector2 screenPos = camera.WorldToScreen(centro.x, centro.y);
            
            ForegroundBatch.Draw(tex, screenPos, null, Color.White, (float)angulo, 
                new Vector2(tex.Width / 2, tex.Height / 2), 1, SpriteEffects.None, 0);
        }

        public void PegarImagen(Rectangulo rect, string clave)
        {
            Texture2D tex = Sprite(clave);
            
            // Transformar coordenadas del mundo a coordenadas de pantalla
            if (!camera.IsVisible(rect)) return; // Culling
            
            var screenPos = camera.WorldToScreen(rect.Left, rect.Top);
            var screenRect = new Microsoft.Xna.Framework.Rectangle(
                (int)screenPos.X, (int)screenPos.Y, rect.Width, rect.Height);

            ForegroundBatch.Draw(tex, screenRect, Color.White);
        }


        #endregion
    }
}
