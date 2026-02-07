using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Random
    public class Random  
    {
        private static System.Random R = new System.Random(DateTime.Now.Millisecond);

        // TRANSMISSINGCOMMENT: Method EnteroEntreCeroY
        public static int EnteroEntreCeroY( int Valor ) 
        {
            return R.Next(0, Valor + 1);
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Real
        public static Double Real(double entre, double Valor)
        {
            return R.NextDouble() * Math.Abs(Valor - entre) + entre;
        }

        public static float Float(float entre, float Valor)
        {
            return (float)R.NextDouble() * Math.Abs(Valor - entre) + entre;
        } 
        
        
        // TRANSMISSINGCOMMENT: Method Entero
        public static int Entero( int entre, int y ) 
        { 
            return EnteroEntreCeroY( y - entre ) + entre; 
        }

        public static int RangoSimétricoSinElCero(int MaxAbsoluto)
        {
            return Signo() * Entero(1, MaxAbsoluto);
        }

        public static int Signo()
        {
            return Entero(1, 2) * 2 - 3;
        }



        internal static bool Bool()
        {
            return (EnteroEntreCeroY(1) == 0);
        }
    } 
    
    
} 
