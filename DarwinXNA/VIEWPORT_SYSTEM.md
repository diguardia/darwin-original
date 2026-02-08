# ? Universo y Pantalla Independientes - Implementado

## ?? Cambio Realizado

El universo ahora tiene dimensiones independientes de la pantalla. El universo puede ser mucho más grande que la ventana visible, y solo se dibuja la porción que está dentro del "viewport" (área visible).

### Antes:
```
Pantalla:  1024 x 768
Terreno:    900 x 768  (pantalla - 124px panel)
```
**El terreno siempre tenía el mismo tamaño que el área visible**

### Ahora:
```
Pantalla:  1024 x 768
Viewport:   900 x 768  (área visible del universo)
Universo:  2000 x 2000 (puede ser cualquier tamaño)
```
**El universo puede ser más grande - solo ves una porción a la vez**

## ?? Arquitectura Implementada

### 1. **Sistema de Cámara (`Camera.cs`)**

Nueva clase que gestiona la visualización:

```csharp
public class Camera
{
    public float X { get; set; }              // Posición de la cámara en el universo
    public float Y { get; set; }
    public int ViewportWidth { get; }         // Tamaño del área visible
    public int ViewportHeight { get; }
    public int UniverseWidth { get; }         // Tamaño del universo completo
    public int UniverseHeight { get; }
}
```

**Funcionalidades:**
- ? Convierte coordenadas del mundo ? pantalla
- ? Convierte coordenadas de pantalla ? mundo
- ? Detecta si un objeto es visible (culling)
- ? Centra la cámara en un punto
- ? Limita la cámara para no salir del universo

### 2. **Configuración Actualizada (`GameConfig.cs`)**

```csharp
// Pantalla
public int ScreenWidth { get; set; } = 1024;
public int ScreenHeight { get; set; } = 768;

// Universo (nuevo)
public int UniverseWidth { get; set; } = 2000;
public int UniverseHeight { get; set; } = 2000;

// Área visible (calculado)
public int ViewportWidth => ScreenWidth - PANEL_WIDTH;
public int ViewportHeight => ScreenHeight;
```

### 3. **Dibujo con Transformación (`PantallaXNA.cs`)**

Todos los métodos de dibujo ahora:
1. Verifican si el objeto es visible (culling)
2. Transforman coordenadas del mundo a pantalla
3. Solo dibujan si está en el viewport

```csharp
public void PegarImagen(Punto centro, string clave)
{
    // 1. Culling - no dibujar si está fuera
    if (!camera.IsVisible(worldX, worldY)) return;
    
    // 2. Transformar coordenadas
    Vector2 screenPos = camera.WorldToScreen(worldX, worldY);
    
    // 3. Dibujar en posición de pantalla
    ForegroundBatch.Draw(tex, screenPos, Color.White);
}
```

## ?? Ejemplo de Configuración

### Universo pequeño (igual que antes):
```json
{
  "screenWidth": 1024,
  "screenHeight": 768,
  "universeWidth": 900,    // Mismo que viewport
  "universeHeight": 768
}
```
**Resultado**: Todo el universo visible, como antes.

### Universo grande (nuevo):
```json
{
  "screenWidth": 1024,
  "screenHeight": 768,
  "universeWidth": 2000,   // Más grande que viewport
  "universeHeight": 2000
}
```
**Resultado**: Solo ves una porción del universo a la vez.

### Universo enorme:
```json
{
  "screenWidth": 1600,
  "screenHeight": 900,
  "universeWidth": 5000,
  "universeHeight": 5000
}
```
**Resultado**: Viewport de 1476x900, universo de 5000x5000.

## ?? Funcionamiento

### Inicialización:
```
1. Crear universo de 2000x2000
2. Crear cámara con viewport 900x768
3. Centrar cámara en el centro del universo (1000, 1000)
4. La cámara muestra las coordenadas del universo (550, 616) a (1450, 1384)
```

### Durante el Juego:
```
Objeto en universo: (1200, 800)
?
Camera.WorldToScreen(1200, 800)
?
Posición en pantalla: (650, 184)  [porque cámara está en (550, 616)]
?
Se dibuja en pantalla en (650, 184)
```

### Culling (Optimización):
```
Objeto en universo: (50, 50)
?
Camera.IsVisible(50, 50) ? false (fuera del viewport)
?
No se dibuja (ahorro de rendimiento)
```

## ? Optimización de Rendimiento

### Culling Implementado:
Todos los métodos de dibujo verifican visibilidad antes de dibujar:

- `DibujarCuadrado()` - culling por rectángulo
- `DibujarPunto()` - culling por punto
- `PegarImagen()` - culling por posición/tamaño

