# Ejemplo de log.txt con el nuevo formato

================================================================================
NUEVA SESIÓN: DarwinXNA v1.0
FECHA: 2026-02-08 16:30:42
PLATAFORMA: Microsoft Windows NT 10.0.22631.0
RUNTIME: .NET 10.0.0
================================================================================

[2026-02-08 16:30:42.123] [INFO   ] Iniciando aplicación Darwin XNA
[2026-02-08 16:30:42.145] [INFO   ] Cargando o creando universo...
[2026-02-08 16:30:42.156] [INFO   ] No existe save.xml, creando nuevo universo
[2026-02-08 16:30:42.234] [INFO   ] Nuevo universo inicializado con valores por defecto
[2026-02-08 16:30:42.245] [INFO   ] Universo inicializado correctamente
[2026-02-08 16:30:42.256] [INFO   ] Iniciando juego MonoGame...
[2026-02-08 16:30:43.789] [ERROR  ] ================================================================================
CONTEXTO: Error de formato de archivo DDS
EXCEPCIÓN: NotSupportedException
MENSAJE: MonoGame no soporta archivos DDS. Por favor convierte 'C:\...\franklin.dds' a PNG.
Ejecuta el script ConvertDDStoPNG.ps1 o convierte manualmente el archivo a PNG.
STACK TRACE:
   at DarwinXNA.XNAFont.LoadFont(GraphicsDevice device, String strXML, String strDDS, SpriteBatch unSpriteBatch) in XNAFont.cs:line 94
   at DarwinXNA.PantallaXNA..ctor(GraphicsDeviceManager _graphics) in PantallaXNA.cs:line 29
   at DarwinXNA.Game1.Initialize() in Game1.cs:line 37
================================================================================

## Ventajas del nuevo formato:

1. **Timestamps precisos**: Cada entrada tiene fecha, hora y milisegundos
2. **Niveles de log**: Fácil de filtrar por tipo (DEBUG, INFO, WARNING, ERROR, FATAL)
3. **Información de sesión**: Registra versión, plataforma y runtime al inicio
4. **Stack traces formateados**: Las excepciones se muestran de forma clara y legible
5. **Thread-safe**: Uso de lock para evitar problemas con acceso concurrente
6. **Codificación UTF-8**: Soporte correcto para caracteres especiales
7. **También en consola**: Los errores se muestran tanto en log como en consola

## Ejemplo de filtrado:

```powershell
# Ver solo errores
Get-Content log.txt | Select-String "\[ERROR"

# Ver errores y fatales
Get-Content log.txt | Select-String "\[(ERROR|FATAL)"

# Ver todo excepto DEBUG
Get-Content log.txt | Select-String "\[DEBUG" -NotMatch

# Ver actividad reciente
Get-Content log.txt -Tail 100

# Contar errores
(Get-Content log.txt | Select-String "\[ERROR").Count
```

## Ejemplo con el script mejorado:

```powershell
# Ver solo errores
.\ViewLogs.ps1 -Level ERROR

# Ver solo fatales
.\ViewLogs.ps1 -Level FATAL

# Seguir solo errores en tiempo real
.\ViewLogs.ps1 -Follow -Level ERROR

# Ver últimas 200 líneas con resumen
.\ViewLogs.ps1 -Lines 200
```
