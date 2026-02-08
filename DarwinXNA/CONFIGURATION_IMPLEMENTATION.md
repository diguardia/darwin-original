# ? Sistema de Configuración Implementado

## ?? Resumen

Se ha implementado un sistema completo de configuración para Darwin XNA que permite personalizar la resolución de pantalla y otras opciones del juego.

## ?? Características Implementadas

### 1. **Archivo de Configuración (`config.json`)**
- ? Configuración de resolución de pantalla
- ? Modo pantalla completa
- ? VSync
- ? Velocidad de simulación por defecto
- ? Se crea automáticamente con valores por defecto
- ? Validación automática de valores

### 2. **Constantes del Juego (`GameConstants.cs`)**
- ? `PANEL_WIDTH = 124` - Ancho fijo del panel de información
- ? Cálculo automático de dimensiones del terreno
- ? Posicionamiento de ventanas UI
- ? Información de versión

### 3. **Clase de Configuración (`GameConfig.cs`)**
- ? Carga/guardado de configuración
- ? Validación de valores
- ? Valores por defecto
- ? Cálculo automático del tamaño del terreno
- ? Logging de cambios

### 4. **Cambios en el Código Existente**

#### Program.cs
- ? Carga configuración al iniciar
- ? Pasa configuración al universo y al juego
- ? Usa constantes para nombres y versiones

#### Game1.cs
- ? Acepta configuración en el constructor
- ? Aplica resolución de pantalla
- ? Posiciona ventanas UI automáticamente
- ? Centra el seguidor en el terreno

#### Universo.cs (DarwinDll)
- ? Constructor que acepta dimensiones personalizadas
- ? Pozos posicionados proporcionalmente
- ? Tamaño del terreno accesible públicamente

#### Terreno.cs (DarwinDll)
- ? Campo `tamaño` ahora es público

#### Content/Franklin.spritefont
- ? Tamaño de fuente reducido de 14 a 11 puntos

## ?? Cómo Funciona

### Flujo de Inicialización:
```
1. Program.Main()
   ?
2. GameConfig.Load()
   ?
3. config.ApplyToConstants()
   ?
4. GameConstants.CalculateDimensions()
   ?
5. new Universo(width, height)
   ?
6. new Game1(universo, config)
   ?
7. Aplicar configuración de gráficos
```

### Cálculo de Dimensiones:
```
Screen Width = 1024 px (configurable)
Panel Width  = 124 px  (constante)
????????????????????????????????????
Terrain Width = 900 px (calculado)
Terrain Height = 768 px (= screen height)
```

## ?? Archivos Creados

1. **`GameConstants.cs`** (235 líneas)
   - Constantes del juego
   - Cálculos automáticos

2. **`GameConfig.cs`** (175 líneas)
   - Sistema de configuración
   - Validación y logging

3. **`config.json.example`** (13 líneas)
   - Ejemplo de configuración
   - Con comentarios explicativos

4. **`CONFIGURATION_GUIDE.md`** (400+ líneas)
   - Guía completa del usuario
   - Ejemplos de configuración
   - Resoluciones recomendadas

## ?? Archivos Modificados

1. **`Program.cs`**
   - Carga y aplica configuración
   - Pasa config a universo y juego
   - Usa GameConstants

2. **`Game1.cs`**
   - Acepta GameConfig en constructor
   - Aplica configuración de gráficos
   - Posiciona UI dinámicamente

3. **`Universo.cs`** (DarwinDll)
   - Constructor con dimensiones
   - Pozos posicionados dinámicamente

4. **`Terreno.cs`** (DarwinDll)
   - Campo tamaño público

5. **`Content/Franklin.spritefont`**
   - Tamaño reducido a 11pt

6. **`README.md`**
   - Sección de configuración
   - Referencia a guía completa

## ?? Ejemplo de Uso

### Configuración por Defecto (1024x768):
```json
{
  "screenWidth": 1024,
  "screenHeight": 768,
  "fullscreen": false,
  "vsync": true,
  "defaultSpeed": 1
}
```
? **Terreno**: 900 x 768

### Configuración HD (1280x720):
```json
{
  "screenWidth": 1280,
  "screenHeight": 720,
  "fullscreen": false,
  "vsync": true,
  "defaultSpeed": 1
}
```
? **Terreno**: 1156 x 720

