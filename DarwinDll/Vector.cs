using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
namespace DarwinDLL
{
    // TRANSMISSINGCOMMENT: Class Vector
    public class Vector  
    { 
        // TRANSMISSINGCOMMENT: Method Permutar
        public static double[] Permutar( double[] arr ) 
        { 
            int i = 0; 
            int[] permutaciones = new int[ 8 ]; 
            double[] res = new double[ 8 ]; 
            
            for ( i=arr.GetLowerBound( 0 ); i <= arr.GetUpperBound( 0 ); i++ ) 
            { 
                permutaciones[ i ] = i; 
            } 
            
            for ( i=arr.GetLowerBound( 0 ); i <= arr.GetUpperBound( 0 ); i++ ) 
            { 
                int indice1 = Random.EnteroEntreCeroY( 7 ); 
                int indice2 = Random.EnteroEntreCeroY( 7 ); 
                int swap = permutaciones[ indice1 ]; 
                permutaciones[ indice1 ] = permutaciones[ indice2 ]; 
                permutaciones[ indice2 ] = swap; 
            } 
            
            for ( i=arr.GetLowerBound( 0 ); i <= arr.GetUpperBound( 0 ); i++ ) 
            { 
                res[ i ] = arr[ permutaciones[ i ] ]; 
            } 
            
            return res; 
        } 
        
    } 
    
    
} 
