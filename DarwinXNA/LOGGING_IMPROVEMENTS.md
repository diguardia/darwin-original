# Mejoras al Sistema de Logging

## ?? Resumen de Cambios

Se ha mejorado completamente el sistema de logging para incluir timestamps, niveles de log, y mejor formato. Esto facilita enormemente el diagnóstico de problemas.

## ? Mejoras Implementadas

### 1. **Clase `Log` mejorada** (`DarwinDll\Log.cs`)

#### Características nuevas:
- ? **Timestamps precisos**: Fecha, hora y milisegundos en cada entrada
- ? **Niveles de log**: DEBUG, INFO, WARNING, ERROR, FATAL
- ? **Thread-safe**: Usa lock para prevenir problemas de concurrencia
- ? **Codificación UTF-8**: Soporte correcto para caracteres especiales
- ? **Cabecera de sesión**: Registra versión, plataforma y runtime al iniciar
- ? **Excepciones formateadas**: Stack traces claros y legibles
- ? **Optimizado para MonoGame**: No intenta escribir en consola (no disponible en WinExe)
- ? **Respaldo de errores**: Si falla el log principal, crea `log_error.txt`

#### Nuevos métodos:
```csharp
// Métodos por nivel
Log.Debug("Mensaje de depuración");
Log.Info("Información general");
Log.Warning("Advertencia");
Log.Error("Error");
Log.Fatal("Error crítico");

// Método para excepciones
Log.Exception(ex, "Contexto del error");

// Inicializar sesión con información
Log.InicializarSesion("DarwinXNA", "1.0");

// Método original aún funciona (por compatibilidad)
Log.escribir("mensaje");  // Se registra como ERROR por defecto
```

### 2. **Program.cs mejorado**

#### Logging estructurado en el flujo principal:
```csharp
Log.InicializarSesion("DarwinXNA", "1.0");
Log.Info("Iniciando aplicación Darwin XNA");
Log.Info("Cargando o creando universo...");
// ... más logs a lo largo de la ejecución
```

#### Mejor manejo de excepciones:
- Usa `Log.Exception()` para capturar stack traces completos
- **Muestra errores con MessageBox** (compatible con aplicaciones MonoGame)
- No usa `Console.ReadKey()` ni `Console.WriteLine()` (no funcionan en WinExe)
- Los errores se registran en `log.txt` y se muestran al usuario en un diálogo

### 3. **ViewLogs.ps1 mejorado**

#### Nuevas características:
```powershell
# Filtrar por nivel
.\ViewLogs.ps1 -Level ERROR     # Solo errores
.\ViewLogs.ps1 -Level FATAL     # Solo críticos
.\ViewLogs.ps1 -Level WARNING   # Solo advertencias

# Opciones existentes mejoradas
.\ViewLogs.ps1 -Lines 100       # Ver más líneas
.\ViewLogs.ps1 -Follow          # Seguir en tiempo real

# Ahora muestra resumen automático
# Cuenta: Fatales, Errores, Advertencias, Info
```

## ?? Formato del Log

### Antes:
```
System.InvalidOperationException: This image format is not supported
   at DarwinXNA.XNAFont.LoadFont...
```

### Ahora:
```
================================================================================
NUEVA SESIÓN: DarwinXNA v1.0
FECHA: 2026-02-08 16:30:42
PLATAFORMA: Microsoft Windows NT 10.0.22631.0
RUNTIME: .NET 10.0.0
================================================================================

[2026-02-08 16:30:42.123] [INFO   ] Iniciando aplicación Darwin XNA
[2026-02-08 16:30:42.145] [INFO   ] Cargando o creando universo...
[2026-02-08 16:30:42.234] [INFO   ] Nuevo universo inicializado
[2026-02-08 16:30:43.789] [ERROR  ] ================================================================================
CONTEXTO: Error de formato de archivo DDS
EXCEPCIÓN: NotSupportedException
MENSAJE: MonoGame no soporta archivos DDS...
STACK TRACE:
   at DarwinXNA.XNAFont.LoadFont(GraphicsDevice device, String strXML...
================================================================================
```