### Configuración Full HD (1920x1080):
```json
{
  "screenWidth": 1920,
  "screenHeight": 1080,
  "fullscreen": true,
  "vsync": true,
  "defaultSpeed": 2
}
```
? **Terreno**: 1796 x 1080

## ?? Valores Constantes (No Configurables)

| Constante | Valor | Descripción |
|-----------|-------|-------------|
| PANEL_WIDTH | 124 | Ancho del panel lateral |
| INFO_WINDOW_OFFSET_X | 60 | Offset X de ventana info |
| INFO_WINDOW_OFFSET_Y | 60 | Offset Y de ventana info |
| INFO_WINDOW_WIDTH | 110 | Ancho ventana info |
| INFO_WINDOW_HEIGHT | 100 | Alto ventana info |
| SPECIES_WINDOW_OFFSET_X | 60 | Offset X de ventana especies |
| SPECIES_WINDOW_OFFSET_Y | 450 | Offset Y de ventana especies |
| SPECIES_WINDOW_WIDTH | 110 | Ancho ventana especies |
| SPECIES_WINDOW_HEIGHT | 600 | Alto ventana especies |
| VERSION | "1.0" | Versión del juego |
| GAME_NAME | "DarwinXNA" | Nombre del juego |

## ?? Validación Automática

La configuración se valida al cargar:

```csharp
// Ancho mínimo: 224px (124 panel + 100 juego)
if (ScreenWidth < 224) ScreenWidth = 224;

// Alto mínimo: 600px
if (ScreenHeight < 600) ScreenHeight = 600;

// Máximos: 4K
if (ScreenWidth > 3840) ScreenWidth = 3840;
if (ScreenHeight > 2160) ScreenHeight = 2160;

// Velocidad: 1-100
if (DefaultSpeed < 1) DefaultSpeed = 1;
if (DefaultSpeed > 100) DefaultSpeed = 100;
```

## ?? Logging

Todos los cambios de configuración se registran en `log.txt`:

```
[INFO] Iniciando aplicación DarwinXNA v1.0
[INFO] Cargando configuración desde config.json
[INFO] Configuración cargada: 1280x720
[INFO] Dimensiones calculadas - Terreno: 1156x720, Panel: 124px
[INFO] Configuración de pantalla aplicada: 1280x720, Fullscreen: false, VSync: true
[INFO] Ventanas UI posicionadas - Panel inicia en x=1156
[INFO] Nuevo universo inicializado: 1156x720
```

## ? Para Ejecutar

```powershell
# 1. Compilar fuente (solo si cambió)
cd Content
mgcb .\Content.mgcb /platform:DesktopGL
cd ..

# 2. Compilar y ejecutar
dotnet build
dotnet run

# El juego creará config.json automáticamente si no existe
```

## ?? Tamaño de Fuente Reducido

**Antes**: 14 puntos
**Ahora**: 11 puntos

Esto permite mostrar más información en las ventanas laterales sin que se vea apretado.

## ?? Compatibilidad con Saves

Si cambias la resolución y tienes un `save.xml` existente:
- El sistema detecta el cambio de tamaño
- Crea un nuevo universo con las nuevas dimensiones
- Registra un warning en el log

```
[WARNING] Tamaño de terreno cambió de 900x768 a 1156x720
[INFO] Creando nuevo universo con tamaño actualizado
```

## ? Ventajas del Sistema

1. **Flexible**: Cualquier resolución soportada
2. **Automático**: Cálculos de terreno y UI automáticos
3. **Validado**: No permite valores inválidos
4. **Logged**: Todos los cambios registrados
5. **Simple**: JSON fácil de editar
6. **Robusto**: Valores por defecto si falla la carga
7. **Proporcional**: Pozos y elementos se ajustan automáticamente

## ?? Documentación

- **`CONFIGURATION_GUIDE.md`**: Guía completa del sistema
- **`config.json.example`**: Ejemplo con comentarios
- **`README.md`**: Sección de configuración
- **`RESOLUTION_EXPLAINED.md`**: Explicación de resoluciones

## ?? Resultado Final

**El juego ahora tiene un sistema de configuración profesional:**
- ? Resolución personalizable
- ? Constantes bien organizadas
- ? Cálculos automáticos
- ? Validación robusta
- ? Logging completo
- ? Documentación exhaustiva
- ? Fuentes de tamaño apropiado

**¡Todo listo para cualquier tamaño de pantalla!** ??
