# Sistema de Configuración de Darwin XNA

## ?? Archivos de Configuración

### config.json
Archivo de configuración principal del juego. Se crea automáticamente en la primera ejecución.

```json
// Configuración de Darwin XNA
// Modifica estos valores y reinicia el juego para aplicar cambios
{
  // Ancho de la ventana (mínimo: 224)
  "screenWidth": 1024,
  // Alto de la ventana (mínimo: 600)
  "screenHeight": 768,
  // Pantalla completa al iniciar
  "fullscreen": false,
  // Sincronización vertical (reduce tearing)
  "vsync": true,
  // Velocidad de simulación inicial (1 = normal)
  "defaultSpeed": 1
}
// Nota: El terreno de juego será de 900x768 (124px reservados para UI)
```

## ?? Constantes del Juego

Definidas en **`GameConstants.cs`** - NO son configurables por el usuario:

```csharp
public const int PANEL_WIDTH = 124;  // Ancho del panel de información lateral
```

### Constantes Calculadas Automáticamente:
- **TerrainWidth** = ScreenWidth - 124
- **TerrainHeight** = ScreenHeight
- **PanelStartX** = TerrainWidth

## ?? Cálculos Automáticos

### Ejemplo 1: Resolución por defecto (1024x768)
```
Screen Width:   1024 px
Panel Width:    - 124 px
?????????????????????????
Terrain Width:    900 px
Terrain Height:   768 px
```

### Ejemplo 2: Resolución HD (1280x720)
```
Screen Width:   1280 px
Panel Width:    - 124 px
?????????????????????????
Terrain Width:   1156 px
Terrain Height:   720 px
```

### Ejemplo 3: Full HD (1920x1080)
```
Screen Width:   1920 px
Panel Width:    - 124 px
?????????????????????????
Terrain Width:   1796 px
Terrain Height:  1080 px
```

## ?? Opciones Configurables

### screenWidth
- **Descripción**: Ancho de la ventana del juego
- **Mínimo**: 224 px (124px panel + 100px juego mínimo)
- **Máximo**: 3840 px (4K)
- **Por defecto**: 1024
- **Nota**: El terreno será: `screenWidth - 124`

### screenHeight
- **Descripción**: Alto de la ventana del juego
- **Mínimo**: 600 px
- **Máximo**: 2160 px (4K)
- **Por defecto**: 768
- **Nota**: El terreno tendrá la misma altura

### fullscreen
- **Descripción**: Iniciar en modo pantalla completa
- **Valores**: `true` / `false`
- **Por defecto**: `false`
- **Nota**: Puedes alternar con `F` durante el juego

### vsync
- **Descripción**: Sincronización vertical
- **Valores**: `true` / `false`
- **Por defecto**: `true`
- **Beneficio**: Reduce el screen tearing
- **Costo**: Puede limitar FPS

### defaultSpeed
- **Descripción**: Velocidad de simulación inicial
- **Mínimo**: 1
- **Máximo**: 100
- **Por defecto**: 1
- **Nota**: 1 = velocidad normal, mayor = más rápido

## ?? Cómo Cambiar la Configuración

### Método 1: Editar config.json manualmente

1. **Cierra el juego** si está ejecutándose
2. **Abre `config.json`** con un editor de texto
3. **Modifica los valores** deseados
4. **Guarda el archivo**
5. **Ejecuta el juego** - los cambios se aplicarán automáticamente

### Método 2: Eliminar config.json

Si eliminas `config.json`, se creará uno nuevo con valores por defecto en la siguiente ejecución.

## ?? Resoluciones Recomendadas

### 4:3 (Clásico)
```json
{ "screenWidth": 1024, "screenHeight": 768 }   // SVGA
{ "screenWidth": 1280, "screenHeight": 960 }   // SXGA-
{ "screenWidth": 1400, "screenHeight": 1050 }  // SXGA+
{ "screenWidth": 1600, "screenHeight": 1200 }  // UXGA
```