**Beneficio**: Si el universo es 5000x5000 pero solo ves 900x768, solo se procesan los objetos visibles.

## ?? Archivos Modificados

### Creados:
1. **`Camera.cs`** (nuevo) - Sistema de cámara/viewport

### Modificados:
2. **`GameConfig.cs`**:
   - Agregado `UniverseWidth` y `UniverseHeight`
   - Cambiado `TerrainWidth/Height` ? `ViewportWidth/Height`
   - Validación de dimensiones de universo

3. **`GameConstants.cs`**:
   - Actualizado `CalculateDimensions()` para viewport
   - Renombrado `TerrainWidth/Height` ? `ViewportWidth/Height`

4. **`Program.cs`**:
   - Usa `UniverseWidth/Height` en lugar de `TerrainWidth/Height`

5. **`Game1.cs`**:
   - Agregada instancia de `Camera`
   - Pasa `camera` a `PantallaXNA`
   - Cámara centrada en el universo al iniciar
   - Tecla 'N' ahora recrea la cámara también

6. **`PantallaXNA.cs`**:
   - Constructor acepta `Camera`
   - Todos los métodos de dibujo usan transformación de cámara
   - Implementado culling en todos los métodos

7. **`config.json.example`**:
   - Agregados campos de universo

## ?? Estado Actual

### ? Lo que funciona:
- Universo puede ser de cualquier tamaño
- Viewport muestra solo la porción visible
- Transformación de coordenadas mundo ? pantalla
- Culling para optimizar rendimiento
- UI se dibuja en coordenadas de pantalla (no se mueve)
- Validación de dimensiones

### ?? Por implementar (siguiente paso):
- Scroll/movimiento de cámara con teclado
- Seguimiento de objetos con la cámara
- Mini-mapa para orientación
- Indicadores de límites del universo

## ?? Conceptos Clave

### Viewport:
El "viewport" es el área visible del universo. Es como una ventana que mira hacia el mundo más grande.

```
Universo (2000x2000):
???????????????????????????????
?                             ?
?    ?????????????           ?
?    ? Viewport  ?           ?  ? Solo esta parte es visible
?    ? (900x768) ?           ?
?    ?????????????           ?
?                             ?
???????????????????????????????
```

### Coordenadas del Mundo vs Pantalla:
- **Mundo**: Coordenadas absolutas en el universo (ej: 1500, 1200)
- **Pantalla**: Coordenadas relativas al viewport (ej: 450, 300)

### Culling:
No dibujar objetos que están fuera del viewport. Mejora el rendimiento enormemente en universos grandes.

## ?? Ejemplo de Uso

```csharp
// Crear universo grande
var universo = new Universo(5000, 5000);

// Crear cámara que muestra 900x768
var camera = new Camera(900, 768, 5000, 5000);

// Centrar en un punto
camera.CenterOn(2500, 2500);  // Centro del universo

// Dibujar objeto
var objeto = new Punto(2450, 2520);  // Cerca del centro
var screenPos = camera.WorldToScreen(objeto.x, objeto.y);
// screenPos = (400, 268) - cerca del centro del viewport

// Objeto lejano
var lejano = new Punto(100, 100);
if (camera.IsVisible(lejano.x, lejano.y))
{
    // No se ejecuta - está fuera del viewport
}
```

## ?? Configuración Recomendada

### Para pruebas:
```json
{
  "screenWidth": 1024,
  "screenHeight": 768,
  "universeWidth": 2000,
  "universeHeight": 2000
}
```

### Para juego normal:
```json
{
  "screenWidth": 1280,
  "screenHeight": 720,
  "universeWidth": 3000,
  "universeHeight": 3000
}
```

### Para mundo masivo:
```json
{
  "screenWidth": 1920,
  "screenHeight": 1080,
  "universeWidth": 8000,
  "universeHeight": 8000
}
```

## ?? Ventajas del Sistema

1. **Escalabilidad**: Universos de cualquier tamaño
2. **Rendimiento**: Culling automático
3. **Flexibilidad**: Viewport independiente del universo
4. **Preparado**: Base lista para scroll
5. **Optimizado**: Solo dibuja lo visible
6. **Configurable**: Todo en config.json

## ?? Próximos Pasos

Una vez que confirmes que esto funciona correctamente, podemos agregar:

1. **Scroll con teclado**: WASD o flechas
2. **Scroll con mouse**: Arrastrar o edge scrolling
3. **Zoom**: Acercar/alejar (cambiar tamaño del viewport)
4. **Mini-mapa**: Vista general del universo
5. **Camera lerp**: Movimiento suave
6. **Follow target**: Cámara siguiendo un objeto
7. **Límites visuales**: Indicadores del borde del universo

¿Listo para probar y luego agregar el scroll? ??
