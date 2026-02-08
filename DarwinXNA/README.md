# DarwinXNA – Cliente MonoGame 3.8 (DesktopGL)

Frontend Windows construido con MonoGame 3.8.1 DesktopGL. Consume el motor `DarwinDll` y dibuja la simulación en 2D.

## Estructura
- `DarwinXNA.csproj`: ejecutable `WinExe` dirigido a `x86`, referencia `DarwinDll`.
- `Content/`: proyecto de contenido anidado (`Content.contentproj`) con sprites y sonidos.
- `imagenes/`, `Fonts/`, `Win/`: assets para texturas, tipografías y bancos de sonido.

## Controles en tiempo de ejecución
- `N`: crear un nuevo universo con especies y pozos por defecto.
- `S`: guardar el estado actual a `save.xml`.
- `R`: recargar el estado serializado en memoria.
- `F`: alternar pantalla completa.
- Click izquierdo: mover el `Seguidor` para inspeccionar otro punto del mapa.
- `Esc`: salir.

## Ciclo del juego
- `Game1.Update`: delega en `Universo.Tick()`, mueve el seguidor y gestiona atajos de guardado/recarga.
- `Game1.Draw`: limpia el backbuffer, pinta el terreno, dibuja cada especie, panel lateral con estadísticas (tiempo, timers, conteo y eficiencia por especie) y reproduce la cola de sonidos.
- Ventanas auxiliares (`Ventana` y `VentanaSeleccion`) muestran detalles y texto con `XNAFont`.

## Configuración

### Archivo config.json

El juego crea automáticamente un archivo `config.json` donde puedes configurar:

- **screenWidth**: Ancho de la ventana (default: 1024)
- **screenHeight**: Alto de la ventana (default: 768)
- **fullscreen**: Pantalla completa (default: false)
- **vsync**: Sincronización vertical (default: true)
- **defaultSpeed**: Velocidad de simulación inicial (default: 1)

**⚠️ Ubicación importante**: El archivo se encuentra en:
```
bin/Debug/net10.0-windows7.0/config.json
```
No edites el del directorio raíz, ya que el juego usa el que está en el directorio de salida.

**Nota**: El terreno de juego se calcula automáticamente como `screenWidth - 124` píxeles (124px están reservados para el panel de información lateral).

### Ejemplo de config.json:
```json
{
  "screenWidth": 1280,
  "screenHeight": 720,
  "fullscreen": false,
  "vsync": true,
  "defaultSpeed": 1
}
```

**Para aplicar cambios**:
1. Edita `bin/Debug/net10.0-windows7.0/config.json`
2. Guarda el archivo
3. Ejecuta `dotnet run`

Ver guías completas:
- [CONFIGURATION_GUIDE.md](CONFIGURATION_GUIDE.md) - Guía de configuración
- [CONFIG_LOCATION_FIX.md](CONFIG_LOCATION_FIX.md) - Ubicación del archivo

## Compilación y ejecución

### Primera vez (configuración completa):
```bash
# 1. Verificar formatos de imagen
.\CheckImageFormats.ps1

# 2. Convertir archivos DDS problemáticos (o crear placeholders)
.\ConvertDDStoPNG.ps1         # Intenta conversión automática
.\CreatePlaceholderImages.ps1  # Si falla, crea imágenes temporales

# 3. Compilar la fuente SpriteFont
cd Content
mgcb .\Content.mgcb /platform:DesktopGL
cd ..

# 4. Compilar y ejecutar
dotnet restore
dotnet build
dotnet run
```

### Ejecuciones posteriores:
```bash
dotnet build
dotnet run
```

**Nota**: 
- El logging mejorado te dirá **exactamente** qué imagen está fallando si hay problemas
- Revisa `log.txt` para diagnóstico completo: `.\ViewLogs.ps1 -Level ERROR`
- Solo necesitas recompilar el contenido si modificas `Content/Franklin.spritefont`

Ver guía completa: [PROBLEMA_DDS_RESUELTO.md](PROBLEMA_DDS_RESUELTO.md)

## Diagnóstico y Logs

### Leer los logs
Si el programa termina inesperadamente, consulta el archivo de logs:

- **Ubicación**: `log.txt` en el directorio donde se ejecuta (ej: `bin/Debug/net10.0/` o la carpeta del proyecto).
- **Formato**: Cada línea incluye timestamp (fecha/hora), nivel de log (INFO, ERROR, etc.) y mensaje.
  ```
  [2026-02-08 16:30:45.123] [INFO   ] Iniciando aplicación Darwin XNA
  [2026-02-08 16:30:45.456] [ERROR  ] Error al cargar textura
  ```
