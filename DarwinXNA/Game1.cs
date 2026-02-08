
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DarwinDLL;
#endregion

namespace DarwinXNA
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public Universo universo;
        PantallaXNA pantalla;
        private Seguidor seguidor = new Seguidor();
        Ventana ventanaSeleccion;
        Ventana ventanaEspecies;
        private Camera camera;
        private GameConfig config;

        public Game1(Universo unUniverso, GameConfig gameConfig)
        {
            universo = unUniverso;
            config = gameConfig;
            graphics = new GraphicsDeviceManager(this);
            
            // Configurar el Content Root Directory
            Content.RootDirectory = "Content/bin/DesktopGL";
            
            // Aplicar configuración
            graphics.PreferredBackBufferWidth = config.ScreenWidth;
            graphics.PreferredBackBufferHeight = config.ScreenHeight;
            graphics.IsFullScreen = config.Fullscreen;
            graphics.SynchronizeWithVerticalRetrace = config.VSync;
            
            graphics.PreferMultiSampling = false;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Bgr32;
            this.IsFixedTimeStep = config.VSync;
            this.IsMouseVisible = true;
            
            Log.Info($"Configuración de pantalla aplicada: {config.ScreenWidth}x{config.ScreenHeight}, Fullscreen: {config.Fullscreen}, VSync: {config.VSync}");
        }

        protected override void Initialize()
        {
            // Crear cámara con tamaño fijo del universo
            camera = new Camera(config.ViewportWidth, config.ViewportHeight, 
                              GameConstants.UNIVERSE_WIDTH, GameConstants.UNIVERSE_HEIGHT);
            
            pantalla = new PantallaXNA(graphics, Content, camera);
            
            // Posicionar ventanas basadas en las constantes
            int panelX = GameConstants.PanelStartX + GameConstants.INFO_WINDOW_OFFSET_X;
            
            ventanaSeleccion = new Ventana(pantalla, 
                new Punto(panelX, GameConstants.INFO_WINDOW_OFFSET_Y), 
                GameConstants.INFO_WINDOW_WIDTH, 
                GameConstants.INFO_WINDOW_HEIGHT);
            ventanaSeleccion.Titulo = "Seleccionado";
            
            ventanaEspecies = new Ventana(pantalla, 
                new Punto(panelX, GameConstants.SPECIES_WINDOW_OFFSET_Y), 
                GameConstants.SPECIES_WINDOW_WIDTH, 
                GameConstants.SPECIES_WINDOW_HEIGHT);
            ventanaEspecies.Titulo = "Especies";
            
            // Posicionar seguidor en el centro del universo (no del viewport)
            seguidor.posicion.x = universo.anchoTerreno / 2;
            seguidor.posicion.y = universo.altoTerreno / 2;
            
            // Centrar cámara en el seguidor
            camera.CenterOn(seguidor.posicion.x, seguidor.posicion.y);
            
            Log.Info($"Cámara inicializada - Viewport: {camera.ViewportWidth}x{camera.ViewportHeight}, Universo: {camera.UniverseWidth}x{camera.UniverseHeight}");
            Log.Info($"Ventanas UI posicionadas - Panel inicia en x={GameConstants.PanelStartX}");

/*            audioEngine = new AudioEngine("./sonidos/darwin.xgs");
            waveBank = new WaveBank(audioEngine, "./sonidos/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "./sonidos/Sound Bank.xsb");
            */
            base.Initialize();
        }

        TimeSpan timeTick;
        TimeSpan timeDraw;
        int iTimeTick = 0;
        int iTimeDraw = 0;
        
        protected override void Update(GameTime gameTime)
        {
            if (universo.tiempo % 5000 == 0 || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                universo.Guardar("save.xml");
            }

            if (universo.tiempo % 10000 == 0 || Keyboard.GetState().IsKeyDown(Keys.R))
            {
                String s = universo.Serialize();
                universo = null;
                GC.Collect();
                universo = Universo.Deserialize(s);
            }

            DateTime t0 = DateTime.Now;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Salir();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                GameConfig newConfig = GameConfig.Load();
                universo = new Universo(GameConstants.UNIVERSE_WIDTH, GameConstants.UNIVERSE_HEIGHT);
                universo.InicializarPorDefault();
                
                // Recrear la cámara con las nuevas dimensiones (universo fijo, viewport puede cambiar)
                camera = new Camera(newConfig.ViewportWidth, newConfig.ViewportHeight,
                                  GameConstants.UNIVERSE_WIDTH, GameConstants.UNIVERSE_HEIGHT);
                camera.CenterOn(GameConstants.UNIVERSE_WIDTH / 2, GameConstants.UNIVERSE_HEIGHT / 2);
                
                Log.Info($"Universo reiniciado: {GameConstants.UNIVERSE_WIDTH}x{GameConstants.UNIVERSE_HEIGHT}");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.ToggleFullScreen();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                seguidor.seleccionado = null;
                seguidor.posicion = new Punto(Mouse.GetState().X, Mouse.GetState().Y);
            }

            universo.Tick();
            seguidor.Tick(universo.terreno);
            timeTick = DateTime.Now.Subtract(t0);
            base.Update(gameTime);
        }

        private void Salir()
        {
            this.Exit();
        }


        DateTime dtFPS = DateTime.Now;
        int fps = 0;
        int frames = 0;

        protected override void Draw(GameTime gameTime)
        {
            DateTime t0 = DateTime.Now;
            graphics.GraphicsDevice.Clear(Color.DarkGray);
            pantalla.IniciarBatch();
            pantalla.PegarImagen(universo.terreno.posicion, "fondo");

            universo.Dibujar(pantalla);

            ventanaSeleccion.Reset();
            ventanaEspecies.Reset();
            seguidor.Dibujar(pantalla, ventanaSeleccion, null);

            pantalla.DibujarCuadradoLleno(new Rectangulo(universo.terreno.Right, 0, universo.terreno.Right + 4, universo.terreno.Bottom), System.Drawing.Color.Firebrick );

            ventanaSeleccion.Escribir("Tiempo: " + Math.Round (universo.tiempo / 100.0,0), System.Drawing.Color.WhiteSmoke);
            Log.escribirTimers(ventanaSeleccion);
            ImprimirTextoEspecies();
//            ImprimirPerformance();

            pantalla.FinalizarBatch();
            // Sonidos deshabilitados en la migración a MonoGame; vaciamos la cola si se usara.
            while (Sonido.colaSonidos.Count > 0)
            {
                Sonido.colaSonidos.Dequeue();
            }

            base.Draw(gameTime);
            timeDraw = DateTime.Now.Subtract(t0);

        }

        private void EscribirTexto(int x, int y, Object o)
        {
            EscribirTexto(x, y, o.ToString());
        }

        private void EscribirTexto(int x, int y, String texto)
        {
            pantalla.EscribirTexto(x, y, texto, System.Drawing.Color.White);
        }

        private void ImprimirPerformance()
        {
            iTimeTick = (int)(iTimeTick * .9 + timeTick.Milliseconds * .1);
            iTimeDraw = (int)(iTimeDraw * .9 + timeDraw.Milliseconds * .1);

            EscribirTexto(900, 400, iTimeTick);
            EscribirTexto(900, 450, iTimeDraw);

            if (DateTime.Now.Subtract(dtFPS).Seconds >= 1)
            {
                fps = frames;
                frames = 0;
                dtFPS = DateTime.Now;
            }
            frames++;
            EscribirTexto(900, 500, fps);
        }


        private void ImprimirTextoEspecies()
        {
            foreach (Especie especie in universo.Especies)
            {
                ventanaEspecies.Escribir(especie.nombre, System.Drawing.Color.Yellow);
                ventanaEspecies.Escribir("Cant.: " + especie.individuos.Count, System.Drawing.Color.GreenYellow);
                ventanaEspecies.Escribir("Comidas: " + especie.comidas, System.Drawing.Color.GreenYellow);
                ventanaEspecies.Escribir("Necesarias: " + especie.ComidasNecesarias(), System.Drawing.Color.GreenYellow);
                ventanaEspecies.Escribir("Record: " + ComidasMasEficiente(especie), System.Drawing.Color.GreenYellow);
                ventanaEspecies.Escribir(MasEficiente(especie), System.Drawing.Color.GreenYellow);
                ventanaEspecies.Escribir("", System.Drawing.Color.White);
            }
        }

        private int ComidasMasEficiente(Especie especieAnimal)
        {
            Animal masEficiente = (Animal)especieAnimal.masEficiente;

            if (masEficiente != null)
                return masEficiente.Comidas;
            else
                return 0;
        }

        private string MasEficiente(Especie especieAnimal)
        {
            Animal masEficiente = (Animal)especieAnimal.masEficiente;

            if (masEficiente != null)
                return masEficiente.Id;
            else
                return "";
        }

    }
}
