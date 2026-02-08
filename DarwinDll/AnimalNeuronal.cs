using System;
using System.Collections.Generic;
using System.Text;

namespace DarwinDLL
{
    public class AnimalNeuronal : AnimalQueMira
    {
        public AnimalNeuronal()
        {
            cerebro = new CerebroNeuronal();
        }
    }
}