- **Niveles de log**:
  - `DEBUG`: Información de depuración detallada
  - `INFO`: Información general del flujo de la aplicación
  - `WARNING`: Advertencias que no detienen la ejecución
  - `ERROR`: Errores que pueden causar fallas
  - `FATAL`: Errores críticos que detienen la aplicación
- **Contenido**: Registra excepciones y errores capturados en el bloque `try-catch` de `Program.Main()`.
- **Cómo revisar**:
  ```powershell
  # Usando el script de ayuda
  .\ViewLogs.ps1           # Ver últimas 50 líneas
  .\ViewLogs.ps1 -Lines 100  # Ver últimas 100 líneas
  .\ViewLogs.ps1 -Follow     # Seguir en tiempo real
  
  # O manualmente
  Get-Content log.txt -Tail 50  # Ver últimas líneas del log
  Get-Content log.txt -Wait      # Seguir el log en tiempo real
  cat log.txt                    # Ver todo el log
  
  # Filtrar por nivel de error
  Get-Content log.txt | Select-String "ERROR"
  Get-Content log.txt | Select-String "FATAL"
  ```

### Problemas comunes
- **El juego termina sin mostrar ventana**: Revisa `log.txt` para ver excepciones de MonoGame, archivos de contenido faltantes o errores de inicialización.
- **Error "This image format is not supported" o "unknown image type"**: Los archivos `.dds` no son compatibles con MonoGame. Ver sección [Migración de DDS a PNG](#migración-de-dds-a-png) más abajo.
- **Error "Cannot read keys when console input has been redirected"**: Esto indica que el código intenta usar `Console.ReadKey()` en una aplicación MonoGame (WinExe). Ya está corregido - los errores se muestran con `MessageBox`.
- **Errores de Content.mgcb**: Asegúrate de que las carpetas `imagenes/`, `Fonts/` y `Win/` existen y contienen los assets necesarios.
- **save.xml corrupto**: Elimina el archivo `save.xml` para forzar la creación de un universo nuevo.

## Migración de DDS a PNG

MonoGame **no soporta archivos DDS** (DirectDraw Surface) que eran comunes en XNA. Debes convertir los archivos `.dds` a `.png`:

**NOTA**: Para fuentes, usa **SpriteFont** en lugar de archivos DDS. Ver [Guía de Fuentes](#fuentes-en-monogame) más abajo.

### Opción 1: Script automático (requiere ImageMagick)
```powershell
# 1. Instalar ImageMagick desde: https://imagemagick.org/script/download.php
# 2. Ejecutar el script de conversión
.\ConvertDDStoPNG.ps1
```

### Opción 2: Conversión online
1. Ve a https://www.aconvert.com/image/dds-to-png/
2. Sube los archivos `.dds` de la carpeta `Fonts/`
3. Descarga los archivos PNG generados
4. Guárdalos en la misma carpeta `Fonts/` con el mismo nombre (ejemplo: `franklin.dds` → `franklin.png`)

### Opción 3: Usar GIMP
1. Descarga GIMP: https://www.gimp.org/downloads/
2. Abre el archivo `Fonts/franklin.dds`
3. Exporta como `Fonts/franklin.png`

El código ya está preparado para usar archivos PNG automáticamente cuando existan.

## Fuentes en MonoGame

MonoGame usa **SpriteFont** para las fuentes, no archivos DDS bitmap.

### Sistema Moderno (Ya implementado)

Este proyecto ya usa SpriteFont correctamente:

```csharp
// En PantallaXNA.cs
SpriteFont font = content.Load<SpriteFont>("Franklin");
spriteBatch.DrawString(font, "Texto", position, Color.White);
```

### Configuración

Archivo `Content/Franklin.spritefont`:
- Define la fuente del sistema a usar (ej: Arial, Franklin Gothic, etc.)
- Configura tamaño, estilo y caracteres incluidos
- Se compila automáticamente con el Content Pipeline

### Personalizar Fuentes

Edita `Content/Franklin.spritefont` para cambiar:
```xml
<FontName>Arial</FontName>      <!-- Fuente del sistema -->
<Size>16</Size>                 <!-- Tamaño en puntos -->
<Style>Bold</Style>             <!-- Regular, Bold, Italic -->
```

### Más Detalles

Ver guía completa: [MONOGAME_FONTS_GUIDE.md](MONOGAME_FONTS_GUIDE.md)

## Notas
- El proyecto se diseñó para resolución 1024x768; se ajusta en `Game1` (`PreferredBackBufferWidth/Height`).
- Audio XACT sigue deshabilitado; la cola de sonidos se drena sin reproducir porque los bancos XGS originales no se migraron.
