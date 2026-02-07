using System;
using DarwinDLL;

namespace DarwinXNA
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
//                Application.SetCompatibleTextRenderingDefault(false);
                Universo universo = CargarOCrearUniverso();
                
                //Darwin darwin = new Darwin();
                //darwin.Iniciar(universo);
                //darwin.Show();
                /*
                while (true)
                {*/
                    Game1 game = new Game1(universo);
                    game.Run();/*
                    game.Dispose();
                    game = null;

                    darwin.Ciclar();
                }*/
                    game.universo.Guardar ("save.xml");
            }
            catch (Exception ex)
            {
                Log.escribir(ex.ToString ());                
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

        private static Universo CargarOCrearUniverso()
        {
            Universo universo;

            if (System.IO.File.Exists("save.xml"))
            {
                universo = Universo.Cargar("save.xml");
            }
            else
            {
                universo = new Universo();
                universo.InicializarPorDefault();
            }

            return universo;
        }

    }
}

