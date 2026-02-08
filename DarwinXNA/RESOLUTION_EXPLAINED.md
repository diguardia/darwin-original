# Resolución de Pantalla vs Universo en Darwin XNA

## ?? Resolución de Pantalla (MonoGame)

Definida en **`Game1.cs`** líneas 30-31:

```csharp
graphics.PreferredBackBufferWidth = 1024;
graphics.PreferredBackBufferHeight = 768;
```

**Tamaño de ventana**: `1024 x 768 píxeles`

## ?? Tamaño del Universo (Terreno)

Definido en **`Universo.cs`** línea 37:

```csharp
terreno = new Terreno(new PuntoEntero(900, 768));
```

**Tamaño del terreno**: `900 x 768 píxeles`

## ?? Comparación

| Componente | Ancho | Alto | Ubicación |
|------------|-------|------|-----------|
| **Ventana MonoGame** | 1024 | 768 | `Game1.cs:30-31` |
| **Universo (Terreno)** | 900 | 768 | `Universo.cs:37` |
| **Diferencia** | +124 | 0 | Espacio lateral derecho |

## ?? Layout de la Pantalla

```
????????????????????????????????????????????????????
?  Área de Juego (Terreno)    ?  Panel Derecho    ?
?                              ?                   ?
?         900 x 768            ?    124 x 768      ?
?                              ?                   ?
?  Aquí se dibuja el universo ?  Ventanas de info ?
?                              ?                   ?
????????????????????????????????????????????????????
                1024 x 768 total
```

## ?? Elementos en el Panel Derecho

Posicionados en **`Game1.cs`** líneas 42-44:

```csharp
ventanaSeleccion = new Ventana(pantalla, new Punto(960, 60), 110, 100);
ventanaEspecies = new Ventana(pantalla, new Punto(960, 450), 110, 600);
```

### Ventana de Selección
- **Posición**: `(960, 60)` - Esquina superior derecha
- **Tamaño**: `110 x 100`
- **Propósito**: Mostrar información del objeto seleccionado

### Ventana de Especies
- **Posición**: `(960, 450)` - Parte inferior derecha
- **Tamaño**: `110 x 600`
- **Propósito**: Listar especies y estadísticas

## ??? Seguidor (Cámara/Cursor)

Inicializado en **`Game1.cs`** líneas 46-47:

```csharp
seguidor.posicion.x = 1000;
seguidor.posicion.y = 700;
```

**Posición inicial**: Fuera del área del terreno (en el panel derecho)

## ?? Resumen de Coordenadas

### Área del Terreno (Jugable)
```
x: 0 ? 900
y: 0 ? 768
```

### Panel de Información
```
x: 900 ? 1024
y: 0 ? 768
```

### Ventanas de UI
```
Selección:  x=960, y=60  (110x100)
Especies:   x=960, y=450 (110x600)
```

## ?? Por Qué Diferentes Tamaños

1. **Universo = 900 píxeles**: Área donde se simula y dibuja el mundo
2. **Pantalla = 1024 píxeles**: Incluye espacio adicional (124px) para UI/información
3. **Diseño intencional**: 
   - Zona izquierda = Simulación/gameplay
   - Zona derecha = Información/estadísticas

## ?? Para Cambiar la Resolución

### Cambiar tamaño de ventana:

**`Game1.cs`** líneas 30-31:
```csharp
graphics.PreferredBackBufferWidth = 1280;  // Nueva anchura
graphics.PreferredBackBufferHeight = 960;  // Nueva altura
```

### Cambiar tamaño del universo:

**`Universo.cs`** línea 37:
```csharp
terreno = new Terreno(new PuntoEntero(1156, 960));  // Nuevo tamaño
// Nota: Deja espacio para el panel derecho (ej: 1280 - 124 = 1156)
```

### Reposicionar ventanas de UI:

**`Game1.cs`** líneas 42-44:
```csharp
ventanaSeleccion = new Ventana(pantalla, new Punto(1180, 60), 110, 100);
ventanaEspecies = new Ventana(pantalla, new Punto(1180, 450), 110, 600);
// Ajustar X para que estén en el nuevo panel derecho
```

## ?? Ejemplo: Resolución 1280x960

```csharp
// Game1.cs
graphics.PreferredBackBufferWidth = 1280;
graphics.PreferredBackBufferHeight = 960;

// Universo.cs
terreno = new Terreno(new PuntoEntero(1156, 960));  // 1280 - 124 = 1156

// Game1.cs (ventanas)
ventanaSeleccion = new Ventana(pantalla, new Punto(1180, 75), 110, 100);
ventanaEspecies = new Ventana(pantalla, new Punto(1180, 562), 110, 600);
```

## ?? Consideraciones

1. **Aspecto 4:3**: La resolución actual (1024x768) es formato 4:3
2. **Panel fijo**: El panel derecho usa ~124 píxeles
3. **Coordenadas**: El terreno usa coordenadas 0-based (0,0) = esquina superior izquierda
4. **Seguidor**: Debe estar dentro de los límites del terreno para funcionar correctamente

## ?? Respuesta Rápida

**¿Son iguales?** 
- ? No
- **Ventana**: 1024 x 768
- **Universo**: 900 x 768
- **Diferencia**: 124 píxeles reservados para UI lateral

**¿Dónde se definen?**
- **Ventana**: `Game1.cs:30-31`
- **Universo**: `Universo.cs:37`
