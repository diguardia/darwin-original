using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DarwinDLL
{
    // Sistema de logging mejorado con timestamps y niveles
    public class Log  
    {
        private static Dictionary<string, DateTime> t0 = new Dictionary<string,DateTime>();
        private static Dictionary<string, double> tiempos = new Dictionary<string, double>();
        private static readonly object lockObj = new object();
        
        public enum LogLevel
        {
            DEBUG,
            INFO,
            WARNING,
            ERROR,
            FATAL
        }

        public static void escribir(int numero)
        {
            escribir(numero.ToString());
        }
        
        public static void escribir(string texto) 
        {
            escribir(texto, LogLevel.ERROR);
        }
        
        public static void escribir(string texto, LogLevel nivel)
        {
            lock (lockObj)
            {
                try
                {
                    using (StreamWriter archivo = new StreamWriter("log.txt", true, Encoding.UTF8))
                    {
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        string nivelStr = nivel.ToString().PadRight(7);
                        string linea = $"[{timestamp}] [{nivelStr}] {texto}";
                        
                        archivo.WriteLine(linea);
                    }
                }
                catch (Exception ex)
                {
                    // Si falla el log, intentar escribir en archivo de respaldo
                    try
                    {
                        using (StreamWriter respaldo = new StreamWriter("log_error.txt", true))
                        {
                            respaldo.WriteLine($"[{DateTime.Now}] ERROR EN LOG PRINCIPAL: {ex.Message}");
                            respaldo.WriteLine($"Mensaje original: {texto}");
                        }
                    }
                    catch
                    {
                        // Si todo falla, no hacer nada para evitar crash
                    }
                }
            }
        }
        
        // Métodos de conveniencia para diferentes niveles
        public static void Debug(string texto)
        {
            escribir(texto, LogLevel.DEBUG);
        }
        
        public static void Info(string texto)
        {
            escribir(texto, LogLevel.INFO);
        }
        
        public static void Warning(string texto)
        {
            escribir(texto, LogLevel.WARNING);
        }
        
        public static void Error(string texto)
        {
            escribir(texto, LogLevel.ERROR);
        }
        
        public static void Fatal(string texto)
        {
            escribir(texto, LogLevel.FATAL);
        }
        
        // Método para escribir excepciones con stack trace formateado
        public static void Exception(Exception ex, string contexto = "")
        {
            lock (lockObj)
            {
                var sb = new StringBuilder();
                sb.AppendLine(new string('=', 80));
                if (!string.IsNullOrEmpty(contexto))
                {
                    sb.AppendLine($"CONTEXTO: {contexto}");
                }
                sb.AppendLine($"EXCEPCIÓN: {ex.GetType().Name}");
                sb.AppendLine($"MENSAJE: {ex.Message}");
                if (ex.InnerException != null)
                {
                    sb.AppendLine($"EXCEPCIÓN INTERNA: {ex.InnerException.GetType().Name}");
                    sb.AppendLine($"MENSAJE INTERNO: {ex.InnerException.Message}");
                }
                sb.AppendLine("STACK TRACE:");
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine(new string('=', 80));
                
                escribir(sb.ToString(), LogLevel.ERROR);
            }
        }
        
        // Inicializar el log con una cabecera
        public static void InicializarSesion(string aplicacion, string version)
        {
            lock (lockObj)
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine(new string('=', 80));
                sb.AppendLine($"NUEVA SESIÓN: {aplicacion} v{version}");
                sb.AppendLine($"FECHA: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"PLATAFORMA: {Environment.OSVersion}");
                sb.AppendLine($"RUNTIME: .NET {Environment.Version}");
                sb.AppendLine(new string('=', 80));
                
                try
                {
                    using (StreamWriter archivo = new StreamWriter("log.txt", true, Encoding.UTF8))
                    {
                        archivo.WriteLine(sb.ToString());
                    }
                }
                catch
                {
                    // Si falla, no hacer nada
                }
            }
        }



        public static void iniciarTimer(string key)
        {
            if (!t0.ContainsKey (key))
            {
                t0.Add(key, DateTime.Now);
            }
            else
            {
                t0[key] = DateTime.Now;
            }
        }

        public static void detenerTimer(string key)
        {
            TimeSpan dt;
            dt = DateTime.Now.Subtract (t0[key]);
            double ticks = dt.TotalMilliseconds;

            if (tiempos.ContainsKey(key))
            {
                tiempos[key] = tiempos[key] * .99 + ticks * .01;
            }
            else
            {
                tiempos.Add(key, ticks);
            }
        }

        public static void escribirTimers(Ventana ventanaSeleccion)
        {
            foreach (string key in tiempos.Keys)
            {
                ventanaSeleccion.Escribir(key + ": " + Math.Round (tiempos[key], 4), System.Drawing.Color.White);
            }
        }

        public static String getTimers()
        {
            String s = "";
            foreach (string key in tiempos.Keys)
            {
                s = s + key + ": " + Math.Round(tiempos[key], 1) + "\n";
            }
            return s;
        }
    } 
    
    
} 
