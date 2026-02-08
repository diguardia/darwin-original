using System;
using Microsoft.Xna.Framework;
using DarwinDLL;

namespace DarwinXNA
{
    /// <summary>
    /// Cámara que define qué porción del universo es visible en la pantalla
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Posición X de la cámara en el universo (esquina superior izquierda del viewport)
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Posición Y de la cámara en el universo (esquina superior izquierda del viewport)
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Ancho del viewport (área visible)
        /// </summary>
        public int ViewportWidth { get; private set; }

        /// <summary>
        /// Alto del viewport (área visible)
        /// </summary>
        public int ViewportHeight { get; private set; }

        /// <summary>
        /// Ancho del universo completo
        /// </summary>
        public int UniverseWidth { get; private set; }

        /// <summary>
        /// Alto del universo completo
        /// </summary>
        public int UniverseHeight { get; private set; }

        public Camera(int viewportWidth, int viewportHeight, int universeWidth, int universeHeight)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            UniverseWidth = universeWidth;
            UniverseHeight = universeHeight;
            
            // Centrar cámara inicialmente
            CenterOn(universeWidth / 2, universeHeight / 2);
        }

        /// <summary>
        /// Centra la cámara en un punto específico del universo
        /// </summary>
        public void CenterOn(double worldX, double worldY)
        {
            X = (float)(worldX - ViewportWidth / 2);
            Y = (float)(worldY - ViewportHeight / 2);
            ClampPosition();
        }

        /// <summary>
        /// Mueve la cámara por un delta
        /// </summary>
        public void Move(float deltaX, float deltaY)
        {
            X += deltaX;
            Y += deltaY;
            ClampPosition();
        }

        /// <summary>
        /// Limita la posición de la cámara para que no salga del universo
        /// </summary>
        private void ClampPosition()
        {
            // No permitir que la cámara muestre áreas fuera del universo
            X = MathHelper.Clamp(X, 0, Math.Max(0, UniverseWidth - ViewportWidth));
            Y = MathHelper.Clamp(Y, 0, Math.Max(0, UniverseHeight - ViewportHeight));
        }

        /// <summary>
        /// Convierte coordenadas del mundo a coordenadas de pantalla
        /// </summary>
        public Vector2 WorldToScreen(double worldX, double worldY)
        {
            return new Vector2((float)(worldX - X), (float)(worldY - Y));
        }

        /// <summary>
        /// Convierte coordenadas de pantalla a coordenadas del mundo
        /// </summary>
        public Punto ScreenToWorld(int screenX, int screenY)
        {
            return new Punto(screenX + X, screenY + Y);
        }

        /// <summary>
        /// Verifica si un punto del mundo es visible en el viewport
        /// </summary>
        public bool IsVisible(double worldX, double worldY)
        {
            return worldX >= X && worldX < X + ViewportWidth &&
                   worldY >= Y && worldY < Y + ViewportHeight;
        }

        /// <summary>
        /// Verifica si un rectángulo del mundo es visible en el viewport
        /// </summary>
        public bool IsVisible(Rectangulo rect)
        {
            return rect.Right >= X && rect.Left < X + ViewportWidth &&
                   rect.Bottom >= Y && rect.Top < Y + ViewportHeight;
        }

        /// <summary>
        /// Obtiene el rectángulo visible del mundo en coordenadas del mundo
        /// </summary>
        public Rectangulo GetVisibleWorldBounds()
        {
            return new Rectangulo((int)X, (int)Y, 
                                 (int)(X + ViewportWidth), 
                                 (int)(Y + ViewportHeight));
        }
    }
}