## ?? Ventajas

1. **Fácil de buscar**: Puedes buscar por fecha/hora exacta
2. **Filtrable**: Usa los niveles para ver solo lo que importa
3. **Contexto completo**: Sabes qué estaba haciendo la app cuando falló
4. **Profesional**: Formato estándar de la industria
5. **Diagnóstico rápido**: Resumen automático de errores
6. **Multiplataforma**: Funciona igual en Windows/Linux/Mac
7. **Compatible con MonoGame**: Optimizado para aplicaciones gráficas sin consola

## ?? Importante: Aplicaciones MonoGame

MonoGame genera aplicaciones **WinExe** (ventanas), no aplicaciones de consola. Por eso:

- ? `Console.ReadKey()` no funciona ? Causa `InvalidOperationException`
- ? `Console.WriteLine()` no se ve ? No hay consola visible
- ? `MessageBox.Show()` sí funciona ? Muestra diálogos al usuario
- ? `Log.escribir()` sí funciona ? Escribe en archivo log.txt

El sistema de logging está diseñado para:
1. **Registrar todo en `log.txt`** ? Diagnóstico completo
2. **Mostrar errores críticos con MessageBox** ? Usuario informado
3. **No depender de consola** ? Compatible con WinExe

## ?? Ejemplos de Uso

### Ver logs básicos:
```powershell
# Ver todo
.\ViewLogs.ps1

# Ver últimas 200 líneas
.\ViewLogs.ps1 -Lines 200
```

### Filtrar por nivel:
```powershell
# Solo errores graves
.\ViewLogs.ps1 -Level ERROR

# Solo advertencias
.\ViewLogs.ps1 -Level WARNING
```

### Búsqueda manual:
```powershell
# Buscar por fecha
Get-Content log.txt | Select-String "2026-02-08 16:30"

# Contar errores
(Get-Content log.txt | Select-String "\[ERROR").Count

# Ver errores y advertencias
Get-Content log.txt | Select-String "\[(ERROR|WARNING)"
```

### Seguimiento en tiempo real:
```powershell
# Seguir todo
.\ViewLogs.ps1 -Follow

# Seguir solo errores
.\ViewLogs.ps1 -Follow -Level ERROR
```

## ?? Compatibilidad

El sistema es **100% compatible** con código existente:
```csharp
// Código viejo sigue funcionando
Log.escribir("mensaje");  // Se registra como ERROR

// Código nuevo es más expresivo
Log.Info("mensaje");      // Se registra como INFO
```

## ?? Archivos Modificados

1. **`DarwinDll\Log.cs`**: Sistema de logging completo
2. **`DarwinXNA\Program.cs`**: Uso del nuevo logging
3. **`DarwinXNA\ViewLogs.ps1`**: Script mejorado con filtros
4. **`DarwinXNA\README.md`**: Documentación actualizada
5. **`README.md`**: Documentación general actualizada

## ?? Archivos Nuevos

- **`LOG_FORMAT_EXAMPLE.md`**: Ejemplos del nuevo formato

## ?? Próximos Pasos

1. ? Convierte `franklin.dds` a `franklin.png` (ver DDS_MIGRATION_GUIDE.md)
2. ? Ejecuta el programa
3. ? Si hay errores, revisa el log con `.\ViewLogs.ps1`
4. ? Usa filtros para enfocarte en problemas específicos

## ?? Tips

- Los logs se rotan por sesión (cada ejecución agrega al archivo)
- Si el log crece mucho, puedes borrarlo manualmente
- Usa `-Level ERROR` para ver solo problemas serios
- El resumen al final te da una vista rápida del estado

## ?? Futuras Mejoras Opcionales

- [ ] Rotación automática de logs por tamaño
- [ ] Logs separados por nivel (error.log, info.log)
- [ ] Compresión de logs antiguos
- [ ] Envío de logs críticos por email
- [ ] Dashboard web para ver logs

¡El sistema de logging ahora es profesional y fácil de usar! ??
