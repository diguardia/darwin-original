using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using DarwinDLL;

namespace DarwinXNA
{
    /// <summary>
    /// Configuración de la aplicación
    /// </summary>
    public class GameConfig
    {
        private const string CONFIG_FILE = "config.json";

        /// <summary>
        /// Ancho de la ventana en píxeles
        /// </summary>
        [JsonPropertyName("screenWidth")]
        public int ScreenWidth { get; set; } = 1024;

        /// <summary>
        /// Alto de la ventana en píxeles
        /// </summary>
        [JsonPropertyName("screenHeight")]
        public int ScreenHeight { get; set; } = 768;

        /// <summary>
        /// Pantalla completa al iniciar
        /// </summary>
        [JsonPropertyName("fullscreen")]
        public bool Fullscreen { get; set; } = false;

        /// <summary>
        /// Habilitar VSync
        /// </summary>
        [JsonPropertyName("vsync")]
        public bool VSync { get; set; } = true;

        /// <summary>
        /// Velocidad de simulación por defecto
        /// </summary>
        [JsonPropertyName("defaultSpeed")]
        public int DefaultSpeed { get; set; } = 1;

        /// <summary>
        /// Ancho del área de visualización del terreno (pantalla - panel)
        /// </summary>
        [JsonIgnore]
        public int ViewportWidth => ScreenWidth - GameConstants.PANEL_WIDTH;

        /// <summary>
        /// Alto del área de visualización del terreno
        /// </summary>
        [JsonIgnore]
        public int ViewportHeight => ScreenHeight;

        /// <summary>
        /// Carga la configuración desde el archivo, o crea una por defecto
        /// </summary>
        public static GameConfig Load()
        {
            try
            {
                if (File.Exists(CONFIG_FILE))
                {
                    Log.Info($"Cargando configuración desde {CONFIG_FILE}");
                    string json = File.ReadAllText(CONFIG_FILE);
                    
                    // Quitar comentarios // del JSON (no son estándar pero los usamos para documentar)
                    var lines = json.Split('\n');
                    var cleanLines = new System.Collections.Generic.List<string>();
                    foreach (var line in lines)
                    {
                        var trimmed = line.Trim();
                        if (!trimmed.StartsWith("//") && !string.IsNullOrWhiteSpace(trimmed))
                        {
                            cleanLines.Add(line);
                        }
                    }
                    json = string.Join("\n", cleanLines);
                    
                    var config = JsonSerializer.Deserialize<GameConfig>(json, new JsonSerializerOptions 
                    { 
                        WriteIndented = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    });
                    
                    if (config != null)
                    {
                        Log.Info($"Configuración cargada: {config.ScreenWidth}x{config.ScreenHeight}");
                        config.Validate();
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Warning($"Error al cargar configuración: {ex.Message}");
                Log.Info("Usando configuración por defecto");
            }

            // Crear configuración por defecto y guardarla
            var defaultConfig = new GameConfig();
            defaultConfig.Save();
            return defaultConfig;
        }

        /// <summary>
        /// Guarda la configuración en el archivo
        /// </summary>
        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                };
                string json = JsonSerializer.Serialize(this, options);
                
                // Agregar comentarios al JSON
                string jsonWithComments = 
                    "// Configuración de Darwin XNA\n" +
                    "// Modifica estos valores y reinicia el juego para aplicar cambios\n" +
                    "{\n" +
                    $"  // Ancho de la ventana (mínimo: {GameConstants.PANEL_WIDTH + 100})\n" +
                    $"  \"screenWidth\": {ScreenWidth},\n" +
                    $"  // Alto de la ventana (mínimo: 600)\n" +
                    $"  \"screenHeight\": {ScreenHeight},\n" +
                    "  // Pantalla completa al iniciar\n" +
                    $"  \"fullscreen\": {Fullscreen.ToString().ToLower()},\n" +
                    "  // Sincronización vertical (reduce tearing)\n" +
                    $"  \"vsync\": {VSync.ToString().ToLower()},\n" +
                    "  // Velocidad de simulación inicial (1 = normal)\n" +
                    $"  \"defaultSpeed\": {DefaultSpeed}\n" +
                    "}\n" +
                    $"// Nota: El viewport de juego será de {ViewportWidth}x{ViewportHeight} ({GameConstants.PANEL_WIDTH}px reservados para UI)\n" +
                    $"// El universo tiene un tamaño fijo de {GameConstants.UNIVERSE_WIDTH}x{GameConstants.UNIVERSE_HEIGHT}";

                File.WriteAllText(CONFIG_FILE, jsonWithComments);
                Log.Info($"Configuración guardada en {CONFIG_FILE}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error al guardar configuración: {ex.Message}");
            }
        }

        /// <summary>
        /// Valida y ajusta los valores de configuración
        /// </summary>
        private void Validate()
        {
            // Ancho mínimo de pantalla: panel + espacio mínimo de juego
            int minWidth = GameConstants.PANEL_WIDTH + 100;
            if (ScreenWidth < minWidth)
            {
                Log.Warning($"Ancho de pantalla {ScreenWidth} es muy pequeño, ajustando a {minWidth}");
                ScreenWidth = minWidth;
            }

            // Alto mínimo de pantalla
            if (ScreenHeight < 600)
            {
                Log.Warning($"Alto de pantalla {ScreenHeight} es muy pequeño, ajustando a 600");
                ScreenHeight = 600;
            }

            // Máximos razonables para pantalla
            if (ScreenWidth > 3840) ScreenWidth = 3840; // 4K
            if (ScreenHeight > 2160) ScreenHeight = 2160; // 4K

            // Velocidad
            if (DefaultSpeed < 1) DefaultSpeed = 1;
            if (DefaultSpeed > 100) DefaultSpeed = 100;
        }

        /// <summary>
        /// Aplica la configuración a GameConstants
        /// </summary>
        public void ApplyToConstants()
        {
            GameConstants.CalculateDimensions(ScreenWidth, ScreenHeight, ViewportWidth, ViewportHeight);
            Log.Info($"Dimensiones de pantalla: {ScreenWidth}x{ScreenHeight}");
            Log.Info($"Dimensiones de viewport: {ViewportWidth}x{ViewportHeight}");
            Log.Info($"Dimensiones de universo: {GameConstants.UNIVERSE_WIDTH}x{GameConstants.UNIVERSE_HEIGHT} (fijo)");
            Log.Info($"Panel de información: {GameConstants.PANEL_WIDTH}px");
        }
    }
}
