using System;
using System.Windows.Forms;
using DarwinDLL;

namespace DarwinXNA
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Inicializar el log con información de sesión
            Log.InicializarSesion(GameConstants.GAME_NAME, GameConstants.VERSION);
            Log.Info($"Iniciando aplicación {GameConstants.GAME_NAME} v{GameConstants.VERSION}");
            
            try
            {
                // Cargar configuración
                var config = GameConfig.Load();
                config.ApplyToConstants();
                
                Log.Info("Cargando o creando universo...");
                Universo universo = CargarOCrearUniverso(config);
                Log.Info("Universo inicializado correctamente");
                
                Log.Info("Iniciando juego MonoGame...");
                Game1 game = new Game1(universo, config);
                game.Run();
                
                Log.Info("Guardando universo...");
                game.universo.Guardar("save.xml");
                Log.Info("Universo guardado exitosamente");
                Log.Info("Aplicación finalizada correctamente");
            }
            catch (NotSupportedException ex) when (ex.Message.Contains("DDS"))
            {
                Log.Exception(ex, "Error de formato de archivo DDS");
                
                string errorMsg = "ERROR: Formato de archivo no soportado\n\n" +
                                 ex.Message + "\n\n" +
                                 "SOLUCIÓN:\n" +
                                 "1. Ejecuta el script: .\\ConvertDDStoPNG.ps1\n" +
                                 "2. O convierte manualmente los archivos .dds a .png\n" +
                                 "   - Online: https://www.aconvert.com/image/dds-to-png/\n" +
                                 "   - O usa GIMP: https://www.gimp.org/downloads/\n\n" +
                                 "Revisa log.txt para más detalles.";
                
                MessageBox.Show(errorMsg, "Error de Formato de Archivo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Error no controlado en Main");
                
                string errorMsg = "ERROR: La aplicación ha encontrado un error.\n\n" +
                                 $"Tipo: {ex.GetType().Name}\n" +
                                 $"Mensaje: {ex.Message}\n\n" +
                                 "Revisa log.txt para más detalles.";
                
                MessageBox.Show(errorMsg, "Error de Aplicación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //static void Main(string[] args)
        //{
        //    bool exit = false;
        //    Universo universo = CargarOCrearUniverso();

        //    //while (!exit)
        //    //{
        //        Game1 game = new Game1(universo);
        //        game.Run();

        //        Darwin f = new Darwin();
        //        f.Iniciar(universo);
        //        Application.Run(new Darwin() );
        //    //}
        //}

        private static Universo CargarOCrearUniverso(GameConfig config)
        {
            Universo universo;

            if (System.IO.File.Exists("save.xml"))
            {
                Log.Info("Cargando universo desde save.xml...");
                try
                {
                    universo = Universo.Cargar("save.xml");
                    
                    // Verificar si las dimensiones guardadas coinciden con las constantes
                    if (universo.anchoTerreno != GameConstants.UNIVERSE_WIDTH || 
                        universo.altoTerreno != GameConstants.UNIVERSE_HEIGHT)
                    {
                        Log.Warning($"Tamaño de universo en save.xml ({universo.anchoTerreno}x{universo.altoTerreno}) " +
                                   $"difiere del tamaño fijo ({GameConstants.UNIVERSE_WIDTH}x{GameConstants.UNIVERSE_HEIGHT})");
                        Log.Info("Recreando universo con tamaño correcto (se mantendrán especies y pozos)");
                        
                        // Recrear el terreno con el tamaño correcto
                        universo.anchoTerreno = GameConstants.UNIVERSE_WIDTH;
                        universo.altoTerreno = GameConstants.UNIVERSE_HEIGHT;
                        universo.terreno = new Terreno(new PuntoEntero(GameConstants.UNIVERSE_WIDTH, GameConstants.UNIVERSE_HEIGHT));
                        
                        // Reagregar pozos al terreno
                        foreach (var pozo in universo.Pozos)
                        {
                            universo.AgregarPozoAlTerreno(pozo);
                        }
                    }
                    
                    Log.Info($"Universo cargado: {universo.anchoTerreno}x{universo.altoTerreno}");
                }
                catch (Exception ex)
                {
                    Log.Warning($"Error al cargar save.xml: {ex.Message}");
                    Log.Info("Creando nuevo universo con valores por defecto");
                    universo = new Universo(GameConstants.UNIVERSE_WIDTH, GameConstants.UNIVERSE_HEIGHT);
                    universo.InicializarPorDefault();
                }
            }
            else
            {
                Log.Info("No existe save.xml, creando nuevo universo");
                universo = new Universo(GameConstants.UNIVERSE_WIDTH, GameConstants.UNIVERSE_HEIGHT);
                universo.InicializarPorDefault();
                Log.Info($"Nuevo universo inicializado: {GameConstants.UNIVERSE_WIDTH}x{GameConstants.UNIVERSE_HEIGHT}");
            }

            // Aplicar velocidad por defecto
            universo.velocidad = config.DefaultSpeed;

            return universo;
        }

    }
}

