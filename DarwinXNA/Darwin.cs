using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DarwinDLL;
using Microsoft.Xna.Framework.Input;

namespace DarwinXNA
{
    public partial class Darwin : Form
    {
        private Universo universo;
        private bool salir;

        public Darwin()
        {
            InitializeComponent();
        }

        public void Iniciar(Universo unUniverso)
        {
            universo = unUniverso;
        }

        private void Darwin_Load(object sender, EventArgs e)
        {

        }
        
        internal void Ciclar()
        {
            salir = false;
            while (!salir)
            {
                universo.Tick();
                Application.DoEvents();
                this.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salir = true;
        }

        private void velocidad_ValueChanged(object sender, EventArgs e)
        {
            universo.velocidad = (int)velocidad.Value;
        }
    }
}