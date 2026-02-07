using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class AnimalVistaReaccion : AnimalQueMira
    {
        public AnimalVistaReaccion()
        {
            cerebro = new CerebroVistaReaccion();
        }
    }
}
