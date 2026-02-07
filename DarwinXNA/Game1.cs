
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using DarwinDLL;
#endregion

namespace DarwinXNA
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static ContentManager content;
        GraphicsDeviceManager graphics;
        public Universo universo;
        PantallaXNA pantalla;
        private Seguidor seguidor = new Seguidor();
        Ventana ventanaSeleccion;
        Ventana ventanaEspecies;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        public Game1(Universo unUniverso)
        {
            universo = unUniverso;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferMultiSampling = false;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Bgr32;
            this.IsFixedTimeStep = true;
            this.IsMouseVisible = true;
       //     graphics.ToggleFullScreen();
            content = new ContentManager(Services);
        }

        protected override void Initialize()
        {
            pantalla = new PantallaXNA(graphics, content);
            ventanaSeleccion = new Ventana(pantalla, new Punto(960, 60), 110, 100);
            ventanaSeleccion.Titulo = "Seleccionado";
            ventanaEspecies = new Ventana(pantalla, new Punto(960, 450), 110, 600);
            ventanaEspecies.Titulo = "Especies";
            seguidor.posicion.x = 1000;
            seguidor.posicion.y = 700;

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
                universo = new Universo();
                universo.InicializarPorDefault();
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
            while (Sonido.colaSonidos.Count > 0)
            {
                soundBank.PlayCue(Sonido.colaSonidos.Dequeue ());
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