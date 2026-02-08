using System;

namespace DarwinXNA
{
    /// <summary>
    /// Constantes del juego que no son configurables por el usuario
    /// </summary>
    public static class GameConstants
    {
        /// <summary>
        /// Ancho del panel de información lateral derecho en píxeles
        /// </summary>
        public const int PANEL_WIDTH = 124;

        /// <summary>
        /// Ancho del universo (tamaño fijo del mundo de juego)
        /// </summary>
        public const int UNIVERSE_WIDTH = 2000;

        /// <summary>
        /// Alto del universo (tamaño fijo del mundo de juego)
        /// </summary>
        public const int UNIVERSE_HEIGHT = 2000;

        /// <summary>
        /// Posición X donde comienza el panel derecho (se calcula en runtime basado en la configuración)
        /// </summary>
        public static int PanelStartX { get; private set; }

        /// <summary>
        /// Ancho del viewport (área visible del universo)
        /// </summary>
        public static int ViewportWidth { get; private set; }

        /// <summary>
        /// Alto del viewport (área visible del universo)
        /// </summary>
        public static int ViewportHeight { get; private set; }

        /// <summary>
        /// Configura los valores calculados basados en la resolución de pantalla
        /// </summary>
        public static void CalculateDimensions(int screenWidth, int screenHeight, int viewportWidth, int viewportHeight)
        {
            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
            PanelStartX = viewportWidth;
        }

        // Posiciones de las ventanas de UI (relativas al panel)
        public const int INFO_WINDOW_OFFSET_X = 60;
        public const int INFO_WINDOW_OFFSET_Y = 60;
        public const int INFO_WINDOW_WIDTH = 110;
        public const int INFO_WINDOW_HEIGHT = 100;

        public const int SPECIES_WINDOW_OFFSET_X = 60;
        public const int SPECIES_WINDOW_OFFSET_Y = 450;
        public const int SPECIES_WINDOW_WIDTH = 110;
        public const int SPECIES_WINDOW_HEIGHT = 600;

        // Versión del juego
        public const string VERSION = "1.0";
        public const string GAME_NAME = "DarwinXNA";
    }
}