### 16:10 (Widescreen)
```json
{ "screenWidth": 1280, "screenHeight": 800 }   // WXGA
{ "screenWidth": 1440, "screenHeight": 900 }   // WXGA+
{ "screenWidth": 1680, "screenHeight": 1050 }  // WSXGA+
{ "screenWidth": 1920, "screenHeight": 1200 }  // WUXGA
```

### 16:9 (HD)
```json
{ "screenWidth": 1280, "screenHeight": 720 }   // HD
{ "screenWidth": 1366, "screenHeight": 768 }   // WXGA
{ "screenWidth": 1600, "screenHeight": 900 }   // HD+
{ "screenWidth": 1920, "screenHeight": 1080 }  // Full HD
{ "screenWidth": 2560, "screenHeight": 1440 }  // 2K
{ "screenWidth": 3840, "screenHeight": 2160 }  // 4K
```

## ?? Validación Automática

La configuración se valida automáticamente al cargar:

- **Ancho muy pequeño** ? Se ajusta al mínimo (224px)
- **Alto muy pequeño** ? Se ajusta al mínimo (600px)
- **Valores muy grandes** ? Se limitan a 4K
- **Velocidad inválida** ? Se ajusta a 1-100

Los ajustes se registran en `log.txt`:
```
[WARNING] Ancho 100 es muy pequeño, ajustando a 224
[INFO] Configuración cargada: 224x600
```

## ??? Ubicación de Archivos

```
DarwinXNA/
??? config.json              ? Configuración del usuario
??? GameConfig.cs            ? Clase de configuración
??? GameConstants.cs         ? Constantes del juego
??? Program.cs               ? Carga configuración
??? Game1.cs                 ? Aplica configuración
??? bin/Debug/.../
    ??? config.json          ? Se crea aquí al ejecutar
    ??? log.txt              ? Registra cambios de config
```

## ?? Logs de Configuración

Cada vez que se carga la configuración, se registra en `log.txt`:

```
[INFO] Cargando configuración desde config.json
[INFO] Configuración cargada: 1280x720
[INFO] Dimensiones calculadas - Terreno: 1156x720, Panel: 124px
[INFO] Configuración de pantalla aplicada: 1280x720, Fullscreen: false, VSync: true
[INFO] Ventanas UI posicionadas - Panel inicia en x=1156
```

## ?? Posicionamiento Automático de UI

Las ventanas de información se posicionan automáticamente basándose en el tamaño de pantalla:

```csharp
// Se calculan automáticamente
ventanaSeleccion:
  X = TerrainWidth + 60
  Y = 60
  Tamaño = 110 x 100

ventanaEspecies:
  X = TerrainWidth + 60
  Y = 450
  Tamaño = 110 x 600
```

## ?? Resolución de Problemas

### El juego no lee mi configuración
- Verifica que `config.json` esté en la misma carpeta que el ejecutable
- Revisa `log.txt` para ver errores de carga
- Elimina `config.json` y déjalo recrearse

### La pantalla es muy pequeña/grande
- Edita `screenWidth` y `screenHeight` en `config.json`
- Respeta los límites mínimos/máximos

### El universo cargado tiene tamaño incorrecto
- El `save.xml` guardó el tamaño anterior
- Elimina `save.xml` para crear un universo nuevo con el tamaño actual

### Los pozos están mal posicionados
- Los pozos ahora se posicionan proporcionalmente
- Elimina `save.xml` para regenerarlos

## ?? Características Futuras (Pendientes)

- [ ] UI en el juego para cambiar configuración
- [ ] Perfiles de configuración múltiples
- [ ] Hotkeys personalizables
- [ ] Configuración de colores/temas
- [ ] Ajuste de volumen de audio

## ?? Referencias

- **GameConstants.cs**: Constantes no configurables
- **GameConfig.cs**: Sistema de configuración
- **Program.cs**: Carga y aplica configuración
- **Game1.cs**: Inicialización con configuración
- **Universo.cs**: Constructor con dimensiones personalizadas
